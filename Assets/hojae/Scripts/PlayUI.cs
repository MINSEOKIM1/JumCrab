using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField] private GameObject hpTop;
    [SerializeField] private GameObject hpBottom;

    public PlayerBehavior playerb;
    
    Image hpBottomImage;
    [SerializeField] [Range(0, 1)] private float hp;
    public float sensitivity;
    private Vector3 hpTopPos; 
    // Start is called before the first frame update
    void Start()
    {
        hpTopPos = hpTop.transform.position;
        hpBottomImage = hpBottom.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        hp = playerb._hp / playerb.maxHp;
        hpBottomImage.fillAmount = hp;
        hpTop.transform.position = hpTopPos + new Vector3(0,  sensitivity * hp, 0);
    }
}
