using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// このスクリプトを"Enemy_敵名"のオブジェクトにアタッチして
// BatteryEnemyBase内の各アニメーション名定数を書き換えれば動く
/// <summary>
/// 敵の行動をStateパターンで実装している
/// その場から動かず発見次第撃ってくる敵
/// 現在の状態における行動を実行する
/// </summary>
public class BatteryEnemyAI : EnemyAIBase
{
    // 回転砲塔、子オブジェクトにMuzzleという名前のオブジェクトが無いと
    // BatteryEnemyIdleのコンストラクタを呼んだ時点でエラーになるので注意
    [SerializeField] Transform _turret;

    public override void Init()
    {
        // ターゲットは現状Playerのみ、タグを変えることでターゲットを変えることが出来る
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        Animator anim = GetComponentInChildren<Animator>();
        GameObject findIcon = transform.Find("FindIcon").gameObject;
        _currentStateClass = new BatteryEnemyIdle(gameObject, target, anim, _turret, findIcon);
    }
}
