using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsRoad;
    private int poolDepth = 11;
    private readonly List<GameObject> pool = new List<GameObject>();
    private void Awake()
    {   
        for (int i = 0; i < poolDepth; i++)
        {
            GameObject pooledObject = Instantiate(prefabsRoad[Random.Range(0,8)]);
            pooledObject.SetActive(false);
            pool.Add(pooledObject);
        }
    }
    public GameObject GetAvailabObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].activeInHierarchy == false)
                return pool[i];
        }
        return null;
    }
}
