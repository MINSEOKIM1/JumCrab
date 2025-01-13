using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbRestore : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyThis());
    }

    private IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerBehavior>()._climbGuage += 1;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Player")
        {
            col.collider.GetComponent<PlayerBehavior>()._climbGuage += 1;
            Destroy(gameObject);
        }
    }
}
