using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// このスクリプトを"Enemy_敵名"のオブジェクトにアタッチするだけで動くようにする
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
        _currentStateClass = new ChaseStraightEnemyIdle(gameObject, target);
    }

    void Update()
    {
        _currentStateClass = _currentStateClass.Process();
    }
}
