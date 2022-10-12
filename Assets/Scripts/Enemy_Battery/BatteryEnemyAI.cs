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
public class BatteryEnemyAI : MonoBehaviour
{
    [SerializeField] Transform _muzzle;
    [SerializeField] Transform _turret;

    /// <summary>現在のステートに対応したクラス</summary>
    BatteryEnemyBase _currentStateClass;

    void Start()
    {
        // ターゲットは現状Playerのみ、タグを変えることでターゲットを変えることが出来る
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        Animator anim = GetComponentInChildren<Animator>();
        _currentStateClass = new BatteryEnemyIdle(gameObject, target, anim);
    }

    void Update()
    {
        _currentStateClass = _currentStateClass.Process();
    }
}
