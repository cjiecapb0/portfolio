using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;// одиночка
    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;
    void Awake()
    {
        S = this;//установить ссылку на объект одиночку
        line = GetComponent<LineRenderer>();//получить ссылку на LineRenderer
        line.enabled = false; //выключить LineRenderer, пока он не понадобится
        points = new List<Vector3>();//инициализировать список точек
    }
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)//если поле _poi содержит действительную ссылку, сбросить все остальные параметры состояния
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    public void Clear()//это метод вызывается непосредсвенно что-бы стирать лини
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }
    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;// вызывается для добавления точки в линии
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
            return;//если точка недостаточно далеко от предыдущей, просто выйти
        if (points.Count == 0) //если это точка запуска...
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;// для определения
            points.Add(pt + launchPosDiff);//...добавить дополнительный фрагмент линии,
            points.Add(pt);// что бы помочь лучше прицелится в будущем
            line.positionCount = 2;
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);// установить первые две точки
            line.enabled = true;
        }
        else//обычная последовательность добавление точки
        {
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }
    public Vector3 lastPoint //возвращаем местоположение последней добавленной точки
    {
        get
        {
            if (points == null)
                return (Vector3.zero);
            return (points[points.Count - 1]);
        }
    }
    void FixedUpdate()
    {
        if (poi == null)//если свойство poi сожержит пустое значеие, найти интересующий объект
        {
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return;//выйти если интересующий объект не найден
                }
            }
            else
            {
                return;//выйти если интересующий объект не найден
            }
        }
        AddPoint();// если интересующий объект найден, попытаться добавить точку с его координатами в каждом FixedUpdate
        if (FollowCam.POI == null)
            poi = null;
    }
}
