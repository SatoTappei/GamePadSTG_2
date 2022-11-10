using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体の進行を管理する
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    PlaySceneUIManager _uiMgr;
    EnemyManager _enemyMgr;
    ActorDataManager _actorDataMgr;
    [SerializeField] Button _retryButton;
    // TODO: 2つコンポーネント取ってくるの無駄、直すべき
    PlayerMove _playerMv;
    PlayerUnit _playerUnit;
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
        _playerUnit = FindObjectOfType<PlayerUnit>();

        _retryButton.onClick.AddListener(() => RetryGame());

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

        // プレイヤーが非表示(死んだ)になったらがめおべらの処理を呼ぶ
        _playerUnit.gameObject.OnDisableAsObservable().Subscribe(_ => GameOver());

        // プレイヤーの体力が減るたびにUIに反映させる
        _playerUnit.OnDamageObservable.Subscribe(i => _uiMgr.SetLifeGauge(_playerUnit.ActorData.MaxHP, i)).AddTo(_playerUnit);

        // ゲームスタートの演出後にタイマーをスタートさせ、プレイヤーと敵をアクティブにする。
        await _uiMgr.PlayGameStartStag();
        _uiMgr.TimerStart(() => GameOver());
        _playerMv.WakeUp();
        _enemyMgr.WakeUpEnemyAll();

        //タイムアップでがめおべらになるようにする
    }

    void Update()
    {

    }

    // ゲームオーバーの処理を行う
    void GameOver()
    {
        FindObjectOfType<CameraController>().enabled = false;
       _uiMgr.PlayGameOverStag();
    }

    /// <summary>ゲームをリトライする</summary>
    void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
