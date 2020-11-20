using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControll : MonoBehaviour
{
    public float speed = 5f;
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
            speed *= -1;
        if (collision.gameObject.tag == "Player")
            Destroy(this.gameObject);
    }
}
