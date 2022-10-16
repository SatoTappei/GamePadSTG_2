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
                areas[z, x]._roadStrs = SetBaseRoad(z, x);
            }

        return areas;
    }

    /// <summary>
    /// ��b�ƂȂ铹�H������
    /// </summary>
    string[,] SetBaseRoad(int zPos, int xPos)
    {
        // 1���̕�����^�̓񎟌��z��
        string[,] area = new string[AreaWide, AreaWide];

        for (int i = 0; i < AreaWide; i++)
            for (int j = 0; j < AreaWide; j++)
                area[i, j] = "g";

        // �^�񒆂͕K�����H�ɂȂ�
        area[3, 3] = "r";

        // ���̋�悪�[�����肷��
        bool leftEdge = xPos == 0 ? true : false;
        bool rightEdge = xPos == 5 - 1 ? true : false;
        bool topEdge = zPos == 0 ? true : false;
        bool bottomEdge = zPos == 5 - 1 ? true : false;

        if (!leftEdge)
        {
            area[3, 2] = "r";
            area[3, 1] = "r";
            area[3, 0] = "r";
        }
        if (!rightEdge)
        {
            area[3, 4] = "r";
            area[3, 5] = "r";
            area[3, 6] = "r";
        }
        if (!topEdge)
        {
            area[2, 3] = "r";
            area[1, 3] = "r";
            area[0, 3] = "r";
        }
        if (!bottomEdge)
        {
            area[6, 3] = "r";
            area[5, 3] = "r";
            area[4, 3] = "r";
        }

        return area;
    }
}
