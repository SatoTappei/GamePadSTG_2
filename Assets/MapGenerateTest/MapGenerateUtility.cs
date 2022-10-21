using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�}�b�v���1�}�X</summary>
public class Mass
{
    int _x;
    int _z;
    public char _char;
}

/// <summary>�����\��������</summary>
public class Section
{
    int _height;
    int _widht;
    Mass[,] _masses;

    public Section(int height, int width)
    {
        _masses = new Mass[height, width];
    }

    /// <summary>���̋��̕�����񎟌��z��ɂ��ĕԂ�</summary>
    public char[,] GetStringArray()
    {
        char[,] array = new char[_height, _widht];
        for (int z = 0; z < _height; z++)
            for (int x = 0; x < _widht; x++)
            {
                array[z, x] = _masses[z, x]._char;
            }

        return array;
    }
}

/// <summary>�}�b�v���\��������</summary>
public class Area
{
    /// <summary>�e���̓e���L�[�̔ԍ��ɑΉ����Ă���</summary>
    Section[] _sections = new Section[]
    {
        new Section(3,3),   // ����
        new Section(3,1),   // ��
        new Section(3,3),   // �E��
        new Section(1,3),   // ��
        new Section(1,1),   // �^��
        new Section(1,3),   // �E
        new Section(3,3),   // ����
        new Section(3,1),   // ��
        new Section(3,3),   // �E��
    };

    // �e�������̂�����1�̕�����^�̓񎟌��z��ɂ��ĕԂ�
    public char[,] GetStringArray()
    {
        // ���̃��\�b�h���Ăяo���ĕԂ����񎟌��z������Ƃ�
        // �}�b�v���I�u�W�F�N�g�Ƃ��Đ������邱�Ƃ𗯈ӂ���
        // TODO:��������
        char[,] mapArray = new char[7, 7];
        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 7; j++)
                mapArray[i, j] = 'r';
        // �����܂ł���������
        return mapArray;
    }
}

/// <summary>�}�b�v</summary>
public class Map
{
    public Area[,] Areas { get; set; }

    public Map(int height, int width)
    {
        Areas = new Area[height, width];

        for (int z = 0; z < height; z++)
            for (int x = 0; x < width; x++)
                Areas[z, x] = new Area();
    }
}

/// <summary>
/// �����̃X�N���v�g�ŋ��ʂ��Ďg������
/// </summary>
public class MapGenerateUtility
{
    // �}�b�v�̑傫���͊����Ȃ��Ƌ����Y��ɕ��ׂ邱�Ƃ��o���Ȃ�
    // �傫������ƕ��ׂ�������(����)�̂ōő�ł�5*5�ɂ��Ă���
    public static readonly int MapWidth = 5;
    public static readonly int MapHeight = 5;
    /// <summary>���̈�ӂ̕��A��ł��������̒l�ł���7�ŌŒ�</summary>
    public static readonly int AreaWide = 7;
}
