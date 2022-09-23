using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面下の端からエイムカーソルに向けてラインを引く
/// </summary>
public class AimDrawLine : MonoBehaviour
{
    /// <summary>線の本体である画像のWidthを書き換えるので参照を持っておく</summary>
    [SerializeField] RectTransform _line;
    /// <summary>ラインを引くターゲット</summary>
    [SerializeField] Transform _target;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 diff = _target.position - transform.position;
        Vector2 size = _line.sizeDelta;
        size.x = diff.magnitude;
        _line.sizeDelta = size;
        transform.up = diff.normalized;
    }
}
