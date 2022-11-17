using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v��Ɍ����𐶐�����
/// </summary>
public class BuildingGenerator : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>�����𐶐�����</summary>
    public void Generate(Area[,] _areas)
    {
        // ���H�����Ɍ����𐶐�����
        for (int z = 0; z < Map.Height; z++)
            for (int x = 0; x < Map.Width; x++)
            {
                char[,] build33 =
                {
                    {'n', 'n', 'n'},
                    {'n', 'b', 'n'},
                    {'n', 'n', 'n'},
                };

                _areas[z, x].GetSectionFromNumKey(1).SetCharArray(build33);
                _areas[z, x].GetSectionFromNumKey(3).SetCharArray(build33);
                _areas[z, x].GetSectionFromNumKey(7).SetCharArray(build33);
                _areas[z, x].GetSectionFromNumKey(9).SetCharArray(build33);

                // 2 4 6 8 �𒲂ׂē��H�̂Ȃ����𑐂ނ�ɂ���
                for (int i = 2; i <= 8; i += 2)
                {
                    char lead = _areas[z, x].GetSectionFromNumKey(i).GetCharArray()[0, 0];

                    if (lead == 'n')
                        _areas[z, x].GetSectionFromNumKey(i).Fill('g');
                }
            }

        // �O���̓r����A�����݂Ȃǂ̈ړ��s�\�Ȃ��̂Ŗ��߂�
        for (int z = 0; z < Map.Height; z++)
        {
            char[,] build33 =
            {
                {'n', 'n', 'n'},
                {'n', 'h', 'n'},
                {'n', 'n', 'n'},
            };
            char[,] build13 =
            {
                {'n','h','n'}
            };

            _areas[z, 0].GetSectionFromNumKey(7).SetCharArray(build33);
            _areas[z, 0].GetSectionFromNumKey(4).SetCharArray(build13);
            _areas[z, 0].GetSectionFromNumKey(1).SetCharArray(build33);
            _areas[z, Map.Width - 1].GetSectionFromNumKey(9).SetCharArray(build33);
            _areas[z, Map.Width - 1].GetSectionFromNumKey(6).SetCharArray(build13);
            _areas[z, Map.Width - 1].GetSectionFromNumKey(3).SetCharArray(build33);
        }
        for (int x = 0; x < Map.Width; x++)
        {
            char[,] build33 =
            {
                {'n', 'n', 'n'},
                {'n', 'v', 'n'},
                {'n', 'n', 'n'},
            };
            char[,] build31 =
            {
                {'n'},
                {'v'},
                {'n'},
            };

            _areas[0, x].GetSectionFromNumKey(7).SetCharArray(build33);
            _areas[0, x].GetSectionFromNumKey(8).SetCharArray(build31);
            _areas[0, x].GetSectionFromNumKey(9).SetCharArray(build33);
            _areas[Map.Width - 1, x].GetSectionFromNumKey(1).SetCharArray(build33);
            _areas[Map.Width - 1, x].GetSectionFromNumKey(2).SetCharArray(build31);
            _areas[Map.Width - 1, x].GetSectionFromNumKey(3).SetCharArray(build33);
        }
    }
}
