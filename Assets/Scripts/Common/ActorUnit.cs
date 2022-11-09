using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �eUnit�N���X(EnemyUnit,PlayerUnit)�̊��N���X
/// </summary>
public abstract class ActorUnit : MonoBehaviour
{
    [SerializeField] DamageReceiver _damageReciever;
    [SerializeField] DamageSender _damageSender;
    [SerializeField] CharacterTag _characterTag;
    [Header("Unity�̋@�\�̃^�O")] // TODO:������CharacterTag�ɒ��������Astatic���\�b�h�Ŕ��肵�āc�݂�����
    [SerializeField] string _targetTag;

    public ActorDataSO ActorData { get; private set; }

    void Start()
    {
        // TODO:PlaySceneManager��Start()�Ń��\�b�h�����s���Ă���̂ŏ������ɂ���Ă͂ʂ��
        // �A�N�^�[�f�[�^���Q�Ɛ悩��Ƃ��Ă���A�vCharacterTag
        ActorDataManager adm = FindObjectOfType<ActorDataManager>();
        ActorData = adm.GetEnemyData(_characterTag);

        // �̗͂̓��V�[�o�[�ɐݒ�
        if (_damageReciever != null)
            _damageReciever.Init(ActorData.MaxHP);
        // �U���͂ƃ^�[�Q�b�g�̓Z���_�[�ɐݒ�
        if (_damageSender != null)
            _damageSender.Init(ActorData.Attack, _targetTag);
    }

    void OnEnable()
    {
        if(_damageReciever != null)
            _damageReciever.OnDamageReceived += OnDamageReceived;
        if (_damageSender != null)
            _damageSender.OnDamageSended += OnDamageSended;
    }

    void OnDisable()
    {
        if (_damageReciever != null)
            _damageReciever.OnDamageReceived -= OnDamageReceived;
        if (_damageSender != null)
            _damageSender.OnDamageSended -= OnDamageSended;
    }

    protected abstract void OnDamageReceived();
    protected abstract void OnDamageSended();
}