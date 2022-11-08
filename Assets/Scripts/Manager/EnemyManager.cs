using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// �X�e�[�W��̓G���Ǘ�����
/// </summary>
public class EnemyManager : MonoBehaviour
{
    // SO����̏������������o���ĕʂ̃X�N���v�g�ŊǗ����邱�Ƃ��l�����Ă���
    /// <summary>�G�̎�ނ��Ƃ̋��ʂ����f�[�^�̎Q�Ɛ��SO</summary>
    [SerializeField] ActorDataSO[] _enemyDatas;
    /// <summary>�G�̋��ʂ����f�[�^�̎Q�Ɛ�ƂȂ�SO��l�Ƃ��ĕۑ����鎫���^</summary>
    Dictionary<CharacterTag, ActorDataSO> _enemyDataDic = new Dictionary<CharacterTag, ActorDataSO>();

    /// <summary>�X�e�[�W��ɑ��݂���G�̃��X�g</summary>
    List<EnemySubjecter> _enemyList = new List<EnemySubjecter>();
    /// <summary>�������Ă���^�[�Q�b�g�̃��X�g</summary>
    ReactiveCollection<EnemySubjecter> _targets = new ReactiveCollection<EnemySubjecter>();

    /// <summary>�^�[�Q�b�g��"��������"�s��������o�^����</summary>
    public IObservable<CollectionRemoveEvent<EnemySubjecter>> TargetsObservable => _targets.ObserveRemove();

    void Awake()
    {
        foreach(ActorDataSO so in _enemyDatas)
        {
            _enemyDataDic.Add(so.Tag, so);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>����������</summary>
    public void Init(CharacterTag tag)
    {
        // �^�O�ŃV�[����̓G��S���擾���ă��X�g�ɒǉ�����
        // TODO:����^�O�Ō������ăR���|�[�l���g���擾���Ă���̂ő��̕��@���Ȃ����͍�����
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemySubjecter es = go.GetComponent<EnemySubjecter>();
            _enemyList.Add(es);

            // �n���ꂽ�^�O�Ɠ�����������^�[�Q�b�g�̃��X�g�ɒǉ���
            // ��\���ɂȂ���(�|���ꂽ)�烊�X�g����폜�����悤�ɂ��邱�ƂŃ^�[�Q�b�g�r���[�̍X�V���Ă�
            if (es.EnemyTag == tag)
            {
                _targets.Add(es);
                es.gameObject.OnDisableAsObservable().Subscribe(_ => _targets.Remove(es));
            }
        }
    }

    /// <summary>�c��̃^�[�Q�b�g����Ԃ�</summary>
    public int GetTargetAmount() => _targets.Count;

    /// <summary>�^�O��n���ƑΉ�����SO��Ԃ�</summary>
    public ActorDataSO GetEnemyData(CharacterTag tag) => _enemyDataDic[tag];

    /// <summary>�S�Ă̓G���N����</summary>
    public void WakeUpEnemyAll() => _enemyList.ForEach(e => e.WakeUp());

    ///// <summary>�������ꂽ�G�������̃��\�b�h���Ă�Ŏ��g��o�^����</summary>
    //public void AddEnemyList(EnemySubjecter enemy) => _enemyList.Add(enemy);

    /// <summary>�X�e�[�W���̓G����^�O�œ|���^�[�Q�b�g���擾����</summary>
    //public List<GameObject> GetTarget(CharacterTag tag)
    //{
    //    _targetList = _enemyList
    //        .Where(e => e.EnemyTag == tag).Select(e => e.gameObject).ToList();

    //    // �^�[�Q�b�g����������̏����������ɏ����Ă�������œK�؂ȏꏊ�ɒu��
    //    _targetList.ForEach(go => go.OnDisableAsObservable().Subscribe(_=> DecreaseTargetCount()));

    //    _rem.Value = _targetList.Count;

    //    return _targetList;
    //}
}
