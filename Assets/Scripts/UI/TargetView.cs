using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// ターゲットを表示・カウントするUI
/// </summary>
public class TargetView : MonoBehaviour
{
    [SerializeField] Text _counter;
    [SerializeField] Image _icon;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Set(int count, Sprite icon)
    {
        _counter.text = count.ToString();
        _icon.sprite = icon;

        _counter.transform.DOPunchScale(Vector3.one * 1.2f, 0.5f, 1, 1);
    }
}
