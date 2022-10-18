using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ���̐������s��
/// </summary>
public class AreaGenerator : MonoBehaviour
{
    enum Direction
    {
        Up,
        Down,
        Right,
        Left,
    }

    /// �}�b�v��1�ӂ͊�����דI�ɑ��v��5�ŌŒ�
    readonly int MapWidth = 5;
    readonly int MapHeight = 5;
    /// <summary>���̈�ӂ̕��A��ł��������̒l�ł���7�ŌŒ�</summary>
    readonly int AreaWide = 7;

    /// <summary>�ʏ�̓��H</summary>
    readonly string _road = "r";
    /// <summary>���̍L�����H</summary>
    readonly string _wRoad = "R";
    /// <summary>��������</summary>
    readonly string _non = "n";

    Area[,] _areaMap;


    List<(int, int)> _edgeAreaList = new List<(int, int)>();
    List<(int, int)> _innerAreaList = new List<(int, int)>();

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
    public Area[,] Generate()
    {
        _areaMap = new Area[5, 5];

        for (int z = 0; z < 5; z++)
            for (int x = 0; x < 5; x++)
            {
                // �f�t�H���g�̕����Ŗ��߂�
                string[,] areaStrs = Init();
                // �\����̓��H�𐶐�����
                SetBaseRoad(areaStrs, z, x);
                _areaMap[z, x]._roadStrs = areaStrs;

                // ���̍��W��[���������Ń��X�g�ɐU�蕪����
                AddPosList(z, x);
            }

        // �����̋��̐ڑ����������폜����
        CutConnectRandom(3, 5);


        //int diffZ = 0;
        //int diffX = 0;
        //for (int i = 0; i < 100; i++)
        //{
        //    IEnumerable<(int, int)> pair = _edgeAreaList.OrderBy(t => System.Guid.NewGuid()).Take(2);
        //    (int, int) posA = pair.ElementAt(0);
        //    (int, int) posB = pair.ElementAt(1);

        //    diffZ = posA.Item1 - posB.Item2;
        //    diffX = posA.Item2 - posB.Item2;

        //    if(diff)
        //}

        (int, int) pointUC = (0, MapWidth / 2);
        (int, int) pointLC = (MapHeight / 2, 0);
        (int, int) pointBC = (MapHeight - 1, MapWidth / 2);
        (int, int) pointRC = (MapHeight / 2, MapWidth - 1);

        Hoge(pointUC, pointLC);
        //Hoge(pointLC, pointBC);
        //Hoge(pointBC, pointRC);
        //Hoge(pointRC, pointUC);

        // posA.z - posB.z > 0 up
        // posA.z - posB.z < 0 down
        // posA.z - posB.z == 0 �Ȃ�
        // posA.x - posB.x > 0 left
        // posA.x - posB.x < 0 right
        // posA.x - posB.x == 0 �Ȃ�



        return _areaMap;

        void Hoge((int, int) point1, (int, int) point2)
        {
            int diffZ = point1.Item1 - point2.Item1;
            int diffX = point1.Item2 - point2.Item2;

            List<Direction> list = new List<Direction>();
            if (diffZ > 0)
            {
                list.Add(Direction.Up);
            }
            else
            {
                list.Add(Direction.Down);
            }

            if (diffX > 0)
            {
                list.Add(Direction.Left);
            }
            else
            {
                list.Add(Direction.Right);
            }

            for (int i = 0; i < 100; i++)
            {
                // �㉺���������E�����ǂ��炩�ɐi��
                foreach (Direction dir in list.OrderBy(_ => System.Guid.NewGuid()))
                {
                    // ���݂̈ʒu�����̕����ɓ���L�΂��Ă��邩���ׂ�
                    bool b = CheckExistRoad(point1.Item1, point1.Item2, dir);
                    // �L�΂��Ă��Ȃ��ꍇ�͈Ⴄ������
                    if (!b) continue;
                    // �L�΂��Ă���ꍇ��
                    (int, int) pair = GetDirTuple(dir);
                    //Debug.Log("pair = " + pair.Item1);
                    //Debug.Log("point1 = " + )
                    (int, int) to = (point1.Item1 + pair.Item1, point1.Item2 + pair.Item2);
                    // ���̕����𑾂����H�ɂ���
                    //string[,] next = _areaMap[to.Item1, to.Item2]._roadStrs;
                    SetWordOnMapEdge(point1.Item1, point1.Item2, "R", dir);
                    // point1�����ݒn�ɍX�V����
                    point1.Item1 = to.Item1;
                    point1.Item2 = to.Item2;
                    break;
                }

                if (point1.Item1 == point2.Item1 && point1.Item2 == point2.Item2)
                    break;
            }
        }
    }

    /// <summary>�����`�̋��(������̓񎟌��z��)�����A�����Ȃ��̕����Ŗ��߂�</summary>
    string[,] Init()
    {
        string[,] area = new string[AreaWide, AreaWide];
        for (int i = 0; i < AreaWide; i++)
            for (int j = 0; j < AreaWide; j++)
                area[i, j] = _non;

        return area;
    }

    /// <summary>���ɏ\���^�̓��H��z�u����</summary>
    string[,] SetBaseRoad(string[,] area, int zPos, int xPos)
    {
        int center = AreaWide / 2;
        // �^�񒆂͕K�����H�ɂȂ�
        area[center, center] = _road;
        // ���̋�悪�[�����肷��
        if (xPos != 0)
            SetWordToDirection(area, _road, Direction.Left);
        if (xPos != MapWidth - 1)
            SetWordToDirection(area, _road, Direction.Right);
        if (zPos != 0)
            SetWordToDirection(area, _road, Direction.Up);
        if (zPos != MapHeight - 1)
            SetWordToDirection(area, _road, Direction.Down);

        return area;
    }

