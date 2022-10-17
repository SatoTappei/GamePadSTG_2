using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        /* �O���̕ӂ��ꕔ�J�b�g���� */
        // �J�b�g����ۂ̊�ɂ�������𐳕��Ō��߂�
        bool isPositive = Random.Range(0, 2) == 1 ? true : false;
        // �ӂ��J�b�g����ۂɊ�ƂȂ���̌��(�㉺���E�̕ӂ̐^�񒆂Ɉʒu������)
        (int, int)[] edgeCenters = { (0, 2), (4, 2), (2, 0), (2, 4) };
        
        // ���h���I��1���������2��J�b�g����Ƃ�������
        int count = Random.Range(1, 3);
        for (int i = 0; i < count; i++)
        {
            // �J�b�g�����ɂȂ�������X�g���烉���_���Ɏ擾
            int r = Random.Range(0, edgeCenters.Length);
            int posZ = edgeCenters[r].Item1;
            int posX = edgeCenters[r].Item2;
            // �΂ɂȂ�ӂł͐��̕��������΂ɂȂ�̂ŉ��ƍ��̏ꍇ�͔��]������
            // ���ƍ��̏ꍇ��X��������Z���ő�Ȃ̂ŁA�����ƈ�ӂ̒����𒴂���
            bool cutPositive = posZ + posX > 5 ? !isPositive : isPositive;

            // �㉺�̕ӂ̏ꍇ
            if (posX == 2)
            {
                // ���E�ǂ��炩�ɃJ�b�g����
                if (cutPositive)
                    CutMapEdge(areas, (posZ, posX), Direction.Right);
                else
                    CutMapEdge(areas, (posZ, posX), Direction.Left);
            }
            // ���E�̕ӂ̏ꍇ
            else
            {
                // �㉺�ǂ��炩�ɃJ�b�g����
                if (cutPositive)
                    CutMapEdge(areas, (posZ, posX), Direction.Up);
                else
                    CutMapEdge(areas, (posZ, posX), Direction.Down);
            }
        }

        /* �����J���� */
        // �C�ӂ̉ӏ����烉���_���ȕ������擾����
        // ���̕�����3�����Ɍq�����Ă��邩���ׂ�
        // �}�b�v�̒[�̓J�b�g����Ƃ��������Ȃ�̂ŏ���
        int rz = Random.Range(1, 5 - 1);
        int rx = Random.Range(1, 5 - 1);
        Direction dir = (Direction)Random.Range(0, 4);

        if (dir == Direction.Up)
        {
            int connect = 0;
            if (areas[rz - 1, rx]._roadStrs[2, 3] == "r") connect++;
            if (areas[rz - 1, rx]._roadStrs[4, 3] == "r") connect++;
            if (areas[rz - 1, rx]._roadStrs[3, 2] == "r") connect++;
            if (areas[rz - 1, rx]._roadStrs[3, 4] == "r") connect++;

            if (connect >= 3)
                CutMapEdge(areas, (rz, rx), dir);
        }
        else if (dir == Direction.Down)
        {
            int connect = 0;
            if (areas[rz + 1, rx]._roadStrs[2, 3] == "r") connect++;
            if (areas[rz + 1, rx]._roadStrs[4, 3] == "r") connect++;
            if (areas[rz + 1, rx]._roadStrs[3, 2] == "r") connect++;
            if (areas[rz + 1, rx]._roadStrs[3, 4] == "r") connect++;

            if (connect >= 3)
                CutMapEdge(areas, (rz, rx), dir);
        }
        else if (dir == Direction.Right)
        {
            int connect = 0;
            if (areas[rz, rx + 1]._roadStrs[2, 3] == "r") connect++;
            if (areas[rz, rx + 1]._roadStrs[4, 3] == "r") connect++;
            if (areas[rz, rx + 1]._roadStrs[3, 2] == "r") connect++;
            if (areas[rz, rx + 1]._roadStrs[3, 4] == "r") connect++;

            if (connect >= 3)
                CutMapEdge(areas, (rz, rx), dir);
        }
        else if (dir == Direction.Left)
        {
            int connect = 0;
            if (areas[rz, rx - 1]._roadStrs[2, 3] == "r") connect++;
            if (areas[rz, rx - 1]._roadStrs[4, 3] == "r") connect++;
            if (areas[rz, rx - 1]._roadStrs[3, 2] == "r") connect++;
            if (areas[rz, rx - 1]._roadStrs[3, 4] == "r") connect++;

            if (connect >= 3)
                CutMapEdge(areas, (rz, rx), dir);
        }

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
