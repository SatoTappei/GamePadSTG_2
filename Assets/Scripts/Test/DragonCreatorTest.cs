using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethod�p�^�[���𗘗p�����G�̐���
/// ���̃R���|�[�l���g��ConcreteCreator�N���X�ɑ�������(�{�X�S��)
/// </summary>
public class DragonCreatorTest : EnemyCreatorTest
{
    public override EnemyTest Create(string name, int hp)
    {
        name = name + "(�{�X)";
        EnemyTest enemy = new DragonTest(name, hp);
        return enemy;
    }
}
