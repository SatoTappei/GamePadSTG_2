using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �X�e�[�g�}�V�����g�p���ăv���C���[�̍U���𐧌䂷��
/// </summary>
public class StateMachinePlayerFire : StateMachineBehaviour
{
    PlayerFire _playerFire;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo _, int __)
    {
        // animator�ɂ�Player�̎q�ł���Hero�ɕt���Ă���Animator���n�����
        _playerFire = animator.GetComponentInParent<PlayerFire>();
        // ����̃R���C�_�[��L����
        ExecuteEvents.Execute<IWeaponControl>(_playerFire.Weapon, null, (reciever, _) => reciever.EnableWeaponCollider());
        Debug.Log("�L��");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateExit(Animator _, AnimatorStateInfo __, int ___)
    {
        // ����̃R���C�_�[�𖳌���
        ExecuteEvents.Execute<IWeaponControl>(_playerFire.Weapon, null, (reciever, _) => reciever.DisableWeaponCollider());
        Debug.Log("����");
    }
}
