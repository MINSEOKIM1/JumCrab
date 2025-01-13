using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    public BGM bgm;
    
    public GameObject climbRestoreItem;
    public GameObject goldPowder;

    public HeighEstimate heighEstimate;

    public Transform waterSurface;
    public BoilingWater boilingWater;

    public float targetHeightForClimbRestore;
    public float currentTargetForClimbRestore;
    
    public float targetHeightForEscalating;
    public float currentTargetForEscalating;

    public Transform player;

    public RectTransform warningMessage;
    

    public Transform cameraPosition;
    public Vector3 cameraOffset;

    private bool esc;

    private void Start()
    {
        currentTargetForClimbRestore = targetHeightForClimbRestore;
        currentTargetForEscalating = targetHeightForEscalating;
    }

    private void Update()
    {
        // 벽타기 포션 주기
        if (heighEstimate.currentHeight > currentTargetForClimbRestore)
        {
            currentTargetForClimbRestore += targetHeightForClimbRestore;

            if (Random.Range(0f, 1f) < 0.6f)
            {
                for (int i = 0; i < 3; i++)
                {
                    var ga = Instantiate(
                        climbRestoreItem, 
                        cameraPosition.position + cameraOffset + Vector3.right * Random.Range(-2f, 2f) + Vector3.up * Random.Range(0f, 2f), 
                        quaternion.identity);
                    
                    ga.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(-1f, 1f) + Vector2.up * 1, ForceMode2D.Impulse);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    var ga = Instantiate(
                        climbRestoreItem, 
                        cameraPosition.position + cameraOffset + Vector3.right * Random.Range(-2, 2), 
                        quaternion.identity);
                    
                    ga.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(-1f, 1f) + Vector2.up * 1, ForceMode2D.Impulse);
                }
            }
        }
        
        // 물차기 이벤트
        if (player.transform.position.y - waterSurface.transform.position.y > 20 && esc == false)
        {
            esc = true;
            currentTargetForEscalating += targetHeightForEscalating;

            StartCoroutine(EscalateWater());
        }
    }

    public void SpawnGoldPowder(int n)
    {
        for (int i = 0; i < n; i++)
        {
            GameObject ga = Instantiate(goldPowder,
                cameraPosition.position + cameraOffset +
                Vector3.up * Random.Range(0f, 2f)
                , Quaternion.identity);
            ga.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(-1f, 1f) + Vector2.up * 1, ForceMode2D.Impulse);
        }
    }

    private IEnumerator EscalateWater()
    {
        bgm.PlaySFX(5);
        float time0 = 0;
        while (time0 < 2)
        {
            
            warningMessage.gameObject.SetActive(true);
            
            time0 += Time.deltaTime;
            yield return null;
        }
        
        warningMessage.gameObject.SetActive(false);

        float original = boilingWater.upwardSpeed;

        int a = 0;
        while (waterSurface.position.y < cameraPosition.position.y - 5)
        {
            cameraPosition.position += Vector3.right * (a < 2 ? 6 : -6) * Time.deltaTime;
            boilingWater.upwardSpeed = 16;
            yield return null;
            a++;
            if (a == 4) a = 0;
        }

        esc = false;
        
        boilingWater.upwardSpeed = original;
    }
}
