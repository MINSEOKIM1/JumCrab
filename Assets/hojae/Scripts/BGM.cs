using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGM : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    
    [Header("Audio")]
    [SerializeField] private AudioSource BGMPlayer;
    [SerializeField] private AudioSource SFXPlayer;
    
    [Header("BGM")]
    [SerializeField] private AudioClip TitleBGMClip;
    [SerializeField] private AudioClip ingameBGMClip1;
    [SerializeField] private AudioClip ingameBGMClip2;
    
    void Start()
    {
        bgmSlider.value = BGMPlayer.volume;
        sfxSlider.value = SFXPlayer.volume;
        BGMPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void startTitleBGM()
    {
        BGMPlayer.clip = TitleBGMClip;
        BGMPlayer.Play();
    }
    
    public void startIngameBGM(int clip)
    {
        switch (clip)
        {
            case 1:
                BGMPlayer.clip = ingameBGMClip1;
                break;
            case 2:
                BGMPlayer.clip = ingameBGMClip2;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        BGMPlayer.Play();
    }
    
    
}
