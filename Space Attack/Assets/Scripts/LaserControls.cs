using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControls : MonoBehaviour
{
    private int laserSpeed = 6;
    void Start()
    {
        
    }
    void Update()
    {
        transform.Translate(Vector3.up * laserSpeed * Time.deltaTime);

        if (transform.position.y >= 5.7f)
            Destroy(this.gameObject, 1f);
    }
}
