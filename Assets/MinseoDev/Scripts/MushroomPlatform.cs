using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomPlatform : MonoBehaviour
{
    public bool descending;
    public bool triggered;

    public float descendingSpeed;

    private void FixedUpdate()
    {
        if (descending)
        {
            transform.position += Vector3.down * descendingSpeed * Time.fixedDeltaTime;
        }
    }

    public IEnumerator Trigger()
    {
        triggered = true;
        yield return new WaitForSeconds(1f);
        descending = true;
    }
}
