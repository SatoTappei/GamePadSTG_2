using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1��1�̋��𐶐�����
/// </summary>
public class AreaGenerator : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>7*7�̋��𐶐�����</summary>
    public string[,] Generate()
    {
        string[,] area = new string[7, 7];

        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 7; j++)
                area[i, j] = "g";

        area[3, 3] = "r";

        // �^�񒆂͕K�����H�ɂȂ�
        // �㉺���E�Ƀ����_���ɓ���L�΂�

        return area;
    }
}
