using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649
public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] roadBlockPrefabs;
    [SerializeField] private Transform playerTransform;

    private PlayerController playerController;

    private float startbBlockXPos;
    private int blockCount = 7;
    private float blockLength = 30;


    private List<GameObject> currentBlocks = new List<GameObject>();

    private void Awake()
    {
        playerController = playerTransform.GetComponent<PlayerController>();
        startbBlockXPos = playerTransform.position.x + 15;
        StartGame();
    }
    private void LateUpdate()
    {
        CheckForSpawn();
    }

    public void StartGame()
    {

        playerController.ResetPositon();
        foreach (var go in currentBlocks)
        {
            Destroy(go);
        }
        currentBlocks.Clear();
        for (int i = 0; i < blockCount - 4; i++)
            SpawnBlock(0);
        for (int i = 0; i < blockCount - 3; i++)
            SpawnBlock(Random.Range(0, roadBlockPrefabs.Length));
    }
    private void CheckForSpawn()
    {
        if (currentBlocks[0].transform.position.x - playerTransform.position.x < -25)
        {
            SpawnBlock(Random.Range(0, roadBlockPrefabs.Length));
            DestroyBlock();
        }
    }
    private void SpawnBlock(int numRoad)
    {
        GameObject block = Instantiate(roadBlockPrefabs[numRoad], transform);
        Vector3 blockPos;
        if (currentBlocks.Count > 0)
            blockPos = currentBlocks[currentBlocks.Count - 1].transform.position
                                                                + new Vector3(blockLength, 0, 0);
        else
            blockPos = new Vector3(startbBlockXPos, 0, 0);

        block.transform.position = blockPos;
        currentBlocks.Add(block);
    }
    private void DestroyBlock()
    {
        Destroy(currentBlocks[0]);
        currentBlocks.RemoveAt(0);
    }
}
