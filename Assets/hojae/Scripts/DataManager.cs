using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static DataManager Instance { get; private set; }

    public float BGMVolume { get; set; }
    public float SFXVolume { get; set; }
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        BGMVolume = 1f;
        SFXVolume = 1f;
        DontDestroyOnLoad(this);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
