using System.Collections;
using System.Collections.Generic;



using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public bool isComicshowed { get; set; }
    private bool startguidePressed;
    
    #endregion
    
    #region LifeCycle

    void Start()
    {
        guideUI = guideCanvas.GetComponent<GuideUI>();
        isPlayerNewbie = DataManager.Instance.isNewbie;
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
        settingCanvas.gameObject.SetActive(false);
        guideCanvas.gameObject.SetActive(false);
        Backbutton.gameObject.SetActive(false);
        if (startguidePressed == true)
        {
            return;
        }
        backGround.gameObject.SetActive(true);
        optionCanvas.gameObject.SetActive(true);
    }
    
    public void StartButtonPressed()   
    {
        //inActive Button UI
        optionCanvas.gameObject.SetActive(false);
        //start game
        if (isPlayerNewbie) //if Newbie, then show guide
        {
            startguidePressed = true;
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
        DataManager.Instance.isNewbie = false;
        if (startguidePressed == true)
        {
            GetComponentInParent<UIManager>().StartGame();
        }
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
