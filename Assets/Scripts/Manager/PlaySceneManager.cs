using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �Q�[���S�̂̐i�s���Ǘ�����
/// </summary>
public class PlaySceneManager : MonoBehaviour
{
    PlaySceneUIManager _psUIm;
    /// <summary>�v���C���[�̍ő�̗�</summary>
    [SerializeField] int _maxLife;
    /// <summary>�v���C���[�̌��݂̗̑�</summary>
    IntReactiveProperty _currentLife = new IntReactiveProperty();
    /// <summary>���݂̃X�R�A</summary>
    IntReactiveProperty _currentScore = new IntReactiveProperty();

    void Awake()
    {
        // ���݂̃X�R�A��0���Z�b�g
        _currentScore.Value = 0;
        // �ő�̗͂����݂̗̑͂Ƃ��ăZ�b�g
        _currentLife.Value = _maxLife;
        _psUIm = GetComponent<PlaySceneUIManager>();

        _currentLife.Subscribe(life => _psUIm.SetLifeGauge(_maxLife, life));
        _currentLife.Where(life => life <= 0).Subscribe(_ => GameOver());
        _currentScore.Subscribe(score => _psUIm.SetScore(score));
    }

    void Start()
    {
        // TODO:�Q�[���J�n�̉��o��ɓG�������悤�ɂ���
        // �G��AI�̃X�N���v�g�͂��̏����ɑΉ��ł���悤����Ă���B
        // �X�^�[�g���o��Ƀ��b�Z�[�W�𔭍s���đS�Ă̓G���A�N�e�B�u�ɂ���
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
