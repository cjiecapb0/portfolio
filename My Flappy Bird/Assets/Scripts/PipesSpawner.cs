using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipesSpawner : MonoBehaviour
{
    public GameObject pipesPrafab;
    public GameObject coinPrefab;
    public int num;
    void Start()
    {
        StartCoroutine(SpawnPipes());
    }
    private void Update()
    {
        num = Random.Range(1, 2);
    }
    IEnumerator SpawnPipes()
    {
        while (true)
        {   
            Instantiate(pipesPrafab, new Vector3(4f, Random.Range(0.5f, 5.34f), 0f), Quaternion.identity);
            yield return new WaitForSeconds(2f);
            if (num == 1)
                Instantiate(coinPrefab, new Vector3(4f, Random.Range(0.5f, 5.34f), 0f), Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }
}
