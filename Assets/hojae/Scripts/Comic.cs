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

    public Image fade;

    [SerializeField] private float time = 2f;
    private bool a, b;
    
    void OnEnable()
    {
        scrollRect = this.GetComponent<ScrollRect>();
    }
    
    private void Update()
    {
        time -= Time.deltaTime;
        
        if (time < 0 && !a)
        {
            StartCoroutine(AutoScroll());
            a = true;
        }

        if(scrollRect.verticalNormalizedPosition < 0.01f && !b){
            time = 2f;
            b = true;
        }

        if (b)
        {
            fade.gameObject.SetActive(true);
            fade.color = new Color(0, 0, 0, (2 - time) / 2);
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
            if (scrollRect.verticalNormalizedPosition > 0f) scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;
            yield return null;
        }
    }


    private void s()
    {
        SceneManager.LoadScene("hojae/MinseoDevScene0", LoadSceneMode.Single);
    }
    /*private IEnumerator waitFortime()
    {
        StartCoroutine(AutoScroll());
        yield return
    }*/
}
