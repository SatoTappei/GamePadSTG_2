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
public class ChaseStraightEnemyAI : MonoBehaviour
{
    /// <summary>現在のステートに対応したクラス</summary>
    ChaseStraightEnemyBase _currentStateClass;

    void Start()
    {
        // ターゲットは現状Playerのみ、タグを変えることでターゲットを変えることが出来る
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        Animator anim = GetComponentInChildren<Animator>();
        _currentStateClass = new ChaseStraightEnemyIdle(gameObject, target, anim);
    }

    void Update()
    {
        _currentStateClass = _currentStateClass.Process();
    }
}
