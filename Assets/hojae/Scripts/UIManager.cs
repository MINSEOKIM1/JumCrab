using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum gameState
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
    private gameState GameState;    //tmp : can be change
    void Start()
    {
        GameState = gameState.Title;
        titleUI = TitleCanvas.GetComponent<TitleUI>();
        stopUI = StopCanvas.GetComponent<StopUI>();
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
        if (!titleUI.isPlayerNewbie) //if player is not Newbie
        {
            GameState = gameState.Play; //game start
        }
    }
    public void PauseGame()
    {
        GameState = gameState.Stop;
        Time.timeScale = 0;
    }
    
    public void ResumeGame()
    {
        GameState = gameState.Play;
        Time.timeScale = 1;
    }


}
