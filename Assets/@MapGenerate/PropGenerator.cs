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
        // �X����ݒu���I��������_�ő��������Ă�����̃��X�g
        List<(Section, bool)> grassList = new List<(Section, bool)>();

        for (int i = 0; i < Map.Height; i++)
        {
            for (int j = 0; j < Map.Width; j++)
            {
                // ���H�[�ɊX����ݒu����ۂɎg���z��
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
                bool isWide = areas[i, j].GetSectionFromNumKey(5).GetMass(0,0).Char == 'R';
                // �����̋�悩��㉺���E�̋��𒲂ׂĂ���
                for (int k = 2; k <= 8; k += 2)
                {
                    // ���̕����ɓ��H���L�тĂ��邩
                    char lead = areas[i, j].GetSectionFromNumKey(k).GetMass(0, 0).Char;
                    // �L�����H�Ȃ璆������1�}�X�ڂ͉����z�u���Ȃ�
                    if (isWide && lead == 'r')
                    {
                        if (k == 2 || k == 8)
                            props[i, j].GetSectionFromNumKey(k).SetCharArray(array31);
                        else
                            props[i, j].GetSectionFromNumKey(k).SetCharArray(array13);
                    }
                    // ���ʂ̓��H�Ȃ炻�̂܂ܓh��Ԃ�
                    else if (lead == 'r')
                    {
                        props[i, j].GetSectionFromNumKey(k).Fill('l');
                    }
                    // ����������
                    else if (lead == 'g')
                    {
                        // ���������Ă�����̃��X�g�ɒǉ�����
                        // ���̋�悪������̒������L�����H�ɂȂ��Ă��邩�̃t���O�ƃy�A�Œǉ�����
                        grassList.Add((props[i, j].GetSectionFromNumKey(k), isWide));
                    }
                }
            }
        }

        // ���������Ă�����ɕǂ�ݒu����
        foreach ((Section sec, bool isWide) pair in grassList)
        {
            // ���̋�悪��������̒������L�����H�ɂȂ��Ă���ꍇ�͕ǂ��͂ݏo���Ȃ��悤�ɂ���K�v������
            if (pair.isWide)
            {
                // ��������1�}�X�̈ʒu�̕ǂ�����
                switch (pair.sec.Number)
                {
                    case 2:
                        pair.sec.SetCharArray(new char[3, 1]
                        {
                            {'n'},
                            {'w'},
                            {'w'},
                        });
                        break;
                    case 4:
                        pair.sec.SetCharArray(new char[1, 3]
                        {
                            { 'i', 'i', 'n' },
                        });
                        break;
                    case 6:
                        pair.sec.SetCharArray(new char[1, 3]
                        {
                            { 'n', 'i', 'i' },
                        });
                        break;
                    case 8:
                        pair.sec.SetCharArray(new char[3, 1]
                        {
                            {'w'},
                            {'w'},
                            {'n'},
                        });
                        break;
                }
            }
            else
            {
                // ���������H�̏ꍇ�͂��̂܂ܓh��Ԃ�
                int num = pair.sec.Number;
                pair.sec.Fill(num == 2 || num == 8 ? 'w' : 'i');
            }
        }
    }
}
