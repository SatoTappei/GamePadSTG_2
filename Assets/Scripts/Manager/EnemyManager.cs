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

    /// <summary>�X�e�[�W���̓G����^�O�œ|���^�[�Q�b�g���擾����</summary>
    public List<GameObject> GetTarget(EnemyTag tag) => _enemyList.Where(e => e.EnemyTag == tag)
                                                                 .Select(e => e.gameObject)
                                                                 .ToList();

    /// <summary>�^�O�Ń^�[�Q�b�g�̃A�C�R�����擾����</summary>
    public Sprite GetTargetIcon(EnemyTag tag) => _enemyList.Where(e => e.EnemyTag == tag)
                                                           .FirstOrDefault().Icon;
}
