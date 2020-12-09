using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedAirPatrol : MonoBehaviour
{
    public Transform[] points;
    public float speed = 2f;
    public float waitTime = 3f;
    bool CanGo = true;
    private int i =1;
    private void Start()
    {
        gameObject.transform.position = new Vector3(points[0].position.x, points[0].position.y, transform.position.z);
    }
    void Update()
    {
        if (CanGo)
            transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

        if (transform.position == points[i].position)
        {
            if (i < points.Length - 1)
                i++;
            else i = 0;
            CanGo = false;
            StartCoroutine(Waiting());
        }
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        CanGo = true;
    }
}
