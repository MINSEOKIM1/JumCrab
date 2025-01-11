using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    
    [Header("SFX")]
    [SerializeField] private AudioClip OnButtonClip;
    [SerializeField] private AudioClip ClickButtonClip;
    
    void Start()
    {
        
        BGMPlayer.volume = DataManager.Instance.BGMVolume;
        SFXPlayer.volume = DataManager.Instance.SFXVolume;
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
    
    public void OnButton()
    {
        SFXPlayer.clip = OnButtonClip;
        SFXPlayer.PlayOneShot(OnButtonClip);
    }
    
    public void ClickButton()
    {
        SFXPlayer.clip = ClickButtonClip;
        SFXPlayer.PlayOneShot(ClickButtonClip);
    }
}
