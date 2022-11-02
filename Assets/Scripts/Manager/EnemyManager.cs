using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W��̓G���Ǘ�����
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>�X�e�[�W��ɑ��݂���G�̃��X�g</summary>
    List<EnemyAIBase> _enemyList = new List<EnemyAIBase>();

    void Start()
    {
        
    }

    void Update()
    {

    }

    // ���������G��T���ă��X�g�ɒǉ����Ȃ��͓̂��I�ɐ������ꂽ�G�ɑΉ������邽��
    /// <summary>�������ꂽ�G�������̃��\�b�h���Ă�Ŏ��g��o�^����</summary>
    public void AddEnemyList(EnemyAIBase ai) => _enemyList.Add(ai);

    /// <summary>�S�Ă̓G���N����</summary>
    public void WakeUpEnemyAll() => _enemyList.ForEach(ai => ai.WakeUp());
}
