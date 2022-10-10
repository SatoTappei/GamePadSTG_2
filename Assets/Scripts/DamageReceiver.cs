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
    ///// <summary>Rigidbody���g�����ǂ���</summary>
    //[SerializeField] bool _useRigidbody;
    [SerializeField] GameObject _damageEffectPrefab;
    GameObject _damageEffect;

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

        // �e�퉉�o
        Sequence seqE = DOTween.Sequence();
        // ����Ɉړ�
        Vector3 knockBackVec = GetAngleVec(hitPos, transform.position);
        seqE.Append(transform.DOMove(knockBackVec * _knockBackPower, 0.5f).SetRelative());
        // �R�[���o�b�N����
        OnDamageReceived?.Invoke();

        //if (_useRigidbody)
        //{
        //    // RigidBody���g��������
        //    _rb.AddForce(knockBackVec * _knockBackPower, ForceMode.Impulse);
        //}
        //else
        //{
        //    // DotWeen���g��������
        //    transform.DOMove(knockBackVec * _knockBackPower, 0.5f).SetRelative();
        //}
    }

    /// <summary>������΂����������߂�</summary>
    Vector3 GetAngleVec(Vector3 from, Vector3 to)
    {
        from.y = 0;
        to.y = 0;
        return Vector3.Normalize(to - from);
    }
}
