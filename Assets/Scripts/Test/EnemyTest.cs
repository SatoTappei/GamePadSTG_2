using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethod�p�^�[���𗘗p�����G�̐���
/// ���̃R���|�[�l���g��Product�N���X�ɑ�������
/// </summary>
public abstract class EnemyTest : MonoBehaviour
{
    protected string _name;
    protected int _hp;

    public abstract string GetName();
    public abstract int GetHp();
    public abstract void Attack();
}
