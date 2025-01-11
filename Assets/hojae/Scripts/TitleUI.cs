using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


enum TitleButton
{
    Start,
    Setting,
    Guide,
    Exit
}

public class TitleUI : MonoBehaviour
{
    #region Field
    [Header("Button")]
    [SerializeField] private Image[] titleButton; //Imag
    [SerializeField] private Image Backbutton;
    [Header("Canvas")]
    [SerializeField] private GameObject guideCanvas; //Canvas
    private GuideUI guideUI;
    [SerializeField] private GameObject settingCanvas;
    [SerializeField] private GameObject optionCanvas; //GameObject
    // 0 : start, 1 : setting, 2: guide, 3: exit 
    [SerializeField] private GameObject backGround;
    public bool isPlayerNewbie { get; set; }
    private TitleButton currentOption;
    private bool startguide;
    #endregion
    
    #region LifeCycle

    void Start()
    {
        guideUI = guideCanvas.GetComponent<GuideUI>();
        isPlayerNewbie = true;
        Titleinit();
    }

    void Update()
    {
            
    }
    #endregion
    
    
    
    #region Method
    public void Titleinit()
    {
        for(int i = 0; i < titleButton.Length; i++)
        {
            titleButton[i].gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        currentOption = TitleButton.Start;
        backGround.gameObject.SetActive(true);
        optionCanvas.gameObject.SetActive(true);
        settingCanvas.gameObject.SetActive(false);
        guideCanvas.gameObject.SetActive(false);
        Backbutton.gameObject.SetActive(false);
    }
    
    public void StartButtonPressed()   
    {
        //inActive Button UI
        optionCanvas.gameObject.SetActive(false);
        //start game
        if (isPlayerNewbie) //if Newbie, then show guide
        {
            startguide = true;
            GuideButtonPressed();
        }
    }

    public void SettingButtonPressed()
    {
        //setting
        optionCanvas.gameObject.SetActive(false);
        settingCanvas.gameObject.SetActive(true);
        Backbutton.gameObject.SetActive(true);
    }
    
    public void GuideButtonPressed()
    {
        //Explain this game
        
        optionCanvas.gameObject.SetActive(false);
        guideCanvas.gameObject.SetActive(true);
        guideUI.guideInit();
        if (!isPlayerNewbie) //if player is not newbie, backbutton is active
        {
            Backbutton.gameObject.SetActive(true);
        }
    }
    public void maturePlayer()
    {
        isPlayerNewbie = false;
        if (startguide == true)
        {
            GetComponentInParent<UIManager>().StartGame();
        }
        startguide = false;
    }

    public void ExitButtonPressed()
    {
#if UNITY_EDITOR    //Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else   //Application
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    
    public void BackButtonPressed()
    {
        Titleinit();
    }
    
    #endregion
}
