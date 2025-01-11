using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommonButton : MonoBehaviour
{
    private Image targetImage; 
    private bool isBlinking = false; 
    private bool isPointerOver = false;    
    [SerializeField] private float flickvalue;
    [SerializeField] private GameObject soundManager;
    Coroutine BlinkingCoroutine;
    private BGM sound;

    private void Start()
    {
        targetImage = this.GetComponent<Image>();
        sound = soundManager.GetComponent<BGM>();
    }

    void Update()
    {
        // 커서가 올라가 있을 때만 깜빡임 처리
        if (isPointerOver && !isBlinking)
        {
            BlinkingCoroutine = StartCoroutine(BlinkEffect());
        }
    }

    public void OnPointerEnter()
    {
        isPointerOver = true; // 커서가 올라갔음을 표시
        sound.OnButton();
    }

    public void OnPointClick()
    {
        sound.ClickButton();
    }

    public void OnPointerExit()
    {
        isPointerOver = false;
        isBlinking = false;
        StopCoroutine(BlinkingCoroutine);
        targetImage.color = new Color(1, 1, 1, 1);
    }

    private IEnumerator BlinkEffect()
    {
        isBlinking = true;
        int flickvector = -1;
        Color c = targetImage.color;
        c.a = 1;
        while (isPointerOver)
        {
            c.a += flickvector * flickvalue;
            c.a = Mathf.Clamp(c.a, 0f, 1f);
            targetImage.color = c;
            if (c.a <= 0.1f) { flickvector = 1;}
            else if(c.a >= 0.9f) {flickvector = -1;}
            yield return null;
        }
        isBlinking = false;
    }
}
