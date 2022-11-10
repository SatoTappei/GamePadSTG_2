using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���S�̂̐i�s���Ǘ�����
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    PlaySceneUIManager _uiMgr;
    EnemyManager _enemyMgr;
    ActorDataManager _actorDataMgr;
    [SerializeField] Button _retryButton;
    // TODO: 2�R���|�[�l���g����Ă���̖��ʁA�����ׂ�
    PlayerMove _playerMv;
    PlayerUnit _playerUnit;
    ///// <summary>�v���C���[�̍ő�̗�</summary>
    //[SerializeField] int _maxLife;
    ///// <summary>�v���C���[�̌��݂̗̑�</summary>
    //IntReactiveProperty _currentLife = new IntReactiveProperty();
    ///// <summary>���݂̃X�R�A</summary>
    //IntReactiveProperty _currentScore = new IntReactiveProperty();

    void Awake()
    {
        _uiMgr = GetComponent<PlaySceneUIManager>();
        _enemyMgr = GetComponent<EnemyManager>();
        _actorDataMgr = GetComponent<ActorDataManager>();
        _playerMv = FindObjectOfType<PlayerMove>();
        _playerUnit = FindObjectOfType<PlayerUnit>();

        _retryButton.onClick.AddListener(() => RetryGame());

        //// ���݂̃X�R�A��0���Z�b�g
        //_currentScore.Value = 0;
        //// �ő�̗͂����݂̗̑͂Ƃ��ăZ�b�g
        //_currentLife.Value = _maxLife;

        //_currentLife.Subscribe(life => _uiMgr.SetLifeGauge(_maxLife, life));
        //_currentLife.Where(life => life <= 0).Subscribe(_ => GameOver());
        //_currentScore.Subscribe(score => _uiMgr.SetScore(score));
    }

    async void Start()
    {
        _enemyMgr.Init(CharacterTag.BlueSoldier);

        // ReactiveCollection��Subscribe���ɏ��������s����Ȃ��̂�
        // ��x�蓮�Ń^�[�Q�b�g�r���[���Z�b�g���Ă���c��̓^�[�Q�b�g�����邽�тɍX�V������B
        _uiMgr.SetTargetView(_enemyMgr.GetTargetAmount(), _actorDataMgr.GetEnemyData(CharacterTag.BlueSoldier).Icon);
        _enemyMgr.TargetsObservable.Subscribe(t => _uiMgr.SetTargetView(_enemyMgr.GetTargetAmount(), t.Value.ActorData.Icon)).AddTo(_enemyMgr);

        // �v���C���[����\��(����)�ɂȂ����炪�߂��ׂ�̏������Ă�
        _playerUnit.gameObject.OnDisableAsObservable().Subscribe(_ => GameOver());

        // �v���C���[�̗̑͂����邽�т�UI�ɔ��f������
        _playerUnit.OnDamageObservable.Subscribe(i => _uiMgr.SetLifeGauge(_playerUnit.ActorData.MaxHP, i)).AddTo(_playerUnit);

        // �Q�[���X�^�[�g�̉��o��Ƀ^�C�}�[���X�^�[�g�����A�v���C���[�ƓG���A�N�e�B�u�ɂ���B
        await _uiMgr.PlayGameStartStag();
        _uiMgr.TimerStart(() => GameOver());
        _playerMv.WakeUp();
        _enemyMgr.WakeUpEnemyAll();

        //�^�C���A�b�v�ł��߂��ׂ�ɂȂ�悤�ɂ���
    }

    void Update()
    {

    }

    // �Q�[���I�[�o�[�̏������s��
    void GameOver()
    {
        FindObjectOfType<CameraController>().enabled = false;
       _uiMgr.PlayGameOverStag();
    }

    /// <summary>�Q�[�������g���C����</summary>
    void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
