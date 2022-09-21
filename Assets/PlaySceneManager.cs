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

    void Awake()
    {
        // �ő�̗͂����݂̗̑͂Ƃ��ăZ�b�g
        _currentLife.Value = _maxLife;
        _psUIm = GetComponent<PlaySceneUIManager>();

        _damageZone.OnTriggerEnterAsObservable()
            .Where(c => c.CompareTag("Cube"))
            .Subscribe(_ => Debug.Log("�q�b�g����"));

        _currentLife.Subscribe(i => _psUIm.SetLifeGauge(_maxLife, i));
        _currentLife.Where(i => i == 0).Subscribe(_ => Debug.Log("���S"));
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.anyKeyDown) _currentLife.Value--;
    }
}
