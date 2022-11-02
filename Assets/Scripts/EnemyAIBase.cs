using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �G�̍s����State�p�^�[���Ŏ������Ă���
/// �G��AI�N���X�͂��̃N���X���p�����č�邱��
/// </summary>
public abstract class EnemyAIBase : MonoBehaviour
{
    /// <summary>�G���@�\�����邩�ǂ����𔻒f����</summary>
    protected bool _isWakeUp;

    protected async void Start()
    {
        // �@�\�����邩�ǂ������Ǘ����Ă��炤���߂Ɏ��g��o�^����
        // ���̂Ƃ���Ŏg���܂킷�ꍇ�͉��̍s�������Ηǂ�
        FindObjectOfType<EnemyManager>().AddEnemyList(this);

        await UniTask.WaitUntil(() => _isWakeUp);
        Init();

        this.UpdateAsObservable()
            .Subscribe(_ => Stay())
            .AddTo(this);
    }
    
    void Update()
    {
        
    }

    /// <summary>�G�Ƃ��ċ@�\������</summary>
    public void WakeUp() => _isWakeUp = true;
    /// <summary>�G�Ƃ��ċ@�\���������ɍŏ��̈�񂾂��Ă΂��</summary>
    public abstract void Init();
    /// <summary>�G�Ƃ��ċ@�\���Ă���Ԗ��t���[���Ă΂��</summary>
    public abstract void Stay();
}
