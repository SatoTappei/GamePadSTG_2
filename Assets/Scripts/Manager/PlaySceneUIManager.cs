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
    [SerializeField] Timer _timer;
    [SerializeField] TargetView _targetView;
    /// <summary>ダメージを受けた割合を示すゲージ</summary>
    [SerializeField] Transform _damageGauge;
    /// <summary>スコアを表示するテキスト</summary>
    [SerializeField] Text _scoreText;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>HPゲージを変化させる</summary>
    public void SetLifeGauge(int max, int current)
    {
        Debug.Log($"最大値{max} 現在値{current}");
        float xValue = 1 - (current * 1.0f / max * 1.0f);
        Debug.Log("xスケール " + xValue);
        xValue = Mathf.Clamp01(xValue);
        _damageGauge.DOScaleX(xValue, 0.5f);
    }

    /// <summary>スコアをセットする</summary>
    public void SetScore(int score)
    {
        int prev = int.Parse(_scoreText.text.Replace(",", ""));
        _scoreText.DOCounter(prev, score, 0.5f);
    }

    /// <summary>ターゲットビューの値を変更する</summary>
    public void SetTargetView(int count, Sprite icon) => _targetView.Set(count, icon);

    /// <summary>ゲーム開始時の演出を行う</summary>
    public async UniTask PlayGameStartStag() => await _gsStag.Play();

    /// <summary>コールバックを渡してタイマーをスタートさせる</summary>
    public void TimerStart(UnityAction action = null)
    {
        _timer.TimeUpEvent += action;
        _timer.TimerStart();
    }
}
