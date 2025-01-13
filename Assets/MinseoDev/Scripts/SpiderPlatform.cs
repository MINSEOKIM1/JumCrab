using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderPlatform : MonoBehaviour
{
    public bool descending;
    public bool init;
    public Transform camera;
    public GameObject spider;

    public float descendingSpeed;

    private void Start()
    {
        camera = GameObject.FindWithTag("MainCamera").transform;
    }

    private void FixedUpdate()
    {
        if (descending)
        {
            transform.position += Vector3.down * descendingSpeed * Time.fixedDeltaTime;
        }
    }

    public IEnumerator Descend()
    {
        if (!init)
        {
            descending = true;
            yield return new WaitForSeconds(1f);
            descending = false;
            for (int i = 0; i < 50; i++)
            {
                GameObject go = Instantiate(spider,
                    camera.position + (Random.Range(0f, 1f) < 0.5f ? Vector3.right : Vector3.left) * 2.5f + Vector3.down * 8 + Vector3.forward * 10,
                    Quaternion.identity);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    
}
