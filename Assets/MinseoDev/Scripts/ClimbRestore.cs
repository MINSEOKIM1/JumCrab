using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbRestore : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Player")
        {
            col.collider.GetComponent<PlayerBehavior>()._climbGuage += 1;
            Destroy(gameObject);
        }
    }
}
