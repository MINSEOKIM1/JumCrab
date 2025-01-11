using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NormalPlatformSpriteRandom : MonoBehaviour
{
    public GameObject[] sp;
    private void Start()
    {
        int a = Random.Range(0, 3);
        sp[a].SetActive(true);
    }
}
