using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Flyweightパターンを用いて生成した敵のクラス
/// </summary>
public class FlyweightEnemyAITest : MonoBehaviour
{
    EnemyParamTest _param;
    public EnemyParamTest Param { get; set; }
    int _currentHp;
    NavMeshAgent _agent;
    Transform _player;
    public Transform Player { set => _player = value; }
    public Renderer _bodyRenderer;

    EnemyStateTest _currentState; // 現在の状態

    /// <summary>プロパティIDを事前に計算</summary>
    static int colorId = Shader.PropertyToID("_Color");

    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        // 体力の設定
        _currentHp = _param._maxHp;

        // カラーの設定、material.SetColorだとマテリアルが複製されてしまうので以下のように行う
        var materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor(colorId, _param._bodyColor);
        _bodyRenderer.SetPropertyBlock(materialPropertyBlock);

        // 最初の状態
        _currentState = new TestIdle(this.gameObject, _agent, _player);
    }

    void Update()
    {
        // 現在の状態を実行。戻り値は次の状態
        _currentState = _currentState.Process();
    }
}
