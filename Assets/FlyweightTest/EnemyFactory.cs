using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    /// <summary>�G�̊e�p�����[�^�������ꂽSO�̃��X�g</summary>
    [SerializeField] List<ParamSO> _paramList = new List<ParamSO>();
    /// <summary>��������G</summary>
    [SerializeField] GameObject _enemyPrefab;
    /// <summary>�G�̐����ꏊ</summary>
    [SerializeField] Transform _enemyContainer;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�{�^�����N���b�N����ƓG�����������</summary>
    public void OnCreateEnemy(int idx)
    {
        // �Q�[���I�u�W�F�N�g�𐶐�����
        GameObject enemy = Instantiate(_enemyPrefab, _enemyContainer);
        // �R���|�[�l���g���擾����
        FlyWeightEnemyAI enemyAI = enemy.GetComponent<FlyWeightEnemyAI>();
        // ���̓G�̃p�����[�^�����X�g����I������
        enemyAI.Param = _paramList[idx];
        // ���̓G���_���^�[�Q�b�g���擾���Ă���
        enemyAI.Player = GameObject.FindGameObjectWithTag("Player").transform;

        // ���̌�ɐ������ꂽ�G��Start���\�b�h���Ă΂��
        // �����Ő��������G��Param��ݒ肵���̂Ŕ��f�����
    }
}
