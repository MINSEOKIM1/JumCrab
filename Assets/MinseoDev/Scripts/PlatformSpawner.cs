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
        // < 6000
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
        
        // phase 2
        // 80 0.6
        for (int i = 6000 + (int)yInterval[3]; i < 15000; i+=(int)yInterval[3])
        {if (i > 10000) break;
        
            if (Random.Range(0f, 1f) > 0.6)
            {
                var platform = Instantiate(platformPrefabs[6],
                    new Vector3(Random.Range(minX, maxX), _startY + i / 40, 0),
                    Quaternion.identity);
            
                _platforms.Add(platform);
            }
            else
            {
                var platform = Instantiate(platformPrefabs[3],
                    new Vector3(i % 2 == 0 ? Random.Range(minX, minX * 0.3f) : Random.Range(maxX * 0.3f, maxX)
                        , _startY + i / 40, 0),
                    Quaternion.identity);

                _platforms.Add(platform);
            }
        }
        
        // 100 0.4
        /*for (int i = 6000 + (int)yInterval[4]; i < 15000; i+=(int)yInterval[4])
        {if (i > 10000) break;
        
            if (Random.Range(0f, 1f) > 0.4) continue; 
            var platform = Instantiate(platformPrefabs[4],
                new Vector3(Random.Range(minX, maxX) * 0.5f, _startY + i / 40, 0),
                Quaternion.identity);
            
            _platforms.Add(platform);
        }*/
        
        // 420 0.8
        for (int i = 6000 + (int)yInterval[5]; i < 15000; i+=(int)yInterval[5])
        {if (i > 10000) break;
        
            if (Random.Range(0f, 1f) > 0.5) continue; 
            var platform = Instantiate(platformPrefabs[4],
                new Vector3(Random.Range(minX, maxX), _startY + i / 40, 0),
                Quaternion.identity);
            
            _platforms.Add(platform);
        }
        
        // 100 0.2
        for (int i = 6000 + (int)yInterval[6]; i < 15000; i+=(int)yInterval[6])
        {if (i > 10000) break;
        
            if (Random.Range(0f, 1f) > 0.2) continue; 
            var platform = Instantiate(platformPrefabs[5],
                new Vector3(Random.Range(minX, maxX), _startY + i / 40, 0),
                Quaternion.identity);
            
            _platforms.Add(platform);
        }
        
        // 140 0.2
        for (int i = 6000 + (int)yInterval[7]; i < 15000; i+=(int)yInterval[7])
        {if (i > 10000) break;
        
            if (Random.Range(0f, 1f) > 0.2) continue; 
            var platform = Instantiate(platformPrefabs[5],
                new Vector3(Random.Range(minX, maxX), _startY + i / 40, 0),
                Quaternion.identity);
            
            _platforms.Add(platform);
        }
        
        // 100 0.1
        /*for (int i = 6000 + (int)yInterval[8]; i < 15000; i+=(int)yInterval[8])
        {if (i > 10000) break;
        
            if (Random.Range(0f, 1f) > 0.1) continue; 
            var platform = Instantiate(platformPrefabs[6],
                new Vector3(Random.Range(minX, maxX), _startY + i / 40, 0),
                Quaternion.identity);
            
            _platforms.Add(platform);
        }*/
    }
}
