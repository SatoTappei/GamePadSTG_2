using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �}�b�v��ɓG��z�u����
/// </summary>
public class EnemyGenerator
{
    readonly int MaxEnemy = 10;

    /// <summary>�G�𐶐�����</summary>
    public void Generate(Area[,] enemies, Area[,] areas)
    {
        // ������܂œG�𐶐�����
        // �G�����������̂�5,2,4,6,8
        // ��Ԃ͂ł������H�ɂ̂ݐ��������
        // ���m�͂ǂ��ɂł����������

        // ���̍L�����H�Ƃ����łȂ����H�ɕ�����
        List<Section> wideRoadList = new List<Section>();
        List<Section> roadList = new List<Section>();

        for (int i = 0; i < Map.Height; i++)
        {
            for (int j = 0; j < Map.Width; j++)
            {
                if(areas[i, j].GetSectionFromNumKey(5).GetMass(0, 0).Char == 'R')
                {
                    wideRoadList.Add(enemies[i, j].GetSectionFromNumKey(5));
                }
                else
                {
                    roadList.Add(enemies[i, j].GetSectionFromNumKey(5));
                }

                for (int k = 2; k <= 8; k += 2)
                {
                    if (areas[i, j].GetSectionFromNumKey(k).GetMass(0, 0).Char == 'R')
                    {
                        wideRoadList.Add(enemies[i, j].GetSectionFromNumKey(k));
                    }
                    else if(areas[i, j].GetSectionFromNumKey(k).GetMass(0, 0).Char == 'r')
                    {
                        roadList.Add(enemies[i, j].GetSectionFromNumKey(k));
                    }
                }

            }
        }

        Debug.Log("�������H" + wideRoadList.Count);
        Debug.Log("���H" + roadList.Count);

        // �������H�̃��X�g�̒����烉���_���ȋ��A�������_���ȃ}�X�ɐ�������
        // 1�̐����������ɂ͐������Ȃ�
        // ��������G�̍ő吔�͒萔�Ŏw�肵�Ă���B
        // ��Ԃƕ��m�͓����ʐ������� S H
        // �Q�[���N���A�̂��߂Ƀ^�[�Q�b�g��1�̐������� T

        wideRoadList = wideRoadList.OrderBy(_ => System.Guid.NewGuid()).ToList();
        roadList = roadList.OrderBy(_ => System.Guid.NewGuid()).ToList();

        // ��Ԃ𐶐�����
        for (int i = 0; i < MaxEnemy / 2; i++)
        {
            if (wideRoadList[i].Number == 5)
            {
                wideRoadList[i].Fill('S');
            }
            else if(wideRoadList[i].Number == 2 || wideRoadList[i].Number == 8)
            {
                char[,] array =
                {
                    {'n'},
                    {'S'},
                    {'n'},
                };

                wideRoadList[i].SetCharArray(array);
            }
            else if(wideRoadList[i].Number == 4 || wideRoadList[i].Number == 6)
            {
                char[,] array =
                {
                    {'n', 'S', 'n'},
                };

                wideRoadList[i].SetCharArray(array);
            }


        }

        //// ���m�𐶐�����
        //for (int i = 0; i < MaxEnemy / 2; i++)
        //{
        //    if (wideRoadList[i].Number == 5)
        //    {
        //        wideRoadList[i].Fill('H');
        //    }
        //    else if (wideRoadList[i].Number == 2 || wideRoadList[i].Number == 8)
        //    {
        //        char[,] array =
        //        {
        //            {'n'},
        //            {'H'},
        //            {'n'},
        //        };

        //        wideRoadList[i].SetCharArray(array);
        //    }
        //    else if (wideRoadList[i].Number == 4 || wideRoadList[i].Number == 6)
        //    {
        //        char[,] array =
        //        {
        //            {'n', 'H', 'n'},
        //        };

        //        wideRoadList[i].SetCharArray(array);
        //    }
        //}
    }
}
