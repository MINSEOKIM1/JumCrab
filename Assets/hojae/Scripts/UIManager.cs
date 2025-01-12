using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum gameState
{
    Title,
    Play,
    Stop
}

public class UIManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    [Header("Canvas")]
    [SerializeField] private Canvas TitleCanvas;
    private TitleUI titleUI;
    [SerializeField] private Canvas PlayCanvas;
    [SerializeField] private Canvas StopCanvas;
    private StopUI stopUI;
    static public gameState GameState;    //tmp : can be change
    
    [Header("Pause")]
    [SerializeField] private float readyTime;
    [SerializeField] private GameObject RTG;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private GameObject soundManager;



    [SerializeField] private GameObject comicGameObject;
    BGM bgm;
    private Coroutine playGame;
    private int eh = 1;
    private bool isComic = true;
    void Start()
    {
        GameState = gameState.Title;
        comicGameObject.gameObject.SetActive(false);
        titleUI = TitleCanvas.GetComponent<TitleUI>();
        stopUI = StopCanvas.GetComponent<StopUI>();
        //Time.timeScale = 0;
        bgm = soundManager.GetComponent<BGM>();
        
        bgm.startTitleBGM();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameState)
        {
            case gameState.Title:
                TitleCanvas.gameObject.SetActive(true);
                PlayCanvas.gameObject.SetActive(false);
                StopCanvas.gameObject.SetActive(false);
                if (playGame != null)
                {
                    StopCoroutine(playGame);
                }
                break;
            case gameState.Play:
                TitleCanvas.gameObject.SetActive(false);
                PlayCanvas.gameObject.SetActive(true);
                //init be needed?
                StopCanvas.gameObject.SetActive(false);
                break;
            case gameState.Stop:
                TitleCanvas.gameObject.SetActive(false);
                PlayCanvas.gameObject.SetActive(false);
                StopCanvas.gameObject.SetActive(true);
                //stopUI init
                break;
            default:
                throw new ArgumentOutOfRangeException();
        } 
    }
    
    
    public void StartGame()
    {
        if (!titleUI.isPlayerNewbie && isComic == true)
        {
            comicGameObject.gameObject.SetActive(true);
            isComic = false;
            return;
        }
        
        if (!titleUI.isPlayerNewbie) //if player is not Newbie
        {
            //merge
            SceneManager.LoadScene("hojae/MinseoDevScene0");
            /*
            Time.timeScale = 1;
            GameState = gameState.Play; //game start
            */
        }
    }
    public void PauseGame()
    {
        GameState = gameState.Stop;
        RTG.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void ResumeGame()
    {
        //start RTG
        RTG.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
        Time.timeScale = 1;
        //then playGame
        playGame = StartCoroutine(DelayedAction(3.0f));
    }
    
    private IEnumerator DelayedAction(float t) //written by chat-gpt
    {
        yield return new WaitForSeconds(t);
        ExecuteAction(); //
    }
    private void ExecuteAction()
    {
        GameState = gameState.Play;
    }
    
    public void HomeButtonPressed()
    {
        GameState = gameState.Title;
        titleUI.Titleinit();
        bgm.startTitleBGM();
    }

}
