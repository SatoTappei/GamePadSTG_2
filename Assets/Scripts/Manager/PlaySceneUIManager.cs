using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

/// <summary>
/// ゲーム本編のUIを管理する
/// </summary>
public class PlaySceneUIManager : MonoBehaviour
{
    [SerializeField] GameStartStag _gsStag;
    [SerializeField] GameOverStag _goStag;
    [SerializeField] GameClearStag _gcStag;
    [SerializeField] Timer _timer;
    [SerializeField] TargetView _targetView;
    /// <summary>ダメージを受けた割合を示すゲージ</summary>
    [SerializeField] Transform _damageGauge;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>HPゲージを変化させる</summary>
    public void SetLifeGauge(int max, int current)
    {
        float xValue = 1 - (current * 1.0f / max * 1.0f);
        xValue = Mathf.Clamp01(xValue);
        _damageGauge.DOScaleX(xValue, 0.5f);
    }

    /// <summary>ターゲットビューの値を変更する</summary>
    public void SetTargetView(int count, Sprite icon)
    {
        if(_targetView != null) _targetView.Set(count, icon);
    }

    /// <summary>ゲーム開始時の演出を行う</summary>
    public IEnumerator PlayGameStartStag()
    {
        yield return _gsStag.Play();
    }

    /// <summary>ゲームオーバー時の演出を行う</summary>
    public void PlayGameOverStag()
    {
        if (_goStag != null) _goStag.Play();
    }

    /// <summary>クリアタイムを渡してゲームクリアの演出を行う</summary>
    public void PlayGameClearStag(int time)
    {
        if (_gcStag != null) _gcStag.Play(time);
    }

    /// <summary>コールバックを渡してタイマーをスタートさせる</summary>
    public void TimerStart(UnityAction action = null)
    {
        _timer.TimeUpEvent += action;
        _timer.TimerStart();
    }

    /// <summary>タイマーを停止させる</summary>
    public void TimerPause() => _timer.TimerPause();

    /// <summary>タイマーの経過時間を取得する</summary>
    public int GetTimerCount() => _timer.GetCount();
}
