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
            text.transform.DOPunchScale(Vector3.one * 1.5f, 0.5f, 1, 1);
            yield return new WaitForSeconds(1.0f);
        }
        text.text = "START!";
        text.transform.localScale = Vector3.zero;
        text.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(1.0f);

        // 演出が終わったら非表示になる
        gameObject.SetActive(false);
    }
}
