using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v��ɑ����𐶐�����
/// </summary>
public class PropGenerator
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>���ɏ�����𐶐�����</summary>
    public void Generate(Area[,] props, Area[,] areas)
    {
        props[3, 3].GetSectionFromNumKey(5).Fill('v');
        // ���݂�Mass�N���X�ɂ�1�����������i�[����t�B�[���h���Ȃ��B
        // ���̃��\�b�h�͕�����̔z��𐶐����Ă��邽�߁AMass�N���X�ɂ�����������\���t�B�[���h���K�v�ɂȂ�
        // ���̃}�X�̌����t�B�[���h�����Ăǂ̏�����𐶐�����̂����߂�Ƃ�����i������
    }
}
