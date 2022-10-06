using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ステートマシンを使用してプレイヤーの攻撃を制御する
/// </summary>
public class StateMachinePlayerFire : StateMachineBehaviour
{
    PlayerFire _playerFire;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo _, int __)
    {
        // animatorにはPlayerの子であるHeroに付いているAnimatorが渡される
        _playerFire = animator.GetComponentInParent<PlayerFire>();
        // 武器のコライダーを有効化
        ExecuteEvents.Execute<IWeaponControl>(_playerFire.Weapon, null, (reciever, _) => reciever.EnableWeaponCollider());
        Debug.Log("有効");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateExit(Animator _, AnimatorStateInfo __, int ___)
    {
        // 武器のコライダーを無効化
        ExecuteEvents.Execute<IWeaponControl>(_playerFire.Weapon, null, (reciever, _) => reciever.DisableWeaponCollider());
        Debug.Log("無効");
    }
}
