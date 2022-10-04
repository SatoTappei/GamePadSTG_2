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
    /// <summary>�L���[�u���G���ƃv���C���[�ւ̃_���[�W�ƂȂ�R���C�_�[</summary>
    [SerializeField] Collider _damageZone;
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

        _damageZone.OnTriggerEnterAsObservable()
            .Where(collider => collider.CompareTag("Cube"))
            .Subscribe(_ => Debug.Log("�q�b�g����"));

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

    // �Q�[���I�[�o�[�̉��o���s��
    void GameOver()
    {
        Debug.Log("���߂��ׂ�");
        // �g��Ȃ��Ȃ����^�C�~���O = ���S�Ȃ̂�Dispose����B
        _currentLife.Dispose();
    }
}
