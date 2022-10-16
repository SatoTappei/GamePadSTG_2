using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̐������s��
/// </summary>
public class AreaGenerator : MonoBehaviour
{
    // ���̈�ӁA��ł��������̒l�ł���7�ŌŒ�
    readonly int AreaWide = 7;

    enum Direction
    {
        Up,
        Down,
        Right,
        Left,
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 7*7�̋��𐶐�����
    /// ���H�̂ݐ�������
    /// </summary>
    public Area[,] Generate(Area[,] areas)
    {
        for (int z = 0; z < 5; z++)
            for (int x = 0; x < 5; x++)
            {
                // 1��敪�̕�����^�̓񎟌��z�������ăf�t�H���g�̕����Ŗ��߂�
                string[,] area = new string[AreaWide, AreaWide];
                for (int i = 0; i < AreaWide; i++)
                    for (int j = 0; j < AreaWide; j++)
                        area[i, j] = "g";

                areas[z, x]._roadStrs = area;
                // ���ɏ\���^�̓��H�𐶐�����
                areas[z, x]._roadStrs = SetBaseRoad(area, z, x);
            }

        // �O���̕ӂ��ꕔ�J�b�g����
        // �ӂ��J�b�g����ۂɊ�ƂȂ���̃��X�g(�㉺���E�̕ӂ̐^�񒆂Ɉʒu������)
        List<(int, int)> edgeCenterList = new List<(int, int)>()
        {
            (0, 2), (4, 2), (2, 0), (2, 4)
        };
        // ���h���I��2���������3��J�b�g����Ƃ�������
        int count = Random.Range(2, 4);
        for (int i = 0; i < count; i++)
        {
            // �J�b�g�����ɂȂ���W�����X�g���烉���_���Ɏ擾
            int r = Random.Range(0, edgeCenterList.Count);
            int posZ = edgeCenterList[r].Item1;
            int posX = edgeCenterList[r].Item2;
            edgeCenterList.RemoveAt(r);
            // �����𐳕��Ō��߂�
            bool isPositive = Random.Range(0, 2) == 1 ? true : false;

            // �㉺�̕ӂ̏ꍇ
            if (posX == 2)
            {
                // ���E�ǂ��炩�ɃJ�b�g����
                if (isPositive)
                    CutMapEdge(areas, (posZ, posX), Direction.Right);
                else
                    CutMapEdge(areas, (posZ, posX), Direction.Left);
            }
            // ���E�̕ӂ̏ꍇ
            else
            {
                // �㉺�ǂ��炩�ɃJ�b�g����
                if (isPositive)
                    CutMapEdge(areas, (posZ, posX), Direction.Up);
                else
                    CutMapEdge(areas, (posZ, posX), Direction.Down);
            }
        }

        // �����J����

        return areas;
    }

    /// <summary>��b�ƂȂ铹�H������</summary>
    string[,] SetBaseRoad(string[,] strs, int zPos, int xPos)
    {
        // �^�񒆂͕K�����H�ɂȂ�
        strs[3, 3] = "r";

        // ���̋�悪�[�����肷��
        bool leftEdge = xPos == 0 ? true : false;
        bool rightEdge = xPos == 5 - 1 ? true : false;
        bool topEdge = zPos == 0 ? true : false;
        bool bottomEdge = zPos == 5 - 1 ? true : false;

        if (!leftEdge)
            SetCharToDirection(strs, "r", Direction.Left);
        if (!rightEdge)
            SetCharToDirection(strs, "r", Direction.Right);
        if (!topEdge)
            SetCharToDirection(strs, "r", Direction.Up);
        if (!bottomEdge)
            SetCharToDirection(strs, "r", Direction.Down);

        return strs;
    }

    /// <summary>�C�ӂ̕ӂ��J�b�g����</summary>
    void CutMapEdge(Area[,] areas, (int, int) index, Direction dir)
    {
        int posZ = index.Item1;
        int posX = index.Item2;

        // �E�����������ėׂ̋��̍�����������
        if (dir == Direction.Right)
        {
            SetCharToDirection(areas[posZ, posX]._roadStrs, "g", Direction.Right);
            SetCharToDirection(areas[posZ, posX + 1]._roadStrs, "g", Direction.Left);
        }
        // �������������ėׂ̋��̉E����������
        else if (dir == Direction.Left)
        {
            SetCharToDirection(areas[posZ, posX]._roadStrs, "g", Direction.Left);
            SetCharToDirection(areas[posZ, posX - 1]._roadStrs, "g", Direction.Right);
        }
        // �������������ėׂ̋��̏����������
        else if (dir == Direction.Down)
        {
            SetCharToDirection(areas[posZ, posX]._roadStrs, "g", Direction.Down);
            SetCharToDirection(areas[posZ + 1, posX]._roadStrs, "g", Direction.Up);
        }
        // ������������ėׂ̋��̉�����������
        else if (dir == Direction.Up)
        {
            SetCharToDirection(areas[posZ, posX]._roadStrs, "g", Direction.Up);
            SetCharToDirection(areas[posZ - 1, posX]._roadStrs, "g", Direction.Down);
        }
    }

    /// <summary>���̒�����}�X�悩��w�肵�������̕�����u��������</summary>
    void SetCharToDirection(string[,] strs, string str, Direction dir)
    {
        int center = 3;
        (int, int) pairZX;
        if      (dir == Direction.Up)    pairZX = (-1, 0);
        else if (dir == Direction.Down)  pairZX = (1, 0);
        else if (dir == Direction.Right) pairZX = (0, 1);
        else    /* Direction.Left */     pairZX = (0, -1);

        // ���̒�������[�܂ł�3�}�X����
        for (int i = 1; i <= 3; i++)
        {
            int addZ = pairZX.Item1 * i;
            int addX = pairZX.Item2 * i;
            strs[center + addZ, center + addX] = str;
        }
    }
}
