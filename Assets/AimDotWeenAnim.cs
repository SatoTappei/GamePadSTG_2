using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// エイムをDotWeenでアニメーションさせる
/// </summary>
public class AimDotWeenAnim : MonoBehaviour
{
    [SerializeField] Transform _cursor_1;
    [SerializeField] Transform _cursor_2;

    void Start()
    {
        _cursor_1.DORotate(new Vector3(0, 0, 360), 3.0f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        _cursor_2.DORotate(new Vector3(0, 0, -360), 6.0f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    void Update()
    {
        
    }
}
