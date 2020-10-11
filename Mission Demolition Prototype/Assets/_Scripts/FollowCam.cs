using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // Ссылка на обьект
    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    [Header("Set Dynamically")]
    public float camZ; //Желаемая координата Z камеры
    private void Awake()
    {
        camZ = this.transform.position.z;
    }
    private void FixedUpdate()
    {
        Vector3 destination;
        if (POI == null)
            destination = Vector3.zero;//выйти если нет интересующего обьекта вернуть P[0,0,0]
        else
        {
            destination = POI.transform.position;//получить позицию интересующего объекта
            if (POI.tag =="Projectile")// если интересующий объект сняряд
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())// если он стоит на месте
                {
                    POI = null;//вернуть иходные настройки поля зрения камеры в седующем кадре
                    return;
                }
            }
        }
        destination.x = Mathf.Max(minXY.x, destination.x);//минамальные значения
        destination.y = Mathf.Max(minXY.y, destination.y);//минамальные значения
        destination = Vector3.Lerp(transform.position, destination, easing);// определит точку между местоположением камеры и destination
        destination.z = camZ;// принудительно отдодвинуть камеру
        transform.position = destination;//поместить камеру в позицию destination
        Camera.main.orthographicSize = destination.y + 10;
    }
}
