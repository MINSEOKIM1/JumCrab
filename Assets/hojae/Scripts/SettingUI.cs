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
        
    }
    // Update is called once per frame
    void Update()
    {
        BGMPlayer.volume = BGMSlider.value;
        SFXPlayer.volume = SFXSlider.value;
        
    }
}
