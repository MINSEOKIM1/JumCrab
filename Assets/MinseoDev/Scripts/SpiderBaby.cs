using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBaby : MonoBehaviour
{
    public bool chasing;
    public Transform target;
    public float speed;
    public Transform camera;
    public float rotationRatio;
    public float time;
    
    private void Start()
    {
        chasing = true;
        StartCoroutine(Chase());
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (chasing)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(target.position - transform.position), rotationRatio * Time.deltaTime);
            
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(camera.position + Vector3.up * 5 - transform.position), rotationRatio * Time.deltaTime);
        }
        
        transform.Translate(transform.up * speed * Time.deltaTime);
       
    }

    IEnumerator Chase()
    {
        yield return new WaitForSeconds(time);

        chasing = false;
    }
}
