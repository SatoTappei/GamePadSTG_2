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


    List<(int, int)> _edgePosList = new List<(int, int)>();
    List<(int, int)> _innerPosList = new List<(int, int)>();

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
    public Area[,] Generate(/*Area[,] areas*/)
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

        // �[����2�Ԗڂ��畝-2�̃}�X
        //�[�������X�g�̍��W�̂����㉺�̒[�Ȃ獶�E�A���E�̒[�Ȃ�㉺���J�b�g����


        ///* �O���̕ӂ��ꕔ�J�b�g���� */
        //// �J�b�g����ۂ̊�ɂ�������𐳕��Ō��߂�
        //bool isPositive = Random.Range(0, 2) == 1 ? true : false;

        ////// �ӂ��J�b�g����ۂɊ�ƂȂ���̌��(�㉺���E�̕ӂ̐^�񒆂Ɉʒu������)
        ////(int, int)[] edgeCenters = { (0, 2), (4, 2), (2, 0), (2, 4) };

        //// ���h���I��1���������2��J�b�g����Ƃ�������
        //int count = Random.Range(1, 3);
        //for (int i = 0; i < count; i++)
        //{
        //    // �J�b�g�����ɂȂ�������X�g���烉���_���Ɏ擾
        //    int r = Random.Range(0, edgeCenters.Length);
        //    int posZ = edgeCenters[r].Item1;
        //    int posX = edgeCenters[r].Item2;
        //    // �΂ɂȂ�ӂł͐��̕��������΂ɂȂ�̂ŉ��ƍ��̏ꍇ�͔��]������
        //    // ���ƍ��̏ꍇ��X��������Z���ő�Ȃ̂ŁA�����ƈ�ӂ̒����𒴂���
        //    bool cutPositive = posZ + posX > 5 ? !isPositive : isPositive;

        //    // �㉺�̕ӂ̏ꍇ
        //    if (posX == 2)
        //    {
        //        // ���E�ǂ��炩�ɃJ�b�g����
        //        if (cutPositive)
        //            CutMapEdge(_areaMap, (posZ, posX), Direction.Right);
        //        else
        //            CutMapEdge(_areaMap, (posZ, posX), Direction.Left);
        //    }
        //    // ���E�̕ӂ̏ꍇ
        //    else
        //    {
        //        // �㉺�ǂ��炩�ɃJ�b�g����
        //        if (cutPositive)
        //            CutMapEdge(_areaMap, (posZ, posX), Direction.Up);
        //        else
        //            CutMapEdge(_areaMap, (posZ, posX), Direction.Down);
        //    }
        //}

        ///* �����J���� */
        //// �C�ӂ̉ӏ����烉���_���ȕ������擾����
        //// ���̕�����3�����Ɍq�����Ă��邩���ׂ�
        //// �}�b�v�̒[�̓J�b�g����Ƃ��������Ȃ�̂ŏ���

        int cutAmount = Random.Range(1, 4);
        CutConnectRandom(cutAmount);


        //int cutCount = 3;
        //int current = 0;
        //for (int i = 0; i < 100; i++)
        //{
        //    int rz = Random.Range(1, 5 - 1);
        //    int rx = Random.Range(1, 5 - 1);
        //    // ���̋�悪4�����Ɍq�����Ă��Ȃ��ꍇ�͏��������Ȃ�
        //    if (GetConnectCount(_areaMap[rz, rx]) < 4) continue;
        //    Direction dir = (Direction)Random.Range(0, 4);

        //    // ���̋�悪4�����Ɍq�����Ă���ꍇ��
        //    // �㉺�ɃJ�b�g����Ƃ��͍��E�𒲂ׂăJ�b�g����Ă��Ȃ������ׂ�
        //    // ���E�ɃJ�b�g����Ƃ��͏㉺�𒲂ׂăJ�b�g����Ă��Ȃ������ׂ�
        //    if (dir == Direction.Up && GetConnectCount(_areaMap[rz - 1, rx]) >= 4)
        //    {
        //        if (GetConnectCount(_areaMap[rz - 1, rx - 1]) < 4 &&
        //            GetConnectCount(_areaMap[rz - 1, rx + 1]) < 4) continue;
        //        CutMapEdge(_areaMap, (rz, rx), dir);
        //        current++;
        //    }
        //    else if (dir == Direction.Down && GetConnectCount(_areaMap[rz + 1, rx]) >= 4)
        //    {
        //        if (GetConnectCount(_areaMap[rz + 1, rx - 1]) < 4 &&
        //            GetConnectCount(_areaMap[rz + 1, rx + 1]) < 4) continue;
        //        CutMapEdge(_areaMap, (rz, rx), dir);
        //        current++;
        //    }
        //    else if (dir == Direction.Right && GetConnectCount(_areaMap[rz, rx + 1]) >= 4)
        //    {
        //        if (GetConnectCount(_areaMap[rz - 1, rx + 1]) < 4 &&
        //            GetConnectCount(_areaMap[rz + 1, rx + 1]) < 4) continue;
        //        CutMapEdge(_areaMap, (rz, rx), dir);
        //        current++;
        //    }
        //    else if (dir == Direction.Left && GetConnectCount(_areaMap[rz, rx - 1]) >= 4)
        //    {
        //        if (GetConnectCount(_areaMap[rz - 1, rx - 1]) < 4 &&
        //            GetConnectCount(_areaMap[rz + 1, rx - 1]) < 4) continue;
        //        CutMapEdge(_areaMap, (rz, rx), dir);
        //        current++;
        //    }

        //    if (current == cutCount) break;
        //}

        return _areaMap;
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

    /// <summary>���W��Ή��������X�g�ɐU�蕪����</summary>
    void AddPosList(int z, int x)
    {
        // �㉺���E�̕ӏ�̋��
        if (GetConnectCount(z, x) < 4)
        {
            // TODO:�[����0�Ԗڂ�1�Ԗڂ�e����������7�}�X�ɂ����Ή����Ă��Ȃ�
            if (!(z == 2 || x == 2 || z == 6 || x == 6)) return;

            _edgePosList.Add((z, x));
        }
        // �����̋��
        else
        {
            _innerPosList.Add((z, x));
        }
    }

    /// <summary>�w�肳�ꂽ�񐔂�����擯�m�̐ڑ��������_���ɍ폜����</summary>
    void CutConnectRandom(int count)
    {
        List<(int, int)> copyInnerPosList = new List<(int, int)>(_innerPosList);
        for (int i = 0; i < count; i++)
        {
            int r = Random.Range(0, copyInnerPosList.Count);
            int pz = copyInnerPosList[r].Item1;
            int px = copyInnerPosList[r].Item2;
            Direction dir = (Direction)Random.Range(0, 4);

            CutMapEdge(pz, px, dir);

            copyInnerPosList.RemoveAt(r);
        }
    }

    /// <summary>�C�ӂ̕ӂ��J�b�g����</summary>
    void CutMapEdge(int z, int x, Direction dir)
    {
        // �ׂ̋��͊�ƂȂ������W�Œu�����������Ƒ΂ɂȂ�����ɒu������
        Direction revDir = GetReverseDir(dir);
        (int, int) pair = GetDirTuple(dir);
        int nextZ = z + pair.Item1;
        int nextX = x + pair.Item2;

        SetWordToDirection(_areaMap[z, x]._roadStrs, _non, dir);
        SetWordToDirection(_areaMap[nextZ, nextX]._roadStrs, _non, revDir);
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
    void DebugLog()
    {
        Debug.Log("�[�������X�g�̒��g");
        _edgePosList.ForEach(t => Debug.Log(t));
        Debug.Log("�������X�g�̒��g");
        _innerPosList.ForEach(t => Debug.Log(t));
    }
}
