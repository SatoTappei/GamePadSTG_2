using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �v���C���[�̃X�e�[�^�X���Ǘ�����
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReciever;
    [SerializeField] DamageSender _damageSender;

    [Header("�e��_���[�W���o�ɕK�v")]
    [SerializeField] Animator _anim;
    /// <summary>�v���C���[�̗̑�</summary>
    [SerializeField] int _maxLife;
    int _currentLife;

    void Start()
    {
        
    }

    void OnEnable()
    {
        _damageReciever.OnDamageReceived += OnDamageReceived;
        _damageSender.OnDamageSended += OnDamageSended;
    }

    void OnDisable()
    {
        _damageReciever.OnDamageReceived -= OnDamageReceived;
        _damageSender.OnDamageSended -= OnDamageSended;
    }

    void Update()
    {
        
    }

    /// <summary>�_���[�W���󂯂��ۂ̉��o</summary>
    void OnDamageReceived()
    {
        // ������
    }

    /// <summary>�_���[�W��^�����ۂ̉��o</summary>
    void OnDamageSended()
    {
        // �A�j���[�V�������ꎞ��~����
        _anim.speed = 0;
        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(ConstValue.HitStopTime);
        sequence.AppendCallback(() => _anim.speed = 1.0f);

        // �J������h�炷
        float duration = ConstValue.HitStopTime;
        Vector3 strength = new Vector3(1f, 1f, 0);
        int vibratio = 20;
        CameraController.Shake(duration, strength, vibratio);
    }
}
