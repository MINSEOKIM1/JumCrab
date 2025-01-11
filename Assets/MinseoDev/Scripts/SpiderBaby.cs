using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        speed += Random.Range(0f, 1f);
        rotationRatio += Random.Range(0f, 1f);
        StartCoroutine(Chase());
        target = GameObject.FindWithTag("Player").transform;
        camera = GameObject.FindWithTag("MainCamera").transform;
        transform.localScale *= Random.Range(0.8f, 1.2f);
    }

    private void Update()
    {
        if (chasing)
        {
            float angle =
                Mathf.Atan2(camera.position.y +  10 - transform.position.y, 0 + 2 * (transform.position.x > 0 ? 1 : -1) - transform.position.x) *
                Mathf.Rad2Deg - 90 + Random.Range(-20f, 20f);
            transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0, 0, angle), rotationRatio * Time.deltaTime);

        }
        else
        {
            float angle =
                Mathf.Atan2(camera.position.y +  10 - transform.position.y, 0 - transform.position.x) *
                Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0, 0, angle), rotationRatio * Time.deltaTime);
        }

        transform.position += transform.up * speed * Time.deltaTime;

    }

    IEnumerator Chase()
    {
        yield return new WaitForSeconds(time);

        chasing = false;
        yield return new WaitForSeconds(15);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerBehavior>().SingleDamage(8);
            Destroy(gameObject);
        }
    }
}
