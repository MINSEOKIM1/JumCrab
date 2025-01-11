using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
    private float _yCameraOffset;

    private void Start()
    {
        _yCameraOffset = transform.position.y - playerTransform.position.y;
    }

    private void Update()
    {
        Vector3 targetPos;
        if (transform.position.y < 240)
        {
            if (playerTransform.GetComponent<PlayerBehavior>()._hitair ||
                playerTransform.GetComponent<PlayerBehavior>().die)
            {
                targetPos = playerTransform.position + Vector3.back * 10;
            }
            else
            {
                targetPos = new Vector3(0, playerTransform.position.y + _yCameraOffset, -10);
            }
        }
        else
        {
            targetPos = new Vector3(0, 240, -10);
        }
        


        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
        
    }
}
