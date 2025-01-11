using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    public GameObject climbRestoreItem;
    public GameObject goldPowder;

    public HeighEstimate heighEstimate;

    public float targetHeightForClimbRestore;
    public float currentTargetForClimbRestore;
    
    public float targetHeightForEscalating;
    public float currentTargetForEscalating;

    public Transform cameraPosition;
    public Vector3 cameraOffset;

    private void Start()
    {
        currentTargetForClimbRestore = targetHeightForClimbRestore;
    }

    private void Update()
    {
        if (heighEstimate.currentHeight > currentTargetForClimbRestore)
        {
            currentTargetForClimbRestore += targetHeightForClimbRestore;

            if (Random.Range(0f, 1f) < 0.6f)
            {
                for (int i = 0; i < 3; i++)
                {
                    var ga = Instantiate(
                        climbRestoreItem, 
                        cameraPosition.position + cameraOffset + Vector3.right * Random.Range(-2f, 2f) + Vector3.up * Random.Range(0f, 2f), 
                        quaternion.identity);
                    
                    ga.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(-1f, 1f) + Vector2.up * 1, ForceMode2D.Impulse);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    var ga = Instantiate(
                        climbRestoreItem, 
                        cameraPosition.position + cameraOffset + Vector3.right * Random.Range(-2, 2), 
                        quaternion.identity);
                    
                    ga.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(-1f, 1f) + Vector2.up * 1, ForceMode2D.Impulse);
                }
            }
        }
    }

    public void SpawnGoldPowder(int n)
    {
        for (int i = 0; i < n; i++)
        {
            GameObject ga = Instantiate(goldPowder,
                cameraPosition.position + cameraOffset +
                Vector3.up * Random.Range(0f, 2f)
                , Quaternion.identity);
            ga.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(-1f, 1f) + Vector2.up * 1, ForceMode2D.Impulse);
        }
    }
}
