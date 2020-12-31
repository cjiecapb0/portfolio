using System.Collections;
using UnityEngine;
#pragma warning disable 0649
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SwipeController swipeController;
    [SerializeField] private AudioSource audioSource ;
    [SerializeField] private AudioClip swipeAudio;
    [SerializeField] private AudioClip сoinAudio;

    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;    
    public Animator Anim;

    private int lineNumber = 1;
    private int lineCount = 2;

    private float firstLinePos = 1.5f;
    private float lineDistance = -1.5f;
    private float sideSpeed = 5f;
    private float jumpSpeed = 13f;

    private Vector3 ccCenterNorm = new Vector3(0, 0.81f, 0);
    private Vector3 ccCenterCrawl = new Vector3(0, 0.4f, 0.57f);
    private Vector3 startPosition;

    private float ccHeightNorm = 1.7f;
    private float ccHeightCrawl = 0.6f;

    private bool isCrawling = false;
    private bool wannaJump = false;
    private bool IsGrounded => Physics.Raycast(transform.position, Vector3.down, 0.05f);

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        swipeController.SwipeEvent += CheckInput;
    }
    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, Physics.gravity.y * 3, 0), ForceMode.Acceleration);
        if (wannaJump && IsGrounded)
        {
            Anim.SetTrigger("Jump");
            rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
            wannaJump = false;
        }
    }
    private void Update()
    {
        Vector3 newPos = transform.position;
        newPos.z = Mathf.Lerp(newPos.z, firstLinePos + (lineNumber * lineDistance),
                                                        Time.deltaTime * sideSpeed);
        transform.position = newPos;
    }
    private void CheckInput(SwipeController.SwipeType type)
    {
        if (IsGrounded && gameManager.CanPlay && !isCrawling)
        {
            if (type == SwipeController.SwipeType.UP)
            {
                audioSource.PlayOneShot(swipeAudio);
                wannaJump = true;
            }
            else if (type == SwipeController.SwipeType.DOWN)
            {
                audioSource.PlayOneShot(swipeAudio);
                StartCoroutine(DoCrawl());
            }
        }

        int sing = 0;

        if (!gameManager.CanPlay || isCrawling) return;

        if (type == SwipeController.SwipeType.LEFT)
        {
            audioSource.PlayOneShot(swipeAudio);
            sing = -1;
        }
        else if (type == SwipeController.SwipeType.RIGHT)
        {
            audioSource.PlayOneShot(swipeAudio);
            sing = 1;
        }
        else
            return;

        lineNumber += sing;
        lineNumber = Mathf.Clamp(lineNumber, 0, lineCount);
    }
    private IEnumerator DoCrawl()
    {
        isCrawling = true;
        Anim.SetBool("isCrawling", true);

        capsuleCollider.center = ccCenterCrawl;
        capsuleCollider.height = ccHeightCrawl;

        yield return new WaitForSeconds(1f);
        Anim.SetBool("isCrawling", false);

        capsuleCollider.center = ccCenterNorm;
        capsuleCollider.height = ccHeightNorm;
        yield return new WaitForSeconds(0.3f);
        isCrawling = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Trap") &&
            !collision.gameObject.CompareTag("DeathPlane") ||
            !gameManager.CanPlay)
            return;
        StartCoroutine(Death());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin")) return;
        audioSource.PlayOneShot(сoinAudio);
        gameManager.AddCoins(1);
        Destroy(other.gameObject);
    }
    private IEnumerator Death()
    {
        gameManager.CanPlay = false;
        Anim.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        Anim.ResetTrigger("Death");
        gameManager.ShowResult();
    }
    public void ResetPositon()
    {
        transform.position = startPosition;
        lineNumber = 1;
    }
}
