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
        // �̗͂̐ݒ�
        _currentHp = _param._maxHp;
        // �ŏ��̏��
        // currentState = new Idle(gameObject, _player);
    }

    void Update()
    {
        // ���݂̏�Ԃ����s����
        // currentState = currentState.Process();
    }
}
