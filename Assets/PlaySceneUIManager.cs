using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ゲーム本編のUIを管理する
/// </summary>
public class PlaySceneUIManager : MonoBehaviour
{
    /// <summary>ダメージを受けた割合を示すゲージ</summary>
    [SerializeField] Transform _damageGauge;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// HPゲージを変化させる
    /// </summary>
    /// <param name="max">プレイヤーの最大体力</param>
    /// <param name="current">プレイヤーの現在の体力</param>
    public void SetLifeGauge(int max, int current)
    {
        float xValue = 1 - (current * 1.0f / max * 1.0f);
        _damageGauge.DOScaleX(xValue, 0.5f);
    }
}
