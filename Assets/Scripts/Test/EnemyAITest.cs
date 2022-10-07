using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Stateパターンを用いた実装のテスト
/// 状況を判断し、その場に応じたステートを実行する
/// </summary>
public class EnemyAITest : MonoBehaviour
{
    // EnemyStateTestが必要としている変数
    NavMeshAgent _agent;        // 敵キャラのNavMeshAgentコンポーネント
    public Transform _player;   // プレイヤーのTransformコンポーネント

    EnemyStateTest _currentState;      // 現在の状態

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        // 最初の状態
        _currentState = new Idle(gameObject, _agent, _player);
    }

    void Update()
    {
        // 現在の状態を実行する、戻り値は次の状態
        _currentState = _currentState.Process();
    }
}
