using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System;

/// <summary>
/// DamageSender����_���[�W���󂯂����Ƃ���M����
/// HP�̊Ǘ��͂��̃N���X���S��
/// </summary>
public class DamageReceiver : MonoBehaviour, IDamageable
{
    /// <summary>�m�b�N�o�b�N��</summary>
    [SerializeField] float _knockBackPower = 3.0f;
    /// <summary>���G����</summary>
    [SerializeField] float _invincibleTime = 2.0f;
    /// <summary>�_���[�W���󂯂��ۂ̃G�t�F�N�g�̃v���n�u</summary>
    [SerializeField] GameObject _damageEffectPrefab;
    /// <summary>���S���ɐ������鉉�o�̃v���t�@�u</summary>
    [SerializeField] GameObject _defeatedEffectPrefab;
    /// <summary>�\����\����؂�ւ��Ďg���܂킷�_���[�W�G�t�F�N�g</summary>
    GameObject _damageEffect;
    /// <summary>���݂̗̑�</summary>
    ReactiveProperty<int> _currentHP = new ReactiveProperty<int>(0);
    /// <summary>���G���Ԓ����ǂ����̃t���O</summary>
    bool _isInvincible = true;
    /// <summary>�_���[�W���󂯂��Ƃ��ɍs���ǉ��̏���</summary>
    public UnityAction OnDamageReceived;
    /// <summary>���S���ɍs���ǉ��̏���</summary>
    public UnityAction OnDeath;

    public ReactiveProperty<int> CurrentHP { get => _currentHP; }

    void Awake()
    {
        // �g���܂킹��悤�Ƀ_���[�W�G�t�F�N�g���C���X�^���X�����Ĕ�A�N�e�B�u�ɂ��Ă���
        if (_damageEffectPrefab != null)
        {
            _damageEffect = Instantiate(_damageEffectPrefab);
            _damageEffect.SetActive(false);
        }
    }

    /// <summary>�����������A���ꂪ�Ă΂��܂œG�����G��Ԃ̂܂�</summary>
    public void Init(int maxHP)
    {
        _currentHP.Value = maxHP;
        _isInvincible = false;
    }

    /// <summary>�U�����󂯂��Ƃ��̏���</summary>
    public void OnDamage(int damage, Vector3 hitPos)
    {
        // ���G���Ԓ��͍U�����󂯂Ȃ�
        if (_isInvincible) return;
        // ���G���Ԃ�ݒ肷��
        DOTween.Sequence()
               .AppendInterval(_invincibleTime)
               .OnStart(() => _isInvincible = true)
               .OnComplete(() => _isInvincible = false);

        // �Փˈʒu�Ƀp�[�e�B�N���𔭐�������
        if (_damageEffect != null) 
            EnabledDamageEffect(hitPos);

        // �q�b�g�X�g�b�v�̌�A�m�b�N�o�b�N������
        DOVirtual.DelayedCall(InGameUtility.HitStopTime, () => CalcKnockBack(hitPos, _knockBackPower));

        // �_���[�W�𔽉f������
        _currentHP.Value -= damage;
        if(_currentHP.Value < 0)
        {
            Instantiate(_defeatedEffectPrefab, transform.position, Quaternion.identity);
            OnDeath?.Invoke();
        }
        else
        {
            OnDamageReceived?.Invoke();
        }
    }

    /// <summary>�_���[�W�G�t�F�N�g�𔭐�������</summary>
    void EnabledDamageEffect(Vector3 pos)
    {
        _damageEffect.transform.position = pos;
        _damageEffect.SetActive(true);
    }

    /// <summary>�m�b�N�o�b�N������</summary>
    void CalcKnockBack(Vector3 hit, float power)
    {
        // �m�b�N�o�b�N��������������߂�
        Vector3 pos = transform.position;
        pos.y = 0;
        hit.y = 0;
        Vector3 dir = Vector3.Normalize(pos - hit);

        // �m�b�N�o�b�N
        Knockback(dir * power, 0.1f);
    }

    /// <summary>
    /// �m�b�N�o�b�N���o
    /// </summary>
    /// <param name="knockbackVector">�m�b�N�o�b�N��������Ƌ�����\���x�N�g��</param>
    /// <param name="speed">�m�b�N�o�b�N�X�s�[�h�B1m�����艽�b�ňړ����邩�H�̒P��</param>
    private void Knockback(Vector3 knockbackVector, float speed)
    {
        // �R���C�_�[�̔��a���l�����Ė��܂�Ȃ��悤�ɂ���
        float colR = 0.5f;

        // �x�N�g���̒������v�Z���āA�m�b�N�o�b�N�������擾
        // Vector�^��"magnitude"�́A�x�N�g���̒������v�Z���ĕԂ��ϐ�
        float knockbackDistance = knockbackVector.magnitude;

        // �ړ��O�Ƀm�b�N�o�b�N�����ɗ���΂��āA"Field"���C���[�̃R���C�_�[�Ƀq�b�g���邩�ǂ����`�F�b�N
        // "maxDistance"�����ɂ̓m�b�N�o�b�N������n���āA���̋�����藣�ꂽ�ʒu�̃R���C�_�[�͖�������
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, knockbackVector, out hitInfo, knockbackDistance, LayerMask.GetMask("Ground")))
        {
            // "Field"���C���[�̃R���C�_�[�Ƀq�b�g���Ă�����A�m�b�N�o�b�N�x�N�g�����q�b�g�����ʒu�܂ł̋����ɏk�߂�

            // ���݈ʒu���烌�C���Ԃ������ʒu�܂ł̋������v�Z
            knockbackDistance = Vector3.Distance(transform.position, hitInfo.point);

            // �m�b�N�o�b�N�x�N�g�����q�b�g�����ʒu�܂ł̋����ɏk�߂�
            // Vector�^��"normalized"�́A�x�N�g���̒�����1�ɂ��ĕԂ��ϐ�
            // ����ɋ�������Z���邱�ƂŁA�x�N�g���̕������ێ����Ē����𔃂��Ă���
            knockbackVector = knockbackVector.normalized * (knockbackDistance - colR);
        }

        // �m�b�N�o�b�N�����ɍ��킹�āA�A�j���[�V�������Ԃ𒲐�����
        float duration = knockbackDistance * speed;

        // DOMove���g���ăm�b�N�o�b�N�ړ������{
        transform.DOMove(transform.position + knockbackVector, duration).SetEase(Ease.OutCubic);
    }
}