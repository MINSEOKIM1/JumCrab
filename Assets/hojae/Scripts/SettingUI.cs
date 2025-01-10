using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private AudioSource BGMPlayer;

    [SerializeField] private Slider Slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        BGMPlayer.volume = Slider.value;
    }
}
