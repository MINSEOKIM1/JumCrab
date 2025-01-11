using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeighEstimate : MonoBehaviour
{
    public Transform player;
    public TMP_Text text;

    public float currentHeight;

    private float _startOffsetY;
    private float _timeElapsed = 0.1f;

    private void Start()
    {
        _startOffsetY = player.position.y;
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;
        
        currentHeight = Mathf.Clamp(Mathf.Round((player.position.y - _startOffsetY) * 40), 0, Mathf.Infinity);
        float hpt = currentHeight / _timeElapsed;
        text.text = "Height : " + currentHeight;
    }
}
