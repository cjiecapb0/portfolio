using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoad : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxPositionZ = -100f;
    private Transform myTransform;

    private void Start()
    {
        myTransform = transform;
    }
    private void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
        if (myTransform.position.z < maxPositionZ)
        {
            gameObject.SetActive(false);
        }
    }
}
