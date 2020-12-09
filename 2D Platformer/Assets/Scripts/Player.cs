using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int curHp;
    private int coins = 0;
    private int maxHp = 3;
    public float speed = 5f;
    public float jumpHeight = 5f;
    private int gemCount = 0;

    public int Coins => coins;
    public int CurHP => curHp;

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private SpriteRenderer spriteRenderer;
    public Transform groundCheck;
    public GameObject blueGem, greenGem;
    private SpriteRenderer spriteBlueGem;
    private SpriteRenderer spriteGreenGem;
    public Main main;

    private bool isGrounded;
    private bool isHit;
    private bool key = false;
    private bool canTP = true;
    public bool inWater = false;
    private bool isClimding = false;
    private bool canHit = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteBlueGem = blueGem.GetComponent<SpriteRenderer>();
        spriteGreenGem = greenGem.GetComponent<SpriteRenderer>();
        curHp = maxHp;
    }
    private void Update()
    {
        if (inWater && !isClimding)
        {
            animator.SetInteger("State", 4);
            isGrounded = false;
            if (Input.GetAxis("Horizontal") != 0)
                Flip();
        }
        else
        {
            CheckGround();
            if (Input.GetAxis("Horizontal") == 0 && (isGrounded) && !isClimding)
                animator.SetInteger("State", 1);
            else
            {
                Flip();
                if (isGrounded && !isClimding)
                    animator.SetInteger("State", 2);
            }
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
    }
    private void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isClimding)
            animator.SetInteger("State", 3);
    }
    public void RecountHp(int deltaHp)
    {
        curHp += deltaHp;
        if (deltaHp < 0 && canHit)
        {
            StopCoroutine(OnHit());
            canHit = false;
            isHit = true;
            StartCoroutine(OnHit());
        }
        else if (curHp > maxHp)
        {
            curHp = maxHp;
        }
        if (curHp <= 0)
        {
            capsuleCollider2D.enabled = false;
            Invoke(nameof(Lose), 1.5f);
        }

    }
    IEnumerator OnHit()
    {
        if (isHit)
            spriteRenderer.color = new Color(1f, spriteRenderer.color.g - 0.04f, spriteRenderer.color.b - 0.04f);
        else
            spriteRenderer.color = new Color(1f, spriteRenderer.color.g + 0.04f, spriteRenderer.color.b + 0.04f);
        if (spriteRenderer.color.g == 1f)
        {
            StopCoroutine(OnHit());
            canHit = true;
        }
        if (spriteRenderer.color.g <= 0)
            isHit = false;
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }
    private void Lose()
    {
        main.GetComponent<Main>().Lose();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            key = true;
        }
        if (collision.gameObject.CompareTag("Door"))
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTP)
            {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTP = false;
                StartCoroutine(TPwait());
            }
            else if (key)
                collision.gameObject.GetComponent<Door>().Unlock();
        }
        if (collision.gameObject.CompareTag("Coins"))
        {
            Destroy(collision.gameObject);
            coins++;
        }
        if (collision.gameObject.CompareTag("Heart"))
        {
            Destroy(collision.gameObject);
            RecountHp(1);
        }
        if (collision.gameObject.CompareTag("Mushroom"))
        {
            Destroy(collision.gameObject);
            RecountHp(-1);
        }
        if (collision.gameObject.CompareTag("BlueGem"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(NoHit());
        }
        if (collision.gameObject.CompareTag("GreenGem"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(SpeedBonus());
        }
    }
    IEnumerator TPwait()
    {
        yield return new WaitForSeconds(1f);
        canTP = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimding = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            if (Input.GetAxis("Vertical") == 0)
            {
                animator.SetInteger("State", 5);
            }
            else
            {
                animator.SetInteger("State", 6);
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.5f * Time.deltaTime);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isClimding = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
        {
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));
        }
    }
    IEnumerator TrampolineAnim(Animator animator)
    {
        animator.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isJump", false);
    }
    IEnumerator NoHit()
    {
        gemCount++;
        blueGem.SetActive(true);
        CheckGems(blueGem);
        canHit = false;
        spriteBlueGem.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(Invis(spriteBlueGem, 0.02f));
        yield return new WaitForSeconds(1f);
        canHit = true;
        gemCount--;
        blueGem.SetActive(false);
        CheckGems(greenGem);
    }
    IEnumerator SpeedBonus()
    {
        gemCount++;
        greenGem.SetActive(true);
        CheckGems(greenGem);
        speed *= 2;
        spriteGreenGem.color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(9f);
        StartCoroutine(Invis(spriteGreenGem, 0.02f));
        yield return new WaitForSeconds(1f);
        speed /= 2;
        gemCount--;
        greenGem.SetActive(false);
        CheckGems(blueGem);
    }
    private void CheckGems(GameObject obj)
    {
        if (gemCount == 1)
            obj.transform.localPosition = new Vector3(0f, 0.35f, obj.transform.localPosition.z);
        else if (gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.25f, 0.35f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.25f, 0.35f, greenGem.transform.localPosition.z);
        }
    }
    IEnumerator Invis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.adaptiveModeThreshold - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(Invis(spr, time));
    }
}
