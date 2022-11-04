using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��X�ǂ������̏ꏊ�Ɉڂ�
/// <summary>�G�����ʂ��邽�߂̃^�O�Ƃ��Ďg��</summary>
public enum EnemyTag
{
    BlueSoldier,
    Tank,
    BossTank,
}

/// <summary>
/// �G�̏���EnemyManager�ɓo�^����
/// </summary>
public class EnemySubjecter : MonoBehaviour
{
    EnemyAIBase _aiBase;
    // �G�̃A�C�R���A���݂͂����ɏ����Ă��邪��X�G�̃f�[�^���܂Ƃ߂��ʂ̏ꏊ�Ɉڂ����Ƃ��l������
    [SerializeField] Sprite _icon;
    [SerializeField] EnemyTag _enemyTag;

    public EnemyTag EnemyTag { get => _enemyTag; }
    public Sprite Icon { get => _icon; }

    void Awake()
    {
        _aiBase = GetComponent<EnemyAIBase>();
    }

    void Start()
    {
        // �@�\�����邩�ǂ������Ǘ����Ă��炤���߂Ɏ��g��o�^����
        FindObjectOfType<EnemyManager>().AddEnemyList(this);
    }

    void Update()
    {
        
    }

    // �G���N����
    public void WakeUp()
    {
        _aiBase.WakeUp();
    }
}
