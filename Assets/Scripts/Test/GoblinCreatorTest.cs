using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethod�p�^�[���𗘗p�����G�̐���
/// ���̃R���|�[�l���g��ConcreteCreator�N���X�ɑ�������
/// </summary>
public class GoblinCreatorTest : EnemyCreatorTest
{
    public override EnemyTest Create(string name, int hp)
    {
        EnemyTest enemy = new GoblinTest(name, hp);
        return enemy;
    }
}
