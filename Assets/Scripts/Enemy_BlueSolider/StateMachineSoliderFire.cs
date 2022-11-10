using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// ステートマシンを使用して敵の近接攻撃アニメーション再生時にイベントを起こす
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
            // アニメーション開始時と同時にコライダーを有効にして、経過時間でコライダーを無効にする
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
