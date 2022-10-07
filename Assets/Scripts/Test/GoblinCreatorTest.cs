using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethodパターンを利用した敵の生成
/// このコンポーネントはConcreteCreatorクラスに相当する
/// </summary>
public class GoblinCreatorTest : EnemyCreatorTest
{
    public override EnemyTest Create(string name, int hp)
    {
        EnemyTest enemy = new GoblinTest(name, hp);
        return enemy;
    }
}
