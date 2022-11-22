using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 現在未使用
/// タイトルのカメラが追従するオブジェクトの往復移動をさせるコンポーネント
/// </summary>
public class TitleCameraTarget : MonoBehaviour
{
    // 往復のポイントとなる2点
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _endPos;
    // 往復時間
    [SerializeField] float _duration;

    Sequence seq;

    void Start()
    {
        seq = DOTween.Sequence();
        seq.Append(transform.DOMove(_endPos.position, _duration));
        seq.Append(transform.DOMove(_startPos.position, _duration));
        seq.SetLoops(-1);
    }

    void Update()
    {
        
    }

    void OnDisable()
    {
        seq.Kill();
    }
}
