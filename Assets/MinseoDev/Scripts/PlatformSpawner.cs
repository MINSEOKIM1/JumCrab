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
        // Normal 100 당 나오는 플랫폼들 랜덤
        for (int i = (int) yInterval[0]; i < 20000; i+= (int) yInterval[0])
        {
            if (i > 6000) break;
            float random = Random.Range(0f, 1f);
            if (random < 0.5f)
            {
                var platform = Instantiate(platformPrefabs[0],
                    new Vector3(i % 2 == 0? Random.Range(minX, minX*0.3f) : Random.Range(maxX*0.3f, maxX), _startY + i / 40, 0),
                    Quaternion.identity);

                _platforms.Add(platform);
            }
            else if (random < 0.8f)
            {
                var platform = Instantiate(platformPrefabs[1],
                    new Vector3(Random.Range(minX, maxX) * 0.5f, _startY +  i / 40, 0),
                    Quaternion.identity);
            
                _platforms.Add(platform);
            }
            else
            {
                var platform = Instantiate(platformPrefabs[2],
                    new Vector3(Random.Range(minX, maxX), _startY + i / 40, 0),
                    Quaternion.identity);
            
                _platforms.Add(platform);
            }
        }
        
        // Vanishing per 220
        for (int i = (int)yInterval[1]; i < 15000; i+=(int)yInterval[1])
        {if (i > 6000) break;
        
            if (Random.Range(0f, 1f) > vanishingProbability) continue; 
            var platform = Instantiate(platformPrefabs[1],
                new Vector3(Random.Range(minX, maxX) * 0.5f, _startY + i / 40, 0),
                Quaternion.identity);
            
            _platforms.Add(platform);
        }

        // Descending 310
        for (int i = (int)yInterval[2]; i < 15000; i+=(int)yInterval[2])
        {
            if (i > 6000) break;
            if (Random.Range(0f, 1f) > descendingProbability) continue; 
            var platform = Instantiate(platformPrefabs[2],
                new Vector3(Random.Range(minX, maxX), _startY + i / 40, 0),
                Quaternion.identity);
            
            _platforms.Add(platform);
        }
    }
}
