using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// ゲーム開始時の演出を行う
/// </summary>
public class GameStartStag : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>演出を行う</summary>
    public IEnumerator Play()
    {
        Text text = GetComponentInChildren<Text>();
        for (int i = 3; i >= 1; i--)
        {
            text.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        text.text = "START!";
        yield return new WaitForSeconds(1.0f);

        // 演出が終わったら非表示になる
        gameObject.SetActive(false);
    }
}
