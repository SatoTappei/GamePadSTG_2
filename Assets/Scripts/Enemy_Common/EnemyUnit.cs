using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �G�L�����N�^�[�̐�����s��
/// </summary>
public class EnemyUnit : ActorUnit
{
    EnemyAIBase _aiBase;
    //ActorDataSO _actorData;
    //int _currentHP;

    //[SerializeField] CharacterTag _enemyTag;

    //public CharacterTag EnemyTag { get => _enemyTag; }
    //public Sprite Icon { get => _actorData.Icon; }

    void Awake()
    {
        _aiBase = GetComponent<EnemyAIBase>();
    }

    void Update()
    {

    }

    /// <summary>�G���N����</summary>
    public void WakeUp()
    {
        _aiBase.WakeUp();
    }

    /// <summary>�_���[�W���󂯂��ۂ̉��o</summary>
    protected override void OnDamageReceived()
    {
        transform.DOShakePosition(InGameUtility.HitStopTime, 0.15f, 25, fadeOut: false);
    }

    /// <summary>�_���[�W��^�����ۂ̉��o</summary>
    protected override void OnDamageSended()
    {
        // ������
    }
}
