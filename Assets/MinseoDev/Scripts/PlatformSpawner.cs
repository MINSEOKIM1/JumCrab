using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{
    public Transform playerTransform;
    
    public GameObject[] platformPrefabs;
    public float minX, maxX;
    public float[] yInterval;

    public float vanishingProbability;
    public float descendingProbability;

    private float _startY;
    private List<GameObject> _platforms;

    private void Start()
    {
        _startY = playerTransform.position.y;

        _platforms = new List<GameObject>();
        
        // 맵 생성
        Create();
    }

    public void ReCreate()
    {
        foreach (var obj in _platforms)
        {
            Destroy(obj);
        }
        
        _platforms.Clear();
        
        Create();
    }

    private void Create()
    {
        // Normal
        for (int i = 1; i < 250; i++)
        {
            if (Random.Range(0f, 1f) < 0.9f)
            {
                var platform = Instantiate(platformPrefabs[0],
                    new Vector3(i % 2 == 0? Random.Range(minX, minX*0.7f) : Random.Range(maxX*0.7f, maxX), _startY + yInterval[0] * i / 40, 0),
                    Quaternion.identity);

                _platforms.Add(platform);
            }
            else
            {
                var platform = Instantiate(platformPrefabs[0],
                    new Vector3(Random.Range(minX, maxX), _startY + yInterval[0] * i / 40, 0),
                    Quaternion.identity);

                _platforms.Add(platform);
            }
        }
        
        // Vanishing per 260
        for (int i = 1; i < 250; i++)
        {
            var platform = Instantiate(platformPrefabs[1],
                new Vector3(Random.Range(minX, maxX) * 0.3f, _startY + yInterval[1] * i / 40, 0),
                Quaternion.identity);
            
            _platforms.Add(platform);
        }
        
        // per 100
        for (int i = 1; i < 250; i++)
        {
            if (Random.Range(0f, 1f) > vanishingProbability) continue; 
            var platform = Instantiate(platformPrefabs[1],
                new Vector3(Random.Range(minX, maxX)* 0.3f, _startY + yInterval[3] * i / 40, 0),
                Quaternion.identity);
            
            _platforms.Add(platform);
        }
        
        // Descending
        for (int i = 1; i < 250; i++)
        {
            if (Random.Range(0f, 1f) > descendingProbability) continue; 
            var platform = Instantiate(platformPrefabs[2],
                new Vector3(Random.Range(minX, maxX), _startY + yInterval[2] * i / 40, 0),
                Quaternion.identity);
            
            _platforms.Add(platform);
        }
    }
}
