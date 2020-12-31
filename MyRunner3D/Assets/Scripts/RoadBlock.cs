using UnityEngine;
#pragma warning disable 0649
public class RoadBlock : MonoBehaviour
{
    [SerializeField] private GameObject coins;

    private GameManager gameManager;
    private Vector3 moveVec;
    private int CoinsChange = 33;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        moveVec = new Vector3(-1, 0, 0);
        coins.SetActive(Random.Range(0, 101) <= CoinsChange);
    }
    private void Update()
    {
        if (gameManager.CanPlay)
            transform.Translate(moveVec * Time.deltaTime * gameManager.MoveSpeed);
    }
}
