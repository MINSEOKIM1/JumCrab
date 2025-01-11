using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GuideUI : MonoBehaviour
{
    [SerializeField] private GameObject[] guidePanel;

    [SerializeField] private GameObject flexible;
    [SerializeField] private Sprite comicImage;
    [SerializeField] private Sprite creditImage;
    private int currentPanel;
    private bool isComicWatched = false;
    
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
        if (currentPanel == 1)
        {
            if (isComicWatched == false)
            {
                flexible.GetComponent<Image>().sprite = comicImage;
                isComicWatched = true;
            }
            else
            {
                flexible.GetComponent<Image>().sprite = creditImage;
            }
        }
        guidePanel[currentPanel].gameObject.SetActive(false);
        guidePanel[currentPanel + 1].gameObject.SetActive(true);
        currentPanel += 1;
    }
    
    #endregion
}
