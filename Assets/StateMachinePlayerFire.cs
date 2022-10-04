using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンを使用してプレイヤーの攻撃を制御する
/// </summary>
public class StateMachinePlayerFire : StateMachineBehaviour
{
    PlayerFire _playerFire;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animatorにはPlayerの子であるHeroに付いているAnimatorが渡される
        _playerFire = animator.GetComponentInParent<PlayerFire>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
