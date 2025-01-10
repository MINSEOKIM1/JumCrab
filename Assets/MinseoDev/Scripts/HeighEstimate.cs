using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeighEstimate : MonoBehaviour
{
    public Transform player;
    public TMP_Text text;

    private float _startOffsetY;
    private float _timeElapsed = 0.1f;

    private void Start()
    {
        _startOffsetY = player.position.y;
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;
        
        float height = Mathf.Clamp(Mathf.Round((player.position.y - _startOffsetY )* 40), 0, Mathf.Infinity);
        float hpt = height / _timeElapsed;
        text.text = "HEIGHT : " + height + "\nHPT : " + hpt;
    }
}
