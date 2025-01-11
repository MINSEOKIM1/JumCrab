using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendingPlatform : MonoBehaviour
{
    public bool descending;
    public bool init;

    public Animator animator;

    public float descendingSpeed;

    private void FixedUpdate()
    {
        if (descending)
        {
            animator.SetTrigger("descend");
            transform.position += Vector3.down * descendingSpeed * Time.fixedDeltaTime;
        }
    }
}
