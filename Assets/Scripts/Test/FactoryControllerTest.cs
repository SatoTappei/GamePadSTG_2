using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FactoryMethod�p�^�[���𗘗p�����G�̐���
/// ���̃R���|�[�l���g��ConcreteProduct�N���X�ɑ�������
/// </summary>
public class FactoryControllerTest : MonoBehaviour
{
    void Start()
    {
        EnemyCreatorTest goblinCreator = new GoblinCreatorTest();
        EnemyTest goblin = goblinCreator.Create("�S�u����", 100);
        goblin.Attack();

        EnemyCreatorTest dragonCreator = new DragonCreatorTest();
        EnemyTest dragon = dragonCreator.Create("�h���S��", 9999);
        dragon.Attack();
    }

    void Update()
    {
        
    }
}
