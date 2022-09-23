using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ゲーム全体の進行を管理する
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    PlaySceneUIManager _psUIm;
    /// <summary>キューブが触れるとプレイヤーへのダメージとなるコライダー</summary>
    [SerializeField] Collider _damageZone;
    /// <summary>プレイヤーの最大体力</summary>
    [SerializeField] int _maxLife;
    /// <summary>プレイヤーの現在の体力</summary>
    IntReactiveProperty _currentLife = new IntReactiveProperty();
    /// <summary>現在のスコア</summary>
    IntReactiveProperty _currentScore = new IntReactiveProperty();

    void Awake()
    {
        // 現在のスコアに0をセット
        _currentScore.Value = 0;
        // 最大体力を現在の体力としてセット
        _currentLife.Value = _maxLife;
        _psUIm = GetComponent<PlaySceneUIManager>();

        _damageZone.OnTriggerEnterAsObservable()
            .Where(collider => collider.CompareTag("Cube"))
            .Subscribe(_ => Debug.Log("ヒットした"));

        _currentLife.Subscribe(life => _psUIm.SetLifeGauge(_maxLife, life));
        _currentLife.Where(life => life <= 0).Subscribe(_ => GameOver());
        _currentScore.Subscribe(score => _psUIm.SetScore(score));
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            _currentLife.Value--;
            _currentScore.Value += 100;
        }
    }

    // ゲームオーバーの演出を行う
    void GameOver()
    {
        Debug.Log("がめおべら");
        // 使わなくなったタイミング = 死亡なのでDisposeする。
        _currentLife.Dispose();
    }
}
