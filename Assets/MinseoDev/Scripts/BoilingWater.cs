using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilingWater : MonoBehaviour
{
    [SerializeField] private float upwardSpeed;
    [SerializeField] private float initialDamage;
    [SerializeField] private float dotDamage;

    private void Update()
    {
        transform.localScale += Vector3.up * upwardSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerBehavior>().gettingHit = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerBehavior>().gettingHit = false;
        }
    }
}
