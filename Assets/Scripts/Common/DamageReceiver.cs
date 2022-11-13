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

    // ���C�L���X�g�̃e�X�g�A�����Ă����Ȃ�
    //Vector3 _rayDir = Vector3.up;
    //Vector3 _prevPos;

    void Awake()
    {
        // �g���܂킹��悤�Ƀ_���[�W�G�t�F�N�g���C���X�^���X�����Ĕ�A�N�e�B�u�ɂ��Ă���
        if (_damageEffectPrefab != null)
        {
            _damageEffect = Instantiate(_damageEffectPrefab);
            _damageEffect.SetActive(false);
        }
    }

    // TODO:���C�L���X�g���g�p�����q�b�g�o�b�N�̃L�����Z����������
    // ���C�L���X�g�̃e�X�g���������Ă��Ȃ��̂�Update�������Ă����Ȃ�
    void Update()
    {
        //Vector3 rayPos = transform.position + new Vector3(0, 1f, 0);
        //Ray ray = new Ray(rayPos, _rayDir);

        //RaycastHit hit;
        //bool isHit = Physics.Raycast(ray, out hit,0.2f);
        //// �ǂɃ��C���q�b�g������O�t���[���̍��W�ɖ߂�
        //if (isHit)
        //{
        //    transform.position = _prevPos;
        //}
        //else
        //{
        //    _prevPos = transform.position;
        //}
        //Debug.DrawRay(rayPos, _rayDir * 0.2f, Color.red);
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
        DOVirtual.DelayedCall(InGameUtility.HitStopTime, () => KnockBack(hitPos, _knockBackPower));

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
    void KnockBack(Vector3 hit, float power)
    {
        // �m�b�N�o�b�N��������������߂�
        Vector3 pos = transform.position;
        pos.y = 0;
        hit.y = 0;
        Vector3 dir = Vector3.Normalize(pos - hit);

        // �m�b�N�o�b�N
        transform.DOMove(dir * power, 0.5f).SetRelative();

        // �e�X�g:���C��ݒ肷��
        //_rayDir = dir;
    }
}

// TODO: �A�j���[�^�[���ǂ�����̌x���Ƃʂ�ۂ𒼂��B