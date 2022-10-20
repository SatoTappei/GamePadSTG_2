using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mass
{
    int x;
    int z;
}

public class Area
{
    enum Section
    {
        Up,
        Down,
        Right,
        Left,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft,
    }

    public string[,] _roadStrs;

    /// <summary>���p���ɋ��𕪂��Ď���</summary>
    Dictionary<Section, string[,]> sectionDic = new Dictionary<Section, string[,]>();
    /// <summary>���H�����̃}�X�̃��X�g</summary>
    List<Mass> byRoadList = new List<Mass>();

    // 7*7�̃}�X�ɓ��H�𐶐�����
    // �󂢂��}�X�Ɍ����𐶐�����

    // ����1�}�X�̓��H�̏ꍇ��3*3�̋󂫂�����
    // ����2�}�X�̓��H�̏ꍇ��2.5*2.5�̋󂫂�����
}

/// <summary>
/// �����̃X�N���v�g�ŋ��ʂ��Ďg������
/// </summary>
public class MapGenerateUtility
{
    /// �}�b�v��1�ӂ͊�����דI�ɑ��v��5�ŌŒ�
    public static readonly int MapWidth = 5;
    public static readonly int MapHeight = 5;
    /// <summary>���̈�ӂ̕��A��ł��������̒l�ł���7�ŌŒ�</summary>
    public static readonly int AreaWide = 7;
}
