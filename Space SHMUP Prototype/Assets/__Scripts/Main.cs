using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;//объект-одиночка Main
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    [Header("Set in Inspector")]
    public GameObject[] prefabEmemis;//массив шаблонов Enemy
    public float enemySpawnPerSecond = 0.5f;//вражеских кораблей в секунду
    public float enemyDefaulPadding = 1.5f;//отступ для позиционирования
    public WeaponDefinition[] weaponDefinitions;
    private BoundsCheck bnbCheck;
    private void Awake()
    {
        S = this;
        //записать в bnbCheck ссылку на компонент BoundsCheck этого игрового объекта
        bnbCheck = GetComponent<BoundsCheck>();
        //вызвать SwapEnemy один раз (в 2 секунды при значениях по умолчанию)
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        //Словарь с ключами типа WeaponType
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
            WEAP_DICT[def.type] = def;
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
    ///<summary>
    ///Статическая функция, возвращающая WeaponDefinition из статического
    ///защитного поля WEAP_DICT класса Main.
    ///</summary>
    ///<returns>Экземпляр WeaponDefinition или, если нет такого определения
    ///для указанного WeaponType, возвращает новый экземпляр WeaponDefinition
    ///с типом none.</returns>
    ///<param name="wt">Тип оружия WeaponType, для которого требуется получить WeaponDefinition</param>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //проверить наличие указанного ключа в словаре
        //Попытка извлечь значение по отсутствующему ключу вызовет ошибку,
        //поэтому следующая инструкция ирает важную роль.
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        //Следующая инструкция возвращает новый экземпляр WeaponDefinition
        //с типом оружия WeaponType.none, что означает неуачную попытку
        // найти требуемое определение WeaponDefinition
        return (new WeaponDefinition());
    }
}
