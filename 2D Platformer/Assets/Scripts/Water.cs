using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (timer >= 0.5f)
            transform.localScale = new Vector3(1f, 1f, 1f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.GetComponent<Player>().inWater = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.GetComponent<Player>().inWater = false;
    }
}
