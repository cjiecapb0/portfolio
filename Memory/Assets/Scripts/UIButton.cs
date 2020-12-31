using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string targetMassage;

    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnMouseEnter()
    {
        if(sprite != null)
            sprite.color = Color.cyan;
    }
    private void OnMouseExit()
    {
        if (sprite != null)
            sprite.color = Color.white;
    }
    private void OnMouseDown()
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }
    private void OnMouseUp()
    {
        transform.localScale = Vector3.one;
        if (targetObject != null)
            targetObject.SendMessage(targetMassage);
    }
}
