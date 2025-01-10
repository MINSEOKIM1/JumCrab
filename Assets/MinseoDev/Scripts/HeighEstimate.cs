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

    private void Start()
    {
        _startOffsetY = player.position.y;
    }

    private void Update()
    {
        float height = Mathf.Clamp(Mathf.Round((player.position.y - _startOffsetY )* 40), 0, Mathf.Infinity);
        text.text = "HEIGHT : " + height;
    }
}
