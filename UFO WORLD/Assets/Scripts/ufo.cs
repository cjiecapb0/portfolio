using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ufo : MonoBehaviour
{
    public Rigidbody leftLegRb;
    public Rigidbody rigthLegRb;

    private SceneLoader sceneLoader;

    public float rotationMultipier = 0.8f;
    public float speed = 8f;
    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            leftLegRb.AddRelativeForce(Vector3.up * speed * rotationMultipier);
            rigthLegRb.AddRelativeForce(Vector3.up * speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigthLegRb.AddRelativeForce(Vector3.up * speed*rotationMultipier);
            leftLegRb.AddRelativeForce(Vector3.up * speed);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            leftLegRb.AddRelativeForce(Vector3.up * speed);
            rigthLegRb.AddRelativeForce(Vector3.up * speed);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            sceneLoader.RestartScene();
        }
        if (collision.gameObject.tag == "Friend")
        {
            sceneLoader.NextScene();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            sceneLoader.RestartScene();
    }
}
