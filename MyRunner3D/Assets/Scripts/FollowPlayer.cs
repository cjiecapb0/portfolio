using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649
public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 startDistance,
                    moveVec;

    private void Start()
    {
        startDistance = transform.position - target.position;
    }
    private void Update()
    {
        moveVec = target.position + startDistance;

        moveVec.z = 0;
        moveVec.y = startDistance.y;
        transform.position = moveVec;
    }
}
