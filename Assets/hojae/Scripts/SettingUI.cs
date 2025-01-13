using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private AudioSource BGMPlayer;
    [SerializeField] private AudioSource SFXPlayer;
    
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;
    // Start is called before the first frame update
    void Start()
    {
        BGMSlider.value = DataManager.Instance.BGMVolume;
        SFXSlider.value = DataManager.Instance.SFXVolume;
    }
    // Update is called once per frame
    void Update()
    {
        //merge
        DataManager.Instance.BGMVolume = BGMSlider.value;
        DataManager.Instance.SFXVolume = SFXSlider.value;
        
        BGMPlayer.volume = DataManager.Instance.BGMVolume;
        SFXPlayer.volume = DataManager.Instance.SFXVolume;
        /*
        BGMPlayer.volume = BGMSlider.value;
        SFXPlayer.volume = SFXSlider.value;
        */
    }
}
