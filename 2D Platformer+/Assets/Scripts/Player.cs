using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float forseJump;

    [SerializeField] private Transform groundCheck;

    private Rigidbody2D rb;
    private bool isGrounded;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        PlayerController();
    }
    private void PlayerController()
    {
        FlipPlayer();
        GroundCheck();
        float horizontalAxes = Input.GetAxis("Horizontal");
        float jumpAxes = Input.GetAxis("Jump");

        rb.velocity = new Vector2(horizontalAxes * speed, rb.velocity.y);
        if (jumpAxes > 0 && isGrounded)
            rb.AddForce(transform.up * forseJump, ForceMode2D.Impulse);
    }
    private void FlipPlayer()
    {
        float horizontalAxes = Input.GetAxis("Horizontal");
        if (horizontalAxes > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (horizontalAxes < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
    private void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
    }
}
