using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    [SerializeField] private float upwardSpeed;
    public BoilingWater boilingWater;

    private void Update()
    {
        transform.position += Vector3.up * boilingWater.upwardSpeed / 2 * Time.deltaTime;
    }
}
