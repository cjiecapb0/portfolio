using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(0, 40f*Time.deltaTime, 0));
    }
}
