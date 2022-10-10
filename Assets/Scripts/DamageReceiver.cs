using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

// �R���C�_�[���t�����I�u�W�F�N�g�ɃA�^�b�`���邾��
/// <summary>
/// DamageSender����_���[�W���󂯂����Ƃ���M����
/// </summary>
public class DamageReceiver : MonoBehaviour, IDamageable
{
    //Rigidbody _rb;
    /// <summary>�m�b�N�o�b�N��</summary>
    [SerializeField] float _knockBackPower = 3.0f;
    /// <summary>���G����</summary>
    [SerializeField] float _invincibleTime = 2.0f;
    [SerializeField] GameObject _damageEffectPrefab;
    GameObject _damageEffect;

    /// <summary>�_���[�W���󂯂��Ƃ��ɍs���ǉ��̏���</summary>
    public event Action OnDamageReceived;

    bool _isInvincible;

    void Awake()
    {
        //_rb = GetComponent<Rigidbody>();

        // Awake���_�Ń_���[�W�G�t�F�N�g���C���X�^���X������
        // ��A�N�e�B�u�ɂ��Ă���
        if (_damageEffectPrefab != null)
        {
            _damageEffect = Instantiate(_damageEffectPrefab);
            _damageEffect.SetActive(false);
        }
    }

    /// <summary>�G����U�����󂯂��Ƃ��̏���</summary>
    public void OnDamage(int damageValue, Vector3 hitPos)
    {
        // ���G���̏ꍇ�͍U�����󂯂Ȃ�
        if (_isInvincible)
        {
            Debug.Log("���G���Ԓ�:" + gameObject.name);
            return;
        }
        else
        {
            Debug.Log("�_���[�W���󂯂�:" + gameObject.name);
        }

        // �Փˈʒu�Ƀp�[�e�B�N���𔭐�
        if (_damageEffect != null)
        {
            _damageEffect.transform.position = hitPos;
            _damageEffect.SetActive(true);
        }

        // ��莞�Ԗ��G
        Sequence sequence = DOTween.Sequence()
            .AppendInterval(_invincibleTime)
            .OnStart(() => _isInvincible = true)
            .OnComplete(() => _isInvincible = false);

        // TODO:�m�b�N�o�b�N������ƕǂ��ђʂ��Ă��܂��̂Ń��C�L���X�g��p����Ȃǂ��ĕǂɖ��܂�Ȃ��悤�ɂ���
        KnockBack(hitPos, _knockBackPower);

        OnDamageReceived?.Invoke();
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
    }
}
