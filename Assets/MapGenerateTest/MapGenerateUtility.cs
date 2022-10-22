using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�}�b�v���1�}�X</summary>
public class Mass
{
    /// <summary>�}�b�v��ł̍��W�A�����ł̔ԍ��Ƃ͈Ⴄ�̂Œ���</summary>
    (int z, int x) _pos;
    char _char;

    public (int z, int x) Pos { get => _pos; }
    public char Char { get => _char; set => _char = value; }
}

/// <summary>�����\��������</summary>
public class Section
{
    readonly int _height;
    readonly int _widht;
    Mass[,] _masses;

    public Section(int height, int width)
    {
        _masses = new Mass[height, width];
    }

    public Mass[,] Masses { get => _masses; }
    public int Height { get => _height; }
    public int Widht { get => _widht; }

    /// <summary>�����̔ԍ���n���ƑΉ������Ή������}�X��Ԃ�</summary>
    public Mass GetMass(int z, int x) => _masses[z, x];

    /// <summary>���̋��𕶎���񎟌��z��ɂ��ĕԂ��B</summary>
    public char[,] GetStringArray()
    {
        char[,] array = new char[_height, _widht];
        for (int z = 0; z < _height; z++)
            for (int x = 0; x < _widht; x++)
            {
                array[z, x] = _masses[z, x].Char;
            }

        return array;
    }

    /// <summary>������񎟌��z������ɔ��f������</summary>
    public void SetStringArray(char[,] array)
    {
        if (array.GetLength(0) != _height || 
            array.GetLength(1) != _widht)
        {
            Debug.LogWarning("�n���ꂽ�z�񂪋��̑傫���ƈႢ�܂��B");
            return;
        }

        for (int z = 0; z < _height; z++)
            for (int x = 0; x < _widht; x++)
            {
                _masses[z, x].Char = array[z, x];
            }
    }
}

/// <summary>�}�b�v���\��������</summary>
public class Area
{
    readonly int _height = 7;
    readonly int _widht = 7;

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
        char[,] mapArray = new char[_height, _widht];
        foreach (Section sct in _sections)
        {
            for (int z = 0; z < sct.Height; z++)
                for (int x = 0; x < sct.Widht; x++)
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
