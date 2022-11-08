using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

// �R���C�_�[���t�����I�u�W�F�N�g�ɃA�^�b�`���邾��
// �v���C���[�ƓG�ŋ��ʂ̉��o�͂������ɏ����Ă���
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
    /// <summary>�\����\����؂�ւ��Ďg���܂킷�_���[�W�G�t�F�N�g</summary>
    GameObject _damageEffect;
    /// <summary>���G���Ԓ����ǂ����̃t���O</summary>
    bool _isInvincible;
    /// <summary>�_���[�W���󂯂��Ƃ��ɍs���ǉ��̏���</summary>
    public UnityAction OnDamageReceived;

    void Awake()
    {
        // �g���܂킹��悤�Ƀ_���[�W�G�t�F�N�g���C���X�^���X�����Ĕ�A�N�e�B�u�ɂ��Ă���
        if (_damageEffectPrefab != null)
        {
            _damageEffect = Instantiate(_damageEffectPrefab);
            _damageEffect.SetActive(false);
        }
    }

    /// <summary>�U�����󂯂��Ƃ��̏���</summary>
    public void OnDamage(int _, Vector3 hitPos)
    {
        // ���G���Ԓ��͍U�����󂯂Ȃ�
        if (_isInvincible) return;
        // ���G���Ԃ�ݒ肷��
        Sequence sequence = DOTween.Sequence()
            .AppendInterval(_invincibleTime)
            .OnStart(() => _isInvincible = true)
            .OnComplete(() => _isInvincible = false);

        // �Փˈʒu�Ƀp�[�e�B�N���𔭐�������
        if (_damageEffect != null) 
            EnabledDamageEffect(hitPos);

        // �q�b�g�X�g�b�v�̌�A�m�b�N�o�b�N������
        DOVirtual.DelayedCall(ConstValue.HitStopTime, () => KnockBack(hitPos, _knockBackPower));

        // �_���[�W����
        OnDamageReceived?.Invoke();
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
    }
}
