using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethod�p�^�[���𗘗p�����G�̐���
/// ���̃R���|�[�l���g��Product�N���X�𗘗p����Creator�N���X�ɑ�������
/// </summary>
public abstract class EnemyCreatorTest : MonoBehaviour
{
    /// <summary>FactoryMethod()�ɑ�������</summary>
    public abstract EnemyTest Create(string name, int hp);

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
