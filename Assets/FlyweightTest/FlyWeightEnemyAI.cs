using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyWeightEnemyAI : MonoBehaviour
{
    ParamSO _param;
    public ParamSO Param { get; set; }

    int _currentHp;

    Transform _player;
    public Transform Player { get; set; }

    void Start()
    {
        // 体力の設定
        _currentHp = _param._maxHp;
        // 最初の状態
        // currentState = new Idle(gameObject, _player);
    }

    void Update()
    {
        // 現在の状態を実行する
        // currentState = currentState.Process();
    }
}
