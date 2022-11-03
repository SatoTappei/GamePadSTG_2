using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ステージ上の敵を管理する
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>ステージ上に存在する敵のリスト</summary>
    List<EnemySubjecter> _enemyList = new List<EnemySubjecter>();

    void Start()
    {
        
    }

    void Update()
    {

    }

    // こっちが敵を探してリストに追加しないのは動的に生成された敵に対応させるため
    /// <summary>生成された敵側がこのメソッドを呼んで自身を登録する</summary>
    public void AddEnemyList(EnemySubjecter enemy) => _enemyList.Add(enemy);

    /// <summary>全ての敵を起こす</summary>
    public void WakeUpEnemyAll() => _enemyList.ForEach(e => e.WakeUp());

    // TODO:作れたので実際にテストする、本番での使い方はターゲットの残り人数を表示するのに使う
    // 敵を識別するタグが渡されるので、そのタグにあった敵の数をリストで返すメソッド
    public int AmountFromTag(EnemyTag tag) => _enemyList.Count(e => e.Tag == tag);
}
