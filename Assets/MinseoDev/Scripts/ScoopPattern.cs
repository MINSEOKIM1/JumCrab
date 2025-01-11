using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScoopPattern : MonoBehaviour
{
    public float cooldown = 5f;
    public Transform cameraTransform;

    public float minX, maxX;

    private float _cooldownElapsed;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, cameraTransform.position.y, 0);
        _cooldownElapsed -= Time.deltaTime;
        if (_cooldownElapsed < 0)
        {
            _cooldownElapsed = cooldown;
            _animator.SetTrigger("attack");
            transform.position = new Vector3(Random.Range(minX, maxX), cameraTransform.position.y, 0);
        }
    }
}
