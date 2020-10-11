using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;//статическое поле, доступное любому коду
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Projectile")//когда в область действия триггера попадает что-то
        {//проверить, является ли это чо-то снарядом
            Goal.goalMet = true;//если это снаряд писвоить полю goalMet значение true
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;//изменение альфа канала
        }
    }
}
