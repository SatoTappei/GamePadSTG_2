using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v��Ɍ����𐶐�����
/// </summary>
public class BuildingGenerator
{
    /// <summary>�����𐶐�����</summary>
    public void Generate(Area[,] _areas)
    {
        // �d�����Ȃ������_���ȍ��W�̃��X�g
        List<(int z, int x)> posList = new List<(int, int)>();

        // ���H�����Ɍ����𐶐�����
        for (int z = 0; z < Map.Height; z++)
            for (int x = 0; x < Map.Width; x++)
            {
                // ���W�̃��X�g�ɒǉ����Ă���
                if (z < Map.Height - 1 && 0 < x && x < Map.Width)
                    posList.Add((z, x));

                // �ƌ��Ă�
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

                    if (lead == Mass.Default)
                        _areas[z, x].GetSectionFromNumKey(i).Fill('g');
                }
            }

        // �C�ӂ̐��̑傫�Ȍ����𐶐�����
        List<char> buildList = new List<char>(new char[] { 'P', 'P', 'F', 'F', 'T', 'T', 'M', 'M' });
        // �����̔z��̐�������������
        for(int i = 0; i < 8; i++)
        {
            int r1 = Random.Range(0, buildList.Count);
            char c = buildList[r1];
            int r2 = Random.Range(0, posList.Count);
            (int z, int x) = posList[r2];

            char[,] build =
            {
                {'n', 'n', 'n'},
                {'n', 'n', 'n'},
                { c , 'n', 'n'},
            };
            char[,] none =
            {
                {'n', 'n', 'n'},
                {'n', 'n', 'n'},
                {'n', 'n', 'n'},
            };

            _areas[z, x].GetSectionFromNumKey(1).SetCharArray(build);
            _areas[z + 1, x].GetSectionFromNumKey(7).SetCharArray(none);
            _areas[z + 1, x - 1].GetSectionFromNumKey(9).SetCharArray(none);
            _areas[z, x - 1].GetSectionFromNumKey(3).SetCharArray(none);

            buildList.RemoveAt(r1);
            posList.RemoveAt(r2);
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
