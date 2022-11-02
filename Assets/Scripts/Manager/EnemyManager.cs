using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ上の敵を管理する
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>ステージ上に存在する敵のリスト</summary>
    List<EnemyAIBase> _enemyList = new List<EnemyAIBase>();

    void Start()
    {
        
    }

    void Update()
    {

    }

    // こっちが敵を探してリストに追加しないのは動的に生成された敵に対応させるため
    /// <summary>生成された敵側がこのメソッドを呼んで自身を登録する</summary>
    public void AddEnemyList(EnemyAIBase ai) => _enemyList.Add(ai);

    /// <summary>全ての敵を起こす</summary>
    public void WakeUpEnemyAll() => _enemyList.ForEach(ai => ai.WakeUp());
}
