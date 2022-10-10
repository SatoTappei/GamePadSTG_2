using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// �X�e�[�g�}�V�����g�p���ăv���C���[�̍U���𐧌䂷��
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
        ExecuteEvents.Execute<IWeaponControl>(_playerFire.Weapon, null, (reciever, _) =>
        {
            // �A�j���[�V�����J�n���Ɠ����ɃR���C�_�[��L���ɂ��āA�o�ߎ��ԂŃR���C�_�[�𖳌��ɂ���
            reciever.EnableCollider();
            DOVirtual.DelayedCall(_duration, () => reciever.DisableCollider());
        });
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateExit(Animator _, AnimatorStateInfo __, int ___)
    {
        //// ����̃R���C�_�[�𖳌���
        //ExecuteEvents.Execute<IWeaponControl>(_playerFire.Weapon, null, (reciever, _) => reciever.DisableCollider());
    }
}
