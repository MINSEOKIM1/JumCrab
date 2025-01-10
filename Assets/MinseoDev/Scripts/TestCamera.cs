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
        transform.position = new Vector3(0, playerTransform.position.y + _yCameraOffset, -10);
    }
}