    /// <summary>����Ή��������X�g�ɐU�蕪����</summary>
    void AddPosList(int z, int x)
    {
        // �㉺���E�̕ӏ�̋��
        if (GetConnectCount(z, x) < 4)
        {
            // TODO:�[����0�Ԗڂ�1�Ԗڂ�e����������7�}�X�ɂ����Ή����Ă��Ȃ�
            if (!(z == 2 || x == 2 || z == 6 || x == 6)) return;

            _edgeAreaList.Add((z, x));
        }
        // �����̋��
        else
        {
            _innerAreaList.Add((z, x));
        }
    }

    /// <summary>��擯�m�̐ڑ��������_���ɍ폜����</summary>
    void CutConnectRandom(int min, int max)
    {
        // ���z�̍폜��
        int ideal = Random.Range(min, max + 1);
        int count = 0;

        // �S�Ă̋��̒����烉���_���ɐڑ����폜�ł��邩���ׂ�
        foreach ((int, int) pos in _innerAreaList.OrderBy(_ => System.Guid.NewGuid()))
        {
            int z = pos.Item1;
            int x = pos.Item2;

            List<Direction> list = new List<Direction>()
                { Direction.Up, Direction.Down, Direction.Right, Direction.Left };

            foreach (Direction dir in list.OrderBy(_ => System.Guid.NewGuid()))
            {
                // �ڑ�����1�ɂȂ��Ă��܂��ꍇ������������
                // �폜��̋�悪4�����ɐڑ�����Ă���ꍇ�̂ݍ폜����
                (int, int) pair = GetDirTuple(dir);
                if (GetConnectCount(z + pair.Item1, x + pair.Item2) == 4)
                {
                    SetWordOnMapEdge(z, x, _non, dir);
                    count++;
                    break;
                }
            }

            // �폜���𖞂����Ă����炱��ȏ�폜����̂���߂�
            if (count == ideal) break;
        }
    }

    /// <summary>�C�ӂ̕ӂ̕�����u��������</summary>
    void SetWordOnMapEdge(int z, int x, string str, Direction dir)
    {
        // �ׂ̋��͊�ƂȂ������W�Œu�����������Ƒ΂ɂȂ�����ɒu������
        Direction revDir = GetReverseDir(dir);
        (int, int) pair = GetDirTuple(dir);
        int nextZ = z + pair.Item1;
        int nextX = x + pair.Item2;

        SetWordToDirection(_areaMap[z, x]._roadStrs, str, dir);
        SetWordToDirection(_areaMap[nextZ, nextX]._roadStrs, str, revDir);
    }

    /// <summary>���̒�����}�X�悩��w�肵�������̕�����u��������</summary>
    void SetWordToDirection(string[,] strs, string str, Direction dir)
    {
        int center = AreaWide / 2;
        (int, int) pair = GetDirTuple(dir);

        // ���S����[�܂ł̋�����"�S��/2"�ŋ��߂邱�Ƃ��o����
        for (int i = 1; i <= AreaWide / 2; i++)
        {
            int addZ = pair.Item1 * i;
            int addX = pair.Item2 * i;
            strs[center + addZ, center + addX] = str;
        }
    }

    /// <summary>���̐ڑ�����Ԃ�</summary>
    int GetConnectCount(int z, int x)
    {
        int center = AreaWide / 2;
        string[,] area = _areaMap[z, x]._roadStrs;

        int count = 0;
        if (area[center - 1, center] == _road) count++;
        if (area[center + 1, center] == _road) count++;
        if (area[center, center - 1] == _road) count++;
        if (area[center, center + 1] == _road) count++;

        return count;
    }

    /// <summary>��悪���̕����ɓ���L�΂��Ă��邩��Ԃ�</summary>
    bool CheckExistRoad(int z, int x, Direction dir)
    {
        int center = AreaWide / 2;
        string[,] area = _areaMap[z, x]._roadStrs;
        (int, int) pair = GetDirTuple(dir);
        int addZ = pair.Item1;
        int addX = pair.Item2;

        return area[center + addZ, center + addX] != _non;
    }

    /// <summary>�t�����̕�����Ԃ�</summary>
    Direction GetReverseDir(Direction dir)
    {
        if      (dir == Direction.Up)    return Direction.Down;
        else if (dir == Direction.Down)  return Direction.Up;
        else if (dir == Direction.Right) return Direction.Left;
        else    /* Direction.Left */     return Direction.Right;
    }

    /// <summary>���̕�����int�^�̃y�A��Ԃ�</summary>
    (int, int) GetDirTuple(Direction dir)
    {
        if      (dir == Direction.Up)    return (-1, 0);
        else if (dir == Direction.Down)  return (1, 0);
        else if (dir == Direction.Right) return (0, 1);
        else    /* Direction.Left */     return (0, -1);
    }

    /// <summary>�f�o�b�O�p:"�[�������X�g"��"�������X�g"�̒��g��\������</summary>
    void DebugLogListContent()
    {
        Debug.Log("�[�������X�g�̒��g");
        _edgeAreaList.ForEach(t => Debug.Log(t));
        Debug.Log("�������X�g�̒��g");
        _innerAreaList.ForEach(t => Debug.Log(t));
    }

    /// <summary>�f�o�b�O�p:�S�Ă̋��̐ڑ�����\������</summary>
    void DebugLogAllConnect()
    {
        for (int j = 0; j < MapWidth; j++)
            for (int k = 0; k < MapHeight; k++)
            {
                Debug.Log(GetConnectCount(j, k));
            }
    }
}
