using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// このスクリプトを"Enemy_敵名"のオブジェクトにアタッチして
// ChaseStraightEnemyBase内の各アニメーション名定数を書き換えれば動く
/// <summary>
/// 敵の行動をStateパターンで実装している
/// ターゲットに向かって真っ直ぐ近づいて近接攻撃してくる敵
/// 現在の状態における行動を実行する
/// </summary>
public class ChaseStraightEnemyAI : EnemyAIBase
{
    public override void Init()
    {
        // ターゲットは現状Playerのみ、タグを変えることでターゲットを変えることが出来る
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        Animator anim = GetComponentInChildren<Animator>();
        GameObject findIcon = transform.Find("FindIcon").gameObject;
        _currentStateClass = new ChaseStraightEnemyIdle(gameObject, target, anim, findIcon);
    }
}
