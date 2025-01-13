using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScoopPattern : MonoBehaviour
{
    public float cooldown = 5f;
    public Transform cameraTransform;
    public ItemSpawner itemSpawner;

    public BGM audioManager;

    public float minX, maxX;

    private float _cooldownElapsed = 10;

    private Animator _animator;
    
    public PlayerBehavior player;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, cameraTransform.position.y, 0);
        _cooldownElapsed -= Time.deltaTime;
        if (_cooldownElapsed < 0 && !player.clear && !player.die)
        {
            _cooldownElapsed = cooldown;
            audioManager.PlaySFX(0);
            _animator.SetTrigger("attack");
            transform.position = new Vector3(Random.Range(minX, maxX), cameraTransform.position.y, 0);
        }
    }

    public void OnSwing()
    {
        audioManager.PlaySFX(4);
    }

    public void SpwanGoldPowder()
    {
        audioManager.PlaySFX(2);
        float pr = Random.Range(0f, 1f);
        if (pr < 0.2f)
        {
            itemSpawner.SpawnGoldPowder(1);
        } else if (pr < 0.7)
        {
            itemSpawner.SpawnGoldPowder(2);
        }
        else
        {
            itemSpawner.SpawnGoldPowder(3);
        }
    }
}
