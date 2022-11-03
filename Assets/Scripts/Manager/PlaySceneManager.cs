using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// ゲーム全体の進行を管理する
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    PlaySceneUIManager _uiMgr;
    EnemyManager _enemyMgr;
    PlayerMove _playerMv;
    /// <summary>プレイヤーの最大体力</summary>
    [SerializeField] int _maxLife;
    /// <summary>プレイヤーの現在の体力</summary>
    IntReactiveProperty _currentLife = new IntReactiveProperty();
    /// <summary>現在のスコア</summary>
    IntReactiveProperty _currentScore = new IntReactiveProperty();

    void Awake()
    {
        _uiMgr = GetComponent<PlaySceneUIManager>();
        _enemyMgr = GetComponent<EnemyManager>();
        _playerMv = FindObjectOfType<PlayerMove>();

        // 現在のスコアに0をセット
        _currentScore.Value = 0;
        // 最大体力を現在の体力としてセット
        _currentLife.Value = _maxLife;

        _currentLife.Subscribe(life => _uiMgr.SetLifeGauge(_maxLife, life));
        _currentLife.Where(life => life <= 0).Subscribe(_ => GameOver());
        //_currentScore.Subscribe(score => _uiMgr.SetScore(score));
    }

    async void Start()
    {
        await _uiMgr.PlayGameStartStag();
        _playerMv.WakeUp();
        _enemyMgr.WakeUpEnemyAll();
        //タイムアップでガメオベラになるようにする
    }

    void Update()
    {
        //if (Input.anyKeyDown)
        //{
        //    _currentLife.Value--;
        //    _currentScore.Value += 100;
        //}
    }

    // ゲームオーバーの演出を行う
    void GameOver()
    {
        Debug.Log("がめおべら");
        // 使わなくなったタイミング = 死亡なのでDisposeする。
        _currentLife.Dispose();
    }
}
