using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flyweightパターンを用いて敵を生成する
/// </summary>
public class EnemyFactoryTest : MonoBehaviour
{
    [SerializeField] List<EnemyParamTest> _enemyParam = new List<EnemyParamTest>();
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] Transform _enemyContainer;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>呼び出されると敵を生成する</summary>
    public void OnCreateEnemy(int idx)
    {
        GameObject enemy = Instantiate(_enemyPrefab, _enemyContainer);
        FlyweightEnemyAITest enemyAI = enemy.GetComponent<FlyweightEnemyAITest>();
        enemyAI.Param = _enemyParam[idx];
        enemyAI.Player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
