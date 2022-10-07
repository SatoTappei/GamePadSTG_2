using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethodパターンを利用した敵の生成
/// このコンポーネントはConcreteCreatorクラスに相当する(ボス担当)
/// </summary>
public class DragonCreatorTest : EnemyCreatorTest
{
    public override EnemyTest Create(string name, int hp)
    {
        name = name + "(ボス)";
        EnemyTest enemy = new DragonTest(name, hp);
        return enemy;
    }
}
