using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2f;
    private float timeToDisable = 10f;
    void Start()
    {
        StartCoroutine(SetDisabled());
    }
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    IEnumerator SetDisabled()
    {
        yield return new WaitForSeconds(timeToDisable);
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopCoroutine(SetDisabled());
        gameObject.SetActive(false);
    }
}
