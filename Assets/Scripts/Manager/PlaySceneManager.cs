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
    PlayerMove _playerMv;
    /// <summary>�v���C���[�̍ő�̗�</summary>
    [SerializeField] int _maxLife;
    /// <summary>�v���C���[�̌��݂̗̑�</summary>
    IntReactiveProperty _currentLife = new IntReactiveProperty();
    /// <summary>���݂̃X�R�A</summary>
    IntReactiveProperty _currentScore = new IntReactiveProperty();

    void Awake()
    {
        _uiMgr = GetComponent<PlaySceneUIManager>();
        _enemyMgr = GetComponent<EnemyManager>();
        _playerMv = FindObjectOfType<PlayerMove>();

        // ���݂̃X�R�A��0���Z�b�g
        _currentScore.Value = 0;
        // �ő�̗͂����݂̗̑͂Ƃ��ăZ�b�g
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
        //�^�C���A�b�v�ŃK���I�x���ɂȂ�悤�ɂ���
    }

    void Update()
    {
        //if (Input.anyKeyDown)
        //{
        //    _currentLife.Value--;
        //    _currentScore.Value += 100;
        //}
    }

    // �Q�[���I�[�o�[�̉��o���s��
    void GameOver()
    {
        Debug.Log("���߂��ׂ�");
        // �g��Ȃ��Ȃ����^�C�~���O = ���S�Ȃ̂�Dispose����B
        _currentLife.Dispose();
    }
}
