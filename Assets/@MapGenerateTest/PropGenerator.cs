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
        for (int i = 0; i < Map.Height; i++)
        {
            for (int j = 0; j < Map.Width; j++)
            {
                // ���H�[�ɊX����ݒu����
                
                char[,] array31 =
                {
                    {'n'},
                    {'l'},
                    {'l'},
                };
                char[,] array13 =
                {
                    {'n', 'l', 'l'},
                };

                // ���̒������L�����H���ǂ���
                bool isWide = areas[i, j].GetSectionFromNumKey(5).GetCharArray()[0, 0] == 'R';
                // �����̋�悩��㉺���E�̋��𒲂ׂĂ���
                for (int k = 2; k <= 8; k += 2)
                {
                    // ���̕����ɓ��H���L�тĂ��邩
                    bool isRoad = areas[i, j].GetSectionFromNumKey(k).GetCharArray()[0, 0] == 'r';
                    // �L�����H�Ȃ璆������1�}�X�ڂ͉����z�u���Ȃ�
                    if (isRoad && isWide)
                    {
                        if (k == 2 || k == 8)
                            props[i, j].GetSectionFromNumKey(k).SetCharArray(array31);
                        else
                            props[i, j].GetSectionFromNumKey(k).SetCharArray(array13);
                    }
                    // ���ʂ̓��H�Ȃ炻�̂܂ܓh��Ԃ�
                    else if (isRoad)
                    {
                        props[i, j].GetSectionFromNumKey(k).Fill('l');
                    }
                    // �����������H����Ȃ��ꍇ�͉����ݒu���Ȃ�
                }
            }
        }
        // ���݂�Mass�N���X�ɂ�1�����������i�[����t�B�[���h���Ȃ��B
        // ���̃��\�b�h�͕�����̔z��𐶐����Ă��邽�߁AMass�N���X�ɂ�����������\���t�B�[���h���K�v�ɂȂ�
        // ���̃}�X�̌����t�B�[���h�����Ăǂ̏�����𐶐�����̂����߂�Ƃ�����i������
    }
}
