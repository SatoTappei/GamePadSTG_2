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
    [SerializeField] Text _text;

    void Start()
    {
        _text.text = "";
    }

    void Update()
    {
        
    }

    /// <summary>演出を行う</summary>
    public IEnumerator Play()
    {
        for (int i = 3; i >= 1; i--)
        {
            _text.text = i.ToString();
            _text.transform.DOPunchScale(Vector3.one * 1.5f, 0.5f, 1, 1);
            yield return new WaitForSeconds(1.0f);
        }
        _text.text = "START!";
        _text.transform.localScale = Vector3.zero;
        _text.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(1.0f);

        // 演出が終わったら非表示になる
        gameObject.SetActive(false);
    }
}
