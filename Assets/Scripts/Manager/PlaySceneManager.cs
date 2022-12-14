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
    CameraWorkManager _cameraWorkManager;
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
        _cameraWorkManager = GetComponent<CameraWorkManager>();
        _playerMv = FindObjectOfType<PlayerMove>();
        _playerUnit = FindObjectOfType<PlayerUnit>();

        //// 現在のスコアに0をセット
        //_currentScore.Value = 0;
        //// 最大体力を現在の体力としてセット
        //_currentLife.Value = _maxLife;

        //_currentLife.Subscribe(life => _uiMgr.SetLifeGauge(_maxLife, life));
        //_currentLife.Where(life => life <= 0).Subscribe(_ => GameOver());
        //_currentScore.Subscribe(score => _uiMgr.SetScore(score));
    }

    IEnumerator Start()
    {
        // タイトル画面でコントローラーのボタンを押したら始まる
        yield return new WaitUntil(() => Input.GetButton("Fire"));
        // タイトルUIを消す
        yield return _uiMgr.RemoveTitleUI();
        // カメラのターゲットをプレイヤーにしてゲームスタートの演出
        _cameraWorkManager.MoveToInGame();
        _uiMgr.ActiveSatusUI();
        // ゲームスタート

        // 他のStart()メソッドが終わるのを待つために1フレーム遅らせる
        yield return null;
        _enemyMgr.Init(CharacterTag.BossTank);

        // ReactiveCollectionはSubscribe時に処理が実行されないので
        // 一度手動でターゲットビューをセットしてから残りはターゲットが減るたびに更新させる。
        _uiMgr.SetTargetView(_enemyMgr.GetTargetAmount(), _actorDataMgr.GetActorDataSO(CharacterTag.BossTank).Icon);
        _enemyMgr.TargetsObservable.Subscribe(t => _uiMgr.SetTargetView(_enemyMgr.GetTargetAmount(), t.Value.ActorData.Icon)).AddTo(_enemyMgr);
        
        // 敵の数が0になったらゲームクリアの処理を行う
        _enemyMgr.TargetsObservable.Where(_ => _enemyMgr.GetTargetAmount() == 0).Subscribe(_ => GameClear()).AddTo(_enemyMgr);

        // プレイヤーが非表示(死んだ)になったらがめおべらの処理を呼ぶ
        _playerUnit.gameObject.OnDisableAsObservable().Subscribe(_ => GameOver()).AddTo(_playerUnit);

        // プレイヤーの体力が減るたびにUIに反映させる
        _playerUnit.OnDamageObservable.Subscribe(i => _uiMgr.SetLifeGauge(_playerUnit.ActorData.MaxHP, i)).AddTo(_playerUnit);

        // ゲームスタートの演出後にタイマーをスタートさせ、プレイヤーと敵をアクティブにする。
        yield return _uiMgr.PlayGameStartStag();
        _uiMgr.TimerStart(() => GameOver());
        _playerUnit.WakeUp();
        _enemyMgr.WakeUpEnemyAll();
    }

    void Update()
    {

    }

    /// <summary>ゲームクリアの処理を行う</summary>
    void GameClear()
    {
        if (_playerMv == null) return;

        CameraController cc = FindObjectOfType<CameraController>();
        if (cc != null)
        {
            cc.IsPause = true;
        }
        _playerMv.enabled = false;
        _uiMgr.PlayGameClearStag(_uiMgr.GetTimerCount());
        _uiMgr.TimerPause();
        _enemyMgr.ExitEnemyAll();
    }

    /// <summary>ゲームオーバーの処理を行う</summary>
    void GameOver()
    {
        CameraController cc = FindObjectOfType<CameraController>();
        if (cc != null)
        {
            cc.IsPause = true;
        }
        _uiMgr.PlayGameOverStag();
        _uiMgr.TimerPause();
        _enemyMgr.ExitEnemyAll();
    }

    /// <summary>ゲームをリトライする</summary>
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
