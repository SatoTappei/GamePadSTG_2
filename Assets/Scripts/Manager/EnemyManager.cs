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
    /// <summary>�X�e�[�W��ɑ��݂���G�̃��X�g</summary>
    List<EnemyUnit> _enemyList = new List<EnemyUnit>();
    /// <summary>�������Ă���^�[�Q�b�g�̃��X�g</summary>
    ReactiveCollection<EnemyUnit> _targets = new ReactiveCollection<EnemyUnit>();

    /// <summary>�^�[�Q�b�g��"��������"�s��������o�^����</summary>
    public IObservable<CollectionRemoveEvent<EnemyUnit>> TargetsObservable => _targets.ObserveRemove();

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
            EnemyUnit es = go.GetComponent<EnemyUnit>();
            _enemyList.Add(es);

            // �n���ꂽ�^�O�Ɠ�����������^�[�Q�b�g�̃��X�g�ɒǉ���
            // ��\���ɂȂ���(�|���ꂽ)�烊�X�g����폜�����悤�ɂ��邱�ƂŃ^�[�Q�b�g�r���[�̍X�V���Ă�
            if (es.ActorData.Tag == tag)
            {
                _targets.Add(es);
                es.gameObject.OnDisableAsObservable().Subscribe(_ => _targets.Remove(es));
            }
        }
    }

    /// <summary>�c��̃^�[�Q�b�g����Ԃ�</summary>
    public int GetTargetAmount() => _targets.Count;

    /// <summary>�S�Ă̓G���N����</summary>
    public void WakeUpEnemyAll() => _enemyList.ForEach(e => e.WakeUp());
}
