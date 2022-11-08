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
    ActorDataSO _actorData;
    int _currentHP;

    [SerializeField] CharacterTag _enemyTag;

    public CharacterTag EnemyTag { get => _enemyTag; }
    public Sprite Icon { get => _actorData.Icon; }

    void Awake()
    {
        _aiBase = GetComponent<EnemyAIBase>();
    }

    void Start()
    {
        EnemyManager em = FindObjectOfType<EnemyManager>();
        //// ���ʂ����f�[�^�̎Q�Ɛ���擾����
        _actorData = em.GetEnemyData(EnemyTag);
        _currentHP = _actorData.MaxHP;
    }

    void Update()
    {
        
    }

    // �G���N����
    public void WakeUp()
    {
        _aiBase.WakeUp();
    }

    /// <summary>�_���[�W���󂯂��ۂ̉��o</summary>
    protected override void OnDamageReceived()
    {
        transform.DOShakePosition(InGameUtility.HitStopTime, 0.15f, 25, fadeOut: false);
        // �_���[�W���󂯂��Ƃ��Ɏ��񂾂��ǂ������肵����
        // HP�����炵��0�ȉ���������Ɣۂ��ŕ��򂷂�
        _currentHP -= 30; // �e�X�g�Œ�l
        if (_currentHP <= 0)
        {
            // �|���ꂽ���\���ɂȂ�
            gameObject.SetActive(false);
        }
    }

    /// <summary>�_���[�W��^�����ۂ̉��o</summary>
    protected override void OnDamageSended()
    {
        // ������
    }
}
