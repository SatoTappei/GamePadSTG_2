using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// �Q�[���S�̂̐i�s���Ǘ�����
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    PlaySceneUIManager _uiMgr;
    EnemyManager _enemyMgr;
    ActorDataManager _actorDataMgr;
    PlayerMove _playerMv;
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

        // �Q�[���X�^�[�g�̉��o��Ƀ^�C�}�[���X�^�[�g�����A�v���C���[�ƓG���A�N�e�B�u�ɂ���B
        await _uiMgr.PlayGameStartStag();
        _uiMgr.TimerStart(()=>Debug.Log("�����Ƀ^�C���A�b�v���̏���������"));
        _playerMv.WakeUp();
        _enemyMgr.WakeUpEnemyAll();

        //�^�C���A�b�v�ł��߂��ׂ�ɂȂ�悤�ɂ���
    }

    void Update()
    {

    }
}
