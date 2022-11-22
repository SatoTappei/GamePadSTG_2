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
                // ���̒������L�����H���ۂ��ŕ��򂷂�
                bool isWide = areas[i, j].GetSectionFromNumKey(5).GetCharArray()[0, 0] == 'R';
                
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

                for (int k = 2; k <= 8; k += 2)
                {
                    if (areas[i, j].GetSectionFromNumKey(k).GetCharArray()[0, 0] == 'r')
                    {
                        if (isWide)
                        {
                            if (k == 2 || k == 8)
                            {
                                props[i, j].GetSectionFromNumKey(k).SetCharArray(array31);
                            }
                            else
                            {
                                props[i, j].GetSectionFromNumKey(k).SetCharArray(array13);
                            }
                        }
                        else
                        {
                            props[i, j].GetSectionFromNumKey(k).Fill('l');
                        }
                    }
                }
            }
        }
        // ���݂�Mass�N���X�ɂ�1�����������i�[����t�B�[���h���Ȃ��B
        // ���̃��\�b�h�͕�����̔z��𐶐����Ă��邽�߁AMass�N���X�ɂ�����������\���t�B�[���h���K�v�ɂȂ�
        // ���̃}�X�̌����t�B�[���h�����Ăǂ̏�����𐶐�����̂����߂�Ƃ�����i������
    }
}
