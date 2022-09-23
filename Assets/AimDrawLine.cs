using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面下の左右の端からエイムに向かって線を引く
/// </summary>
public class AimDrawLine : MonoBehaviour
{
    /// <summary>線の長さを変更するのにwidthを変更するのでRectTransform型が必要</summary>
    [SerializeField] RectTransform _line;
    /// <summary>線を向けるターゲット</summary>
    [SerializeField] Transform _target;

    void Start()
    {

    }

    void Update()
    {
        Vector3 diff = _target.position - transform.position;
        // RectTransformのWidthとHeightが格納されているのがsizeDeltaプロパティ
        Vector2 size = _line.sizeDelta;
        // Widthをターゲットとの距離と同じにする
        size.x = diff.magnitude;
        _line.sizeDelta = size;
        // 線をターゲットに向ける
        transform.up = diff.normalized;
    }
}
