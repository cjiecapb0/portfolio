using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;//объект-одиночка Main
    [Header("Set in Inspector")]
    public GameObject[] prefabEmemis;//массив шаблонов Enemy
    public float enemySpawnPerSecond = 0.5f;//вражеских кораблей в секунду
    public float enemyDefaulPadding = 1.5f;//отступ для позиционирования
    private BoundsCheck bnbCheck;
    private void Awake()
    {
        S = this;
        //записать в bnbCheck ссылку на компонент BoundsCheck этого игрового объекта
        bnbCheck = GetComponent<BoundsCheck>();
        //вызвать SwapEnemy один раз (в 2 секунды при значениях по умолчанию)
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void SpawnEnemy()
    {
        //выбрать случайный шаблон Enemy для создания
        int ndx = Random.Range(0, prefabEmemis.Length);
        GameObject go = Instantiate<GameObject>(prefabEmemis[ndx]);
        //разместить вражеский корабль над экраном позиции х
        float enemyPadding = enemyDefaulPadding;
        if (go.GetComponent<BoundsCheck>() != null)
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        //установить начальные координатыы созданного корабля
        Vector3 pos = Vector3.zero;
        float xMin = -bnbCheck.camWidth + enemyPadding;
        float xMax = bnbCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bnbCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        //снова вызвать SpawnEnemy()
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }
}
