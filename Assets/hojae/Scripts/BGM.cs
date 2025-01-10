using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGM : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider bgmSlider;
    
    [Header("Audio")]
    [SerializeField] private AudioSource BGMPlayer;
    
    [Header("BGM")]
    [SerializeField] private AudioClip bgmClip;

    
    private void Awake()
    {
        BGMPlayer.clip = bgmClip;
    }
    void Start()
    {
        bgmSlider.value = BGMPlayer.volume;
        BGMPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
