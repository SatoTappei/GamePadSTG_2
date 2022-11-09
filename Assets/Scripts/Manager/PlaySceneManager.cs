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
    ActorDataManager _actorDataMgr;
    PlayerMove _playerMv;
    ///// <summary>プレイヤーの最大体力</summary>
    //[SerializeField] int _maxLife;
    ///// <summary>プレイヤーの現在の体力</summary>
    //IntReactiveProperty _currentLife = new IntReactiveProperty();
    ///// <summary>現在のスコア</summary>
    //IntReactiveProperty _currentScore = new IntReactiveProperty();

    void Awake()
    {
        _uiMgr = GetComponent<PlaySceneUIManager>();
        _enemyMgr = GetComponent<EnemyManager>();
        _actorDataMgr = GetComponent<ActorDataManager>();
        _playerMv = FindObjectOfType<PlayerMove>();

        //// 現在のスコアに0をセット
        //_currentScore.Value = 0;
        //// 最大体力を現在の体力としてセット
        //_currentLife.Value = _maxLife;

        //_currentLife.Subscribe(life => _uiMgr.SetLifeGauge(_maxLife, life));
        //_currentLife.Where(life => life <= 0).Subscribe(_ => GameOver());
        //_currentScore.Subscribe(score => _uiMgr.SetScore(score));
    }

    async void Start()
    {
        _enemyMgr.Init(CharacterTag.BlueSoldier);

        // ReactiveCollectionはSubscribe時に処理が実行されないので
        // 一度手動でターゲットビューをセットしてから残りはターゲットが減るたびに更新させる。
        _uiMgr.SetTargetView(_enemyMgr.GetTargetAmount(), _actorDataMgr.GetEnemyData(CharacterTag.BlueSoldier).Icon);
        _enemyMgr.TargetsObservable.Subscribe(t => _uiMgr.SetTargetView(_enemyMgr.GetTargetAmount(), t.Value.ActorData.Icon)).AddTo(_enemyMgr);

        // ゲームスタートの演出後にタイマーをスタートさせ、プレイヤーと敵をアクティブにする。
        await _uiMgr.PlayGameStartStag();
        _uiMgr.TimerStart(()=>Debug.Log("ここにタイムアップ時の処理を入れる"));
        _playerMv.WakeUp();
        _enemyMgr.WakeUpEnemyAll();

        //タイムアップでがめおべらになるようにする
    }

    void Update()
    {

    }
}
