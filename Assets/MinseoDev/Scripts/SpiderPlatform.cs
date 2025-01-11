using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPlatform : MonoBehaviour
{
    public bool descending;
    public bool init;

    public float descendingSpeed;

    private void FixedUpdate()
    {
        if (descending)
        {
            transform.position += Vector3.down * descendingSpeed * Time.fixedDeltaTime;
        }
    }

    public IEnumerator Descend()
    {
        if (!init)
        {
            descending = true;
            yield return new WaitForSeconds(2f);
            descending = false;
        }
    }
}
