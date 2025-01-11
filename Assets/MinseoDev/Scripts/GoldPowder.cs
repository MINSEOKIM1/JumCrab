using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPowder : MonoBehaviour
{
    public float power;
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
            col.gameObject.GetComponent<PlayerBehavior>().Jump(power);
            Destroy(gameObject);
        }
    }
}
