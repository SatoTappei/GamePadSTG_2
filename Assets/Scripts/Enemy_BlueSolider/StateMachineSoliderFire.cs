using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// �X�e�[�g�}�V�����g�p���ēG�̋ߐڍU���A�j���[�V�����Đ����ɃC�x���g���N����
/// </summary>

public class StateMachineSoliderFire : StateMachineBehaviour
{
    SoliderFire _soliderFire;

    [SerializeField, Range(0, 1.0f)] float _duration;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo _, int __)
    {
        if (_soliderFire == null)
            _soliderFire = animator.GetComponentInParent<SoliderFire>();

        ExecuteEvents.Execute<IAttackAnimControl>(_soliderFire.Weapon, null, (reciever, _) =>
        {
            // �A�j���[�V�����J�n���Ɠ����ɃR���C�_�[��L���ɂ��āA�o�ߎ��ԂŃR���C�_�[�𖳌��ɂ���
            reciever.OnAnimEnter();
            DOVirtual.DelayedCall(_duration, () => reciever.OnAnimExit());
        });
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo _, int __)
    {

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo _, int __)
    {

    }
}
