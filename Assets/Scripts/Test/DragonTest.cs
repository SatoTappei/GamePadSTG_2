using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethodパターンを利用した敵の生成
/// このコンポーネントはConcreteProductクラスに相当する(ボス担当)
/// </summary>
public class DragonTest : EnemyTest
{
    public DragonTest(string name, int hp)
    {
        _name = name;
        _hp = hp;
    }

    public override void Attack()
    {
        Debug.Log($"{_name}への攻撃！");
    }

    public override int GetHp() => _hp;

    public override string GetName() => _name;
}
