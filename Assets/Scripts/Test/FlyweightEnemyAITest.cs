using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Flyweight�p�^�[����p���Đ��������G�̃N���X
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

    EnemyStateTest _currentState; // ���݂̏��

    /// <summary>�v���p�e�BID�����O�Ɍv�Z</summary>
    static int colorId = Shader.PropertyToID("_Color");

    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        // �̗͂̐ݒ�
        _currentHp = _param._maxHp;

        // �J���[�̐ݒ�Amaterial.SetColor���ƃ}�e���A������������Ă��܂��̂ňȉ��̂悤�ɍs��
        var materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor(colorId, _param._bodyColor);
        _bodyRenderer.SetPropertyBlock(materialPropertyBlock);

        // �ŏ��̏��
        _currentState = new TestIdle(this.gameObject, _agent, _player);
    }

    void Update()
    {
        // ���݂̏�Ԃ����s�B�߂�l�͎��̏��
        _currentState = _currentState.Process();
    }
}
