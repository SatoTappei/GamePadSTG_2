using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// ��X�ǂ������̏ꏊ�Ɉڂ�
/// <summary>�L�����N�^�[�����ʂ��邽�߂̃^�O�Ƃ��Ďg��</summary>
public enum CharacterTag
{
    Player,
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
    ActorDataSO _actorData;
    int _currentHP;

    [SerializeField] DamageReceiver _damageReceiver;
    [SerializeField] DamageSender _damageSender;
    [SerializeField] CharacterTag _enemyTag;

    public CharacterTag EnemyTag { get => _enemyTag; }
    public Sprite Icon { get => _actorData.Icon; }

    void Awake()
    {
        _aiBase = GetComponent<EnemyAIBase>();
        //_currentHP = /*_actorData.MaxHP*/100;
    }

    void OnEnable()
    {
        if (_damageSender != null)
            _damageSender.OnDamageSended += OnDamageSended;
        _damageReceiver.OnDamageReceived += OnDamageReceived;

    }

    void OnDisable()
    {
        if (_damageSender != null)
            _damageSender.OnDamageSended -= OnDamageSended;
        _damageReceiver.OnDamageReceived -= OnDamageReceived;
    }

    void Start()
    {
        //// TODO:���݂͓G�𑝂₵�����̂��߂ɂ���������Ǘ�����悤�o�^���Ă��邪
        ////      EnemyManager����o�^����悤�ɕύX�ł��Ȃ����͍�����
        EnemyManager em = FindObjectOfType<EnemyManager>();
        //// �@�\�����邩�ǂ������Ǘ����Ă��炤���߂Ɏ��g��o�^����
        //em.AddEnemyList(this);
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
    void OnDamageReceived()
    {
        transform.DOShakePosition(ConstValue.HitStopTime, 0.15f, 25, fadeOut: false);
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
    void OnDamageSended()
    {
        // ������
    }
}
