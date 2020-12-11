using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject[] blocks;
    public Sprite buttonDown;
    private CircleCollider2D circleCollider2D;
    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MarkBox")
        {
            GetComponent<SpriteRenderer>().sprite = buttonDown;
            circleCollider2D.enabled = false;
            for (int i = 0; i < blocks.Length; i++)
            {
                Destroy(blocks[i]);
            }
        }
    }
}
