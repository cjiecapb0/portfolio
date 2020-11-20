using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int CurHp;
    private int MaxHp = 3;
    public float Speed = 5f;
    public float jumpHeight = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private SpriteRenderer spriteRenderer;

    public Transform groundCheck;
    public Main main;

    private bool isGrounded;
    private bool isHit;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurHp = MaxHp;
    }
    private void Update()
    {
        CheckGround();
        if (Input.GetAxis("Horizontal") == 0 && isGrounded)
            animator.SetInteger("State", 1);
        else
        {
            Flip();
            if (isGrounded)
                animator.SetInteger("State", 2);
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed, rb.velocity.y);
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
        if (!isGrounded)
            animator.SetInteger("State", 3);
    }
    public void RecountHp(int deltaHp)
    {
        CurHp += deltaHp;
        if (deltaHp < 0)
        {
            StopCoroutine(OnHit());
            isHit = true;
            StartCoroutine(OnHit());
        }
        if (CurHp <= 0)
        {
            capsuleCollider2D.enabled = false;
            Invoke("Lose", 1.5f);
        }

    }
    IEnumerator OnHit()
    {
        if (isHit)
            spriteRenderer.color = new Color(1f, spriteRenderer.color.g - 0.04f, spriteRenderer.color.b - 0.04f);
        else
            spriteRenderer.color = new Color(1f, spriteRenderer.color.g + 0.04f, spriteRenderer.color.b + 0.04f);
        if (spriteRenderer.color.g == 1f)
            StopCoroutine(OnHit());
        if (spriteRenderer.color.g <= 0)
            isHit = false;
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }
    private void Lose()
    {
        main.GetComponent<Main>().Lose();
    }
}
