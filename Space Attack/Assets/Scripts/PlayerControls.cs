using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float speed = 6;
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }
    void Update()
    {
        SpaceMovement();
    }
    private void SpaceMovement()
    {
        float horizonInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizonInput);
        transform.Translate(Vector3.up * Time.deltaTime * speed * vertInput);

        if (transform.position.y > 0)
            transform.position = new Vector3(transform.position.x, 0, 0);
        else if (transform.position.y < -4.07f)
            transform.position = new Vector3(transform.position.x, -4.07f, 0);
        if (transform.position.x > 9.7f)
            transform.position = new Vector3(-9.7f, transform.position.y, 0);
        else if (transform.position.x < -9.7f)
            transform.position = new Vector3(9.7f, transform.position.y, 0);
    }
}
