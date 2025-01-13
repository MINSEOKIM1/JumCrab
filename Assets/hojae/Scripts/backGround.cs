using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backGround : MonoBehaviour
{
    #region Field
    [Header("Image")]
    [SerializeField] private GameObject basic;
    //[SerializeField] private GameObject filter;
    public float flickvalue;
    
    [Header("small")]
    [SerializeField] private GameObject smallItem;
    public float smallshakeRange;
    public float smallshakeSpeed;
    
    [Header("big")]
    [SerializeField] private GameObject bigItem;
    public float bigshakeRange;
    public float bigshakeSpeed;
    #endregion

    #region LifeCycle
    void Start()
    {
        //StartCoroutine(BlinkImage(filter));
        StartCoroutine(ShakeImage(smallItem, smallshakeRange, smallshakeSpeed));
        StartCoroutine(ShakeImage(bigItem, bigshakeRange, bigshakeSpeed));
    }

    void Update()
    {
        
    }
    #endregion

    #region Method


    private IEnumerator BlinkImage(GameObject go)   //written by chat-gpt
    {
        Image image = go.GetComponent<Image>();
        int flickvector = -1;
        while (true)
        {
            Color tempColor = image.color;
            tempColor.a += flickvector * flickvalue;
            tempColor.a = Mathf.Clamp(tempColor.a, 0f, 1f);
            image.color = tempColor;
            if (tempColor.a <= 0.3f)
            {
                flickvector = 1;
            }
            else if (tempColor.a >= 0.9f) // 0.9 이상일 때 반전
            {
                flickvector = -1;
            }
            yield return null;
        }
    }

    private IEnumerator ShakeImage(GameObject go, float shakeRange, float shakeSpeed)
    {
        Vector3 originPos = go.transform.position;
        int shakevector = -1;
        Vector3 newPos = originPos;
        
        while (true)
        {
            newPos.y += shakevector * shakeSpeed;
            go.transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
            if (newPos.y <= originPos.y-shakeRange) { shakevector = 1; }
            else if (newPos.y >= originPos.y+shakeRange) { shakevector = -1; }
        
            yield return null; // Adjust the delay as needed
        }
    }

    #endregion
}
