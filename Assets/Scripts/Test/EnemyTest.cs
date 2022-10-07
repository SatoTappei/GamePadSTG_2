using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethodパターンを利用した敵の生成
/// このコンポーネントはProductクラスに相当する
/// </summary>
public abstract class EnemyTest : MonoBehaviour
{
    protected string _name;
    protected int _hp;

    public abstract string GetName();
    public abstract int GetHp();
    public abstract void Attack();
}
