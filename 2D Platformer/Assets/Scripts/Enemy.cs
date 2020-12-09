using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D collider;
    public GameObject drop;
    private bool isHit = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isHit)
        {
            collision.gameObject.GetComponent<Player>().RecountHp(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 8f, ForceMode2D.Impulse);
        }
    }
    IEnumerator Death()
    {
        if (drop != null)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
        isHit = true;
        animator.SetBool("dead", true);
        rb.bodyType = RigidbodyType2D.Dynamic;
        collider.enabled = false;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
    public void StartDeath()
    {
        StartCoroutine(Death());
    }
}
