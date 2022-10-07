using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethodパターンを利用した敵の生成
/// このコンポーネントはConcreteProductクラスに相当する
/// </summary>
public class FactoryControllerTest : MonoBehaviour
{
    void Start()
    {
        EnemyCreatorTest goblinCreator = new GoblinCreatorTest();
        EnemyTest goblin = goblinCreator.Create("ゴブリン", 100);
        goblin.Attack();

        EnemyCreatorTest dragonCreator = new DragonCreatorTest();
        EnemyTest dragon = dragonCreator.Create("ドラゴン", 9999);
        dragon.Attack();
    }

    void Update()
    {
        
    }
}
