using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBody : MonoBehaviour
{
    public BoilingWater bowa;

    private void Update()
    {
        transform.localScale += Vector3.up * bowa.upwardSpeed * Time.deltaTime;
    }

}
