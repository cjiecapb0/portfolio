using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList;
    void Start()
    {
        basketList = new List<GameObject>();
        for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }
    }
    public void AppleDestroyed()
    {
        GameObject[] tApplleArray = GameObject.FindGameObjectsWithTag("Apple");
        int basketIndex = basketList.Count - 1;//Получить индек последней корзины
        GameObject tBasketGO = basketList[basketIndex];//Получить ссылку на Basket
        foreach (GameObject tGO in tApplleArray)
            Destroy(tGO);//Удалить все упавшие яблоки
        basketList.RemoveAt(basketIndex);//Исключить козину из списка
        Destroy(tBasketGO);//Уалить сам ировой обьект
        if (basketList.Count == 0)
            SceneManager.LoadScene("_Scene_2");//Перезапускает игру
    }

}
