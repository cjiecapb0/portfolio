using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpHeight = 5f;
    private float groundRadius = 0.2f;
    private Rigidbody rbPlayer;
    private Transform myTransform;
    public Transform groundCheck;

    private bool isGrounded = false;
    private void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        myTransform = transform;

    }
    private void FixedUpdate()
    {
        CheckGround();
        PlayerControll();
    }
    private void CheckGround()
    {
        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundRadius);
        isGrounded = colliders.Length > 1;
    }
    private void PlayerControll()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rbPlayer.velocity = new Vector3(horizontalInput * speed, rbPlayer.velocity.y, rbPlayer.velocity.z);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rbPlayer.AddForce(myTransform.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
        }
    }
}
