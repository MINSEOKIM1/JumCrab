using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Comic : MonoBehaviour
{
    // Start is called before the first frame update
    
    ScrollRect scrollRect;
    [SerializeField] RectTransform content;
    [SerializeField] float scrollSpeed;

    [SerializeField] private float time = 2f;
    private bool a, b;
    
    void OnEnable()
    {
        scrollRect = this.GetComponent<ScrollRect>();
    }
    
    private void Update()
    {
        time -= Time.deltaTime;
        Debug.Log(time);
        
        if (time < 0 && !a)
        {Debug.Log("?!?!!?!?!?!");
            StartCoroutine(AutoScroll());
            a = true;
        }

        if(content.anchoredPosition.y >= 1700 && !b){
            Debug.Log("@@@");
            time = 2f;
            b = true;
        }
        
        if (time < 0 && b)
        {
            s();
        }

    }

    private IEnumerator AutoScroll()
    {
        scrollRect.verticalNormalizedPosition = 1f;
        while (true)
        {
            scrollRect.verticalNormalizedPosition -= scrollSpeed;
            yield return null;
        }
    }


    private void s()
    {
        SceneManager.LoadScene("hojae/MinseoDevScene0");
    }
    /*private IEnumerator waitFortime()
    {
        StartCoroutine(AutoScroll());
        yield return
    }*/
}
