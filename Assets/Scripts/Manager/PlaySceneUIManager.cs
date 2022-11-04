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
    /// <summary>ダメージを受けた割合を示すゲージ</summary>
    [SerializeField] Transform _damageGauge;
    /// <summary>スコアを表示するテキスト</summary>
    [SerializeField] Text _scoreText;
    /// <summary>ターゲットの情報を表示するUI(カウンター)</summary>
    [SerializeField] Text _targetLabelCount;
    /// <summary>ターゲットの情報を表示するUI(アイコン)</summary>
    [SerializeField] Image _targetLabelIcon;
    
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
        xValue = Mathf.Clamp01(xValue);
        _damageGauge.DOScaleX(xValue, 0.5f);
    }

    /// <summary>
    /// スコアをセットする
    /// </summary>
    /// <param name="score">セットするスコア</param>
    public void SetScore(int score)
    {
        int prev = int.Parse(_scoreText.text.Replace(",", ""));
        _scoreText.DOCounter(prev, score, 0.5f);
    }

    /// <summary>ターゲットの数とそのアイコンをセットする</summary>
    public void SetTargetLabel(int count, Sprite icon)
    {
        _targetLabelCount.text = count.ToString();
        _targetLabelIcon.sprite = icon;
    }

    /// <summary>ゲーム開始時の演出を行う</summary>
    public async UniTask PlayGameStartStag()
    {
        await _gsStag.Play();
    }

    /// <summary>コールバックを渡してタイマーをスタートさせる</summary>
    public void TimerStart(UnityAction action = null)
    {
        _timer.TimeUpEvent += action;
        _timer.TimerStart();
    }
}
