using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// DamageReceiver��DamageSender��
/// �v���C���[���_���[�W���󂯂��^�����Ƃ��̏�����o�^����
/// </summary>
public class PlayerDamageSubscriber : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReciever;
    [SerializeField] DamageSender _damageSender;

    [Header("�e��_���[�W���o�ɕK�v")]
    [SerializeField] Animator _anim;
    /// <summary>
    /// �q�b�g�X�g�b�v�����̃e�X�g�p
    /// ����������Ă��q�b�g�X�g�b�v�������Ȃ邾���ő��ɉe���͂Ȃ�
    /// </summary>
    //[SerializeField] Animator _anim;

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
