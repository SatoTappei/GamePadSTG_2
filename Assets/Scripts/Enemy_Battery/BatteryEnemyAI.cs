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

    /// <summary>現在のステートに対応したクラス</summary>
    BatteryEnemyBase _currentStateClass;

    public override void Init()
    {
        // ターゲットは現状Playerのみ、タグを変えることでターゲットを変えることが出来る
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        Animator anim = GetComponentInChildren<Animator>();
        _currentStateClass = new BatteryEnemyIdle(gameObject, target, anim, _turret);
    }

    public override void Stay()
    {
        _currentStateClass = _currentStateClass.Process();
    }

    public override void Exit()
    {
        if (_currentStateClass != null)
        {
            _currentStateClass.ChangeCompleted();
            _currentStateClass.Process();
        }
    }

    // TODO:戦車の死亡時のプレファブが常に正面を向いてしまうので死亡時に向いていた方向に生成されるよう直す
    // TODO:敵のラグドールがプレイヤーとの当たり判定をもたないに直す。レイヤーで設定する。
}
