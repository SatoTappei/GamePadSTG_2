using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethod�p�^�[���𗘗p�����G�̐���
/// ���̃R���|�[�l���g��ConcreteProduct�N���X�ɑ�������
/// </summary>
public class GoblinTest : EnemyTest
{
    public GoblinTest(string name, int hp)
    {
        _name = name;
        _hp = hp;
    }

    public override void Attack()
    {
        Debug.Log($"{_name}�ւ̍U���I");
    }

    public override int GetHp() => _hp;

    public override string GetName() => _name;
}