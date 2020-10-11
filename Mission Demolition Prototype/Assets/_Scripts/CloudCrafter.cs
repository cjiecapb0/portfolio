using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40;// Число облаков
    public GameObject cloudPrefab;//Шаблон для облаков
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;//Мин. масштаб каждого облака
    public float cloudScaleMax = 3;//Мфкс. масштаб каждого облака
    public float cloudSpeedMult = 0.5f;//Коэфф. скорости облаков
    private GameObject[] cloudInstances;
    private void Awake()
    {
        cloudInstances = new GameObject[numClouds];//создаем массив для облаков
        GameObject anchor = GameObject.Find("CloudAnchor");//найти родительский объект
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            cloud = Instantiate<GameObject>(cloudPrefab);//создать экземпляр cloudPrefab
            Vector3 cPos = Vector3.zero; //выбрать местоположение облака
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);//маштабировать облако
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);//меньшие облака должны быть ближе к земле
            cPos.z = 100 - 90 * scaleU;//меньшие облака должны быть дальше
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;//применить полученные значения координат и маштаба к облаку
            cloud.transform.SetParent(anchor.transform);//сделать облако дочерним
            cloudInstances[i] = cloud;//добавить облако в массив
        }
    }
    private void Update()
    {
        foreach (GameObject cloud  in cloudInstances)//обойти все созданные облака
        {
            float scaleVal = cloud.transform.localScale.x;//получить маштаб обалка
            Vector3 cPos = cloud.transform.position;//получить координаты облака
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;//увеличить скорость для ближних облаков
            if (cPos.x <= cloudPosMin.x)//если облако сместилось слишком далеко влево
                cPos.x = cloudPosMax.x;//переместить его далеко вправо
            cloud.transform.position = cPos;//примменить новые координаты к облаку
        }
    }
}
