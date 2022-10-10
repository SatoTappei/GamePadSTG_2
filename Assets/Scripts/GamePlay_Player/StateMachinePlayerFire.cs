using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// ステートマシンを使用してプレイヤーの攻撃を制御する
/// </summary>
public class StateMachinePlayerFire : StateMachineBehaviour
{
    PlayerFire _playerFire;
    /// <summary>コライダーがオンになっている時間</summary>
    [SerializeField, Range(0, 1.0f)] float _duration;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo _, int __)
    {
        // animatorにはPlayerの子であるHeroに付いているAnimatorが渡される
        if (_playerFire == null)
            _playerFire = animator.GetComponentInParent<PlayerFire>();
        // 武器のコライダーを有効化するのにExecuteを使用する
        // 第1引数:インターフェースを継承したクラスのコンポーネントをアタッチしたオブジェクト
        // 第2引数:イベントデータ
        // 第3引数:第1引数で指定したオブジェクトにアタッチされたコンポーネント, イベントデータ => ラムダ式
        ExecuteEvents.Execute<IWeaponControl>(_playerFire.Weapon, null, (reciever, _) =>
        {
            // アニメーション開始時と同時にコライダーを有効にして、経過時間でコライダーを無効にする
            reciever.EnableCollider();
            DOVirtual.DelayedCall(_duration, () => reciever.DisableCollider());
        });
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateExit(Animator _, AnimatorStateInfo __, int ___)
    {
        //// 武器のコライダーを無効化
        //ExecuteEvents.Execute<IWeaponControl>(_playerFire.Weapon, null, (reciever, _) => reciever.DisableCollider());
    }
}
