using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v���1�}�X
/// </summary>
public class Mass
{
    /// <summary>����ł̍��W�A�����ł̔ԍ��Ƃ͈Ⴄ�̂Œ���</summary>
    public readonly (int z, int x) Pos;
    char _char;

    public Mass(int z, int x)
    {
        Pos.z = z;
        Pos.x = x;
    }

    public char Char { get => _char; set => _char = value; }
}

/// <summary>
/// �����\��������
/// </summary>
public class Section
{
    // ���̍���ƉE����ۑ����� => �e�}�X�Ɋ��蓖�Ă邽��
    public readonly int Height;
    public readonly int Width;
    readonly (int z, int x) _upperLeft;
    readonly (int z, int x) _bottomRight;
    Mass[,] _masses;

    public Section(int height, int width, (int z, int x) upperLeft, (int z, int x) bottomRight)
    {
        _masses = new Mass[height, width];
        Height = height;
        Width = width;
        _upperLeft = upperLeft;
        _bottomRight = bottomRight;

        for (int z = 0, areaZ = _upperLeft.z; z < height; z++, areaZ++)
            for (int x = 0, areaX = _upperLeft.x; x < width; x++, areaX++)
            {
                _masses[z, x] = new Mass(areaZ, areaX);
            }
    }

    public Mass[,] Masses { get => _masses; }

    /// <summary>�����̔ԍ���n���ƑΉ������Ή������}�X��Ԃ�</summary>
    public Mass GetMass(int z, int x) => _masses[z, x];

    /// <summary>���̋��𕶎��̓񎟌��z��ɂ��ĕԂ��B</summary>
    public char[,] GetCharArray()
    {
        char[,] array = new char[Height, Width];
        for (int z = 0; z < Height; z++)
            for (int x = 0; x < Width; x++)
            {
                array[z, x] = _masses[z, x].Char;
            }

        return array;
    }

    /// <summary>����n���ꂽ�����Ŗ��߂�</summary>
    public void Fill(char c)
    {
        for (int z = 0; z < Height; z++)
            for (int x = 0; x < Width; x++)
            {
                _masses[z, x].Char = c;
            }
    }

    /// <summary>�����̓񎟌��z������ɔ��f������</summary>
    public void SetCharArray(char[,] array)
    {
        if (array.GetLength(0) != Height || array.GetLength(1) != Width)
        {
            Debug.LogWarning("�n���ꂽ�z�񂪋��̑傫���ƈႢ�܂��B");
            return;
        }

        for (int z = 0; z < Height; z++)
            for (int x = 0; x < Width; x++)
            {
                _masses[z, x].Char = array[z, x];
            }
    }
}

/// <summary>
/// �}�b�v���\��������
/// </summary>
public class Area
{
    public static int Wide = 7;

    /// <summary>�e���̓e���L�[�̔ԍ��ɑΉ����Ă���</summary>
    Section[] _sections;

    public Area()
    {
        _sections = new Section[]
        {
            new Section(3,3,(4,0),(6,2)),   // ����
            new Section(3,1,(4,3),(6,3)),   // ��
            new Section(3,3,(4,4),(6,6)),   // �E��
            new Section(1,3,(3,0),(3,2)),   // ��
            new Section(1,1,(3,3),(3,3)),   // �^��
            new Section(1,3,(3,4),(3,6)),   // �E
            new Section(3,3,(0,0),(2,2)),   // ����
            new Section(3,1,(0,3),(2,3)),   // ��
            new Section(3,3,(0,4),(0,6)),   // �E��
         };
    }

    /// <summary>�e���L�[�ɑΉ���������Ԃ�</summary>
    public Section GetSectionFromNumKey(int numKey) => _sections[numKey - 1];

    /// <summary>�e�������̂�����1�̕����^�̓񎟌��z��ɂ��ĕԂ�</summary>
    public char[,] GetCharArray()
    {
        char[,] mapArray = new char[Wide, Wide];
        foreach (Section sct in _sections)
        {
            for (int z = 0; z < sct.Height; z++)
                for (int x = 0; x < sct.Width; x++)
                {
                    // �����̊e�}�X���}�b�v�̕�����񎟌��z��ɔ��f����
                    Mass mass = sct.GetMass(z, x);
                    mapArray[mass.Pos.z, mass.Pos.x] = mass.Char;
                }
        }

        return mapArray;
    }
}

/// <summary>
/// �����̋�悩��Ȃ�}�b�v
/// �����͋�悲�Ƃɍs���̂Ń}�b�v�N���X�ɂ͕�����񎟌��z��͎����Ȃ�
/// </summary>
public class Map
{
    public static readonly int Height = 5;
    public static readonly int Width = 5;

    public Area[,] Areas { get; set; }

    public Map(int height, int width)
    {
        Areas = new Area[height, width];

        for (int z = 0; z < height; z++)
            for (int x = 0; x < width; x++)
                Areas[z, x] = new Area();
    }
}
