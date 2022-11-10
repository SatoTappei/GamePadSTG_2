using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// �X�e�[�g�}�V�����g�p���ăv���C���[�̍U���A�j���[�V�����Đ����ɃC�x���g���N����
/// </summary>
public class StateMachinePlayerFire : StateMachineBehaviour
{
    PlayerFire _playerFire;

    /// <summary>�R���C�_�[���I���ɂȂ��Ă��鎞��</summary>
    [SerializeField, Range(0, 1.0f)] float _duration;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo _, int __)
    {
        // animator�ɂ�Player�̎q�ł���Hero�ɕt���Ă���Animator���n�����
        if (_playerFire == null)
            _playerFire = animator.GetComponentInParent<PlayerFire>();

        // ����̃R���C�_�[��L��������̂�Execute���g�p����
        // ��1����:�C���^�[�t�F�[�X���p�������N���X�̃R���|�[�l���g���A�^�b�`�����I�u�W�F�N�g
        // ��2����:�C�x���g�f�[�^
        // ��3����:��1�����Ŏw�肵���I�u�W�F�N�g�ɃA�^�b�`���ꂽ�R���|�[�l���g, �C�x���g�f�[�^ => �����_��
        ExecuteEvents.Execute<IAttackAnimControl>(_playerFire.Weapon, null, (reciever, _) =>
        {
            // �A�j���[�V�����J�n���Ɠ����ɃR���C�_�[��L���ɂ��āA�o�ߎ��ԂŃR���C�_�[�𖳌��ɂ���
            reciever.OnAnimEnter();
            DOVirtual.DelayedCall(_duration, () => reciever.OnAnimExit());
        });

        // PlayerMove�̍U�����Ɉړ��ł��Ȃ����鏈�����Ă΂��
        ExecuteEvents.Execute<IAttackAnimControl>(_playerFire.gameObject, null, (reciever, _) =>
        {
            // �A�j���[�V�����J�n���Ɠ����Ɉړ��s�\�ɂ��āA�o�ߎ��Ԃňړ��\�ɂ���
            reciever.OnAnimEnter();
            DOVirtual.DelayedCall(_duration, () => reciever.OnAnimExit());
        });
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateExit(Animator _, AnimatorStateInfo __, int ___)
    {

    }
}
