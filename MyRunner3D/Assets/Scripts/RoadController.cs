using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    private ObjectPool objectPool;
    private Transform myTransform;
    void Start()
    {
        myTransform = transform;
        objectPool = GetComponent<ObjectPool>();
        InvokeRepeating("SpawnRoad", 0f, 1.69f);
    }
    private void SpawnRoad()
    {
        GameObject road = objectPool.GetAvailabObject();
        road.transform.position = myTransform.position;
        road.SetActive(true);
    }
}
