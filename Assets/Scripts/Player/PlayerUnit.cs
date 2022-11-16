using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using DG.Tweening;

/// <summary>
/// �v���C���[�̐�����s��
/// </summary>
public class PlayerUnit : ActorUnit
{
    [Header("�e��_���[�W���o�ɕK�v")]
    [SerializeField] Animator _anim;

    void Update()
    {
        
    }

    /// <summary>�_���[�W���󂯂��ۂ̉��o</summary>
    protected override void OnDamageReceived()
    {
        // ������
    }

    protected override void OnDeath()
    {
        gameObject.SetActive(false);
    }

    /// <summary>�_���[�W��^�����ۂ̉��o</summary>
    protected override void OnDamageSended()
    {
        // �A�j���[�V�������ꎞ��~����
        _anim.speed = 0;
        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(InGameUtility.HitStopTime);
        sequence.AppendCallback(() => _anim.speed = 1.0f);

        // �J������h�炷
        float duration = InGameUtility.HitStopTime;
        Vector3 strength = new Vector3(1f, 1f, 0);
        int vibratio = 20;
        CameraController.Shake(duration, strength, vibratio);
    }
}
