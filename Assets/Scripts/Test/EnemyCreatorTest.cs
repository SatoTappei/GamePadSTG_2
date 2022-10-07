using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethodパターンを利用した敵の生成
/// このコンポーネントはProductクラスを利用するCreatorクラスに相当する
/// </summary>
public abstract class EnemyCreatorTest : MonoBehaviour
{
    /// <summary>FactoryMethod()に相当する</summary>
    public abstract EnemyTest Create(string name, int hp);

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
