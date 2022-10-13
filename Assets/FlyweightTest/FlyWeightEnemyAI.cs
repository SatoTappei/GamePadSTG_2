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
        // ‘Ì—Í‚Ìİ’è
        _currentHp = _param._maxHp;
        // Å‰‚Ìó‘Ô
        // currentState = new Idle(gameObject, _player);
    }

    void Update()
    {
        // Œ»İ‚Ìó‘Ô‚ğÀs‚·‚é
        // currentState = currentState.Process();
    }
}
