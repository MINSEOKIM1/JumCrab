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
    [SerializeField] private AudioClip phase2BGM;
    [SerializeField] private AudioClip clearBGM;
    [SerializeField] private AudioClip failBGM;
    
    [Header("SFX")]
    [SerializeField] private AudioClip OnButtonClip;
    [SerializeField] private AudioClip ClickButtonClip;
    [SerializeField] private AudioClip ScoopWarning;
    [SerializeField] private AudioClip Jump;
    [SerializeField] private AudioClip ScoopItemDrop;
    [SerializeField] private AudioClip Diving;
    [SerializeField] private AudioClip ScoopSwing;
    [SerializeField] private AudioClip Warning;

    [SerializeField] private AudioClip[] clips;

    public float fadeinout = 1f;
    
    void Start()
    {
        
        BGMPlayer.volume = DataManager.Instance.BGMVolume;
        SFXPlayer.volume = DataManager.Instance.SFXVolume;
        BGMPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        BGMPlayer.volume = DataManager.Instance.BGMVolume * fadeinout;
    }

    public IEnumerator BgmFadeInOut()
    {
        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime;
            fadeinout = 1 - time;
            yield return null;
        }

        BGMPlayer.clip = phase2BGM;
        BGMPlayer.Play();
        
        time = 0;

        while (time < 1)
        {
            time += Time.deltaTime;
            fadeinout = time;
            yield return null;
        }
    }
    
    public IEnumerator Clear()
    {
        float time = 0;

        while (time < 1.5f)
        {
            time += Time.deltaTime;
            fadeinout = (1.5f - time) / 1.5f;
            yield return null;
        }

        BGMPlayer.clip = clearBGM;
        BGMPlayer.loop = false;
        BGMPlayer.Play();

        fadeinout = 1;
    }
    
    public IEnumerator Die()
    {
        float time = 0;

        while (time < 1.5f)
        {
            time += Time.deltaTime;
            fadeinout = 1 - time;
            yield return null;
        }

        BGMPlayer.clip = failBGM;
        BGMPlayer.loop = false;
        BGMPlayer.Play();

        fadeinout = 1;
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
    
    public void PlaySFX(int n)
    {
        SFXPlayer.PlayOneShot(clips[n]);
    }
}
