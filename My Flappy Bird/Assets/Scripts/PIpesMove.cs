using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIpesMove : MonoBehaviour
{
    public float speed = 1f;
    void FixedUpdate()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        if (transform.position.x < -3.5f)
            Destroy(this.gameObject);
    }
}
