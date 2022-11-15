using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using System;

/// <summary>
/// �G�̍s����State�p�^�[���Ŏ������Ă���
/// �G��AI�N���X�͂��̃N���X���p�����č�邱��
/// </summary>
public abstract class EnemyAIBase : MonoBehaviour
{
    /// <summary>�G���@�\�����邩�ǂ����𔻒f����</summary>
    protected bool _isWakeUp;
    /// <summary>���݂̃X�e�[�g�ɑΉ������N���X</summary>
    protected StateMachineBase _currentStateClass;

    protected async void Start()
    {
        await UniTask.WaitUntil(() => _isWakeUp);
        Init();

        this.UpdateAsObservable()
            .Subscribe(_ => Stay())
            .AddTo(this);
    }
    
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Exit();
        }
#endif
    }

    /// <summary>�G�Ƃ��ċ@�\������</summary>
    public void WakeUp() => _isWakeUp = true;
    
    /// <summary>�G�Ƃ��ċ@�\���������ɍŏ��̈�񂾂��Ă΂��</summary>
    public abstract void Init();

    /// <summary>�G�Ƃ��ċ@�\���Ă���Ԗ��t���[���Ă΂��</summary>
    public void Stay()
    {
        _currentStateClass = _currentStateClass.Process();
    }

    /// <summary>����ȏ㓮���Ȃ��悤�ɓG���~�߂鎞�ɑ��̃X�N���v�g����Ă�</summary>
    public void Exit()
    {
        if (_currentStateClass != null)
        {
            _currentStateClass.ToCompleted();
            _currentStateClass.Process();
        }
    }
}
