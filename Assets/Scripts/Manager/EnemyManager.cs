using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �X�e�[�W��̓G���Ǘ�����
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>�X�e�[�W��ɑ��݂���G�̃��X�g</summary>
    List<EnemySubjecter> _enemyList = new List<EnemySubjecter>();

    void Start()
    {
        
    }

    void Update()
    {

    }

    // ���������G��T���ă��X�g�ɒǉ����Ȃ��͓̂��I�ɐ������ꂽ�G�ɑΉ������邽��
    /// <summary>�������ꂽ�G�������̃��\�b�h���Ă�Ŏ��g��o�^����</summary>
    public void AddEnemyList(EnemySubjecter enemy) => _enemyList.Add(enemy);

    /// <summary>�S�Ă̓G���N����</summary>
    public void WakeUpEnemyAll() => _enemyList.ForEach(e => e.WakeUp());

    // TODO:��ꂽ�̂Ŏ��ۂɃe�X�g����A�{�Ԃł̎g�����̓^�[�Q�b�g�̎c��l����\������̂Ɏg��
    // �G�����ʂ���^�O���n�����̂ŁA���̃^�O�ɂ������G�̐������X�g�ŕԂ����\�b�h
    public int AmountFromTag(EnemyTag tag) => _enemyList.Count(e => e.Tag == tag);
}
