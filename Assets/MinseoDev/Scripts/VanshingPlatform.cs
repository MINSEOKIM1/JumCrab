using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VanshingPlatform : MonoBehaviour
{
    [SerializeField] private float stumblingTime;
    
    private BoxCollider2D _box;
    private Rigidbody2D _rigid;

    private bool _collapsing = false;
    private float _elapsed = 2f;
    
    

    private void Start()
    {
        _box = GetComponent<BoxCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_collapsing) _elapsed -= Time.deltaTime;
    }

    public void Vanishing()
    {
        if (_collapsing == false)
        {
            _collapsing = true;
            StartCoroutine(Stumbling());
        }
    }

    IEnumerator Stumbling()
    {
        float time = 0;

        int a = 0;
        
        while (time < stumblingTime)
        {
            time += Time.deltaTime;
            

            if (a < 3)
            {
                transform.position += Vector3.right * 0.1f;
            }
            else
            {
                transform.position -= Vector3.right * 0.1f;
            }
            a++; 

            if (a >= 6) a = 0;

            yield return null;
        }

        _rigid.bodyType = RigidbodyType2D.Dynamic;
        _box.isTrigger = true;
        _rigid.AddTorque(Random.Range(0f, 3f));
        gameObject.layer = 0;
    }
}
