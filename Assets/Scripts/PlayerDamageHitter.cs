using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �v���C���[���_���[�W���󂯂��^�����Ƃ��̏�����o�^����
/// </summary>
public class PlayerDamageHitter : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReciever;
    [SerializeField] DamageSender _damageSender;
    /// <summary>
    /// �q�b�g�X�g�b�v�����̃e�X�g�p
    /// ����������Ă��q�b�g�X�g�b�v�������Ȃ邾���ő��ɉe���͂Ȃ�
    /// </summary>
    [SerializeField] Animator _anim;
 
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

    void OnDamageReceived()
    {
        // ������
    }

    void OnDamageSended()
    {
        // �A�j���[�V�������ꎞ��~���ăq�b�g�X�g�b�v�̎���
        _anim.speed = 0;
        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(EffectConst.HitStopTime);
        sequence.AppendCallback(() => _anim.speed = 1.0f);
    }
}
