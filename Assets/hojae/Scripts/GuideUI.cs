using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GuideUI : MonoBehaviour
{
    [SerializeField] private GameObject[] guidePanel;
    private int currentPanel;
    
    // Start is called before the first frame update
    private void Awake()
    {
        for (int i = 0; i < guidePanel.Length; i++)
        {
            guidePanel[i].gameObject.SetActive(false);
        }
    }

    #region Method

    public void guideInit()
    {
        for(int i = 0; i < guidePanel.Length; i++)
        {
            guidePanel[i].gameObject.SetActive(false);
        }
        currentPanel = 0;
        guidePanel[currentPanel].gameObject.SetActive(true);
    }

    public void ChangeGuidePanel()
    {
        guidePanel[currentPanel].gameObject.SetActive(false);
        if(guidePanel.Length == currentPanel + 1) { return; }
        guidePanel[currentPanel + 1].gameObject.SetActive(true);
        currentPanel += 1;
    }
    
    #endregion
}
