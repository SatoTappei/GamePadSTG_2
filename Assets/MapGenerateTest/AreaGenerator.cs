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
    readonly int MapWidth = MapGenerateUtility.MapWidth;
    readonly int MapHeight = MapGenerateUtility.MapHeight;
    /// <summary>���̈�ӂ̕��A��ł��������̒l�ł���7�ŌŒ�</summary>
    readonly int AreaWide = MapGenerateUtility.AreaWide;

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
        //_areaMap = new Area[5, 5];

        //for (int z = 0; z < 5; z++)
        //    for (int x = 0; x < 5; x++)
        //    {
        //        _areaMap[z, x] = new Area();
        //        // �f�t�H���g�̕����Ŗ��߂�
        //        string[,] areaStrs = Init();
        //        // �\����̓��H�𐶐�����
        //        SetBaseRoad(areaStrs, z, x);
        //        _areaMap[z, x]._roadStrs = areaStrs;

        //        // ���̍��W��[���������Ń��X�g�ɐU�蕪����
        //        AddPosList(z, x);
        //    }

        //// �����̋��̐ڑ����������폜����
        //CutConnectRandom(3, 5);

        //// �e�ӂ̒��S�̍��W
        //(int, int) TopCenter = (0, MapWidth / 2);
        //(int, int) LeftCenter = (MapHeight / 2, 0);
        //(int, int) RightCenter = (MapHeight - 1, MapWidth / 2);
        //(int, int) BottomCenter = (MapHeight / 2, MapWidth - 1);
        //// �O������ɑ������H������
        //SetWideRoadOnGround(TopCenter, LeftCenter);
        //SetWideRoadOnGround(LeftCenter, RightCenter);
        //SetWideRoadOnGround(RightCenter, BottomCenter);
        //SetWideRoadOnGround(BottomCenter, TopCenter);

        return null; // <- ����
    }

    /// <summary>�����`�̋��(������̓񎟌��z��)�����A�����Ȃ��̕����Ŗ��߂�</summary>
    //string[,] Init()
    //{
    //    string[,] area = new string[AreaWide, AreaWide];
    //    for (int i = 0; i < AreaWide; i++)
    //        for (int j = 0; j < AreaWide; j++)
    //            area[i, j] = _non;

    //    return area;
    //}

    ///// <summary>���ɏ\���^�̓��H��z�u����</summary>
    //string[,] SetBaseRoad(string[,] area, int zPos, int xPos)
    //{
    //    int center = AreaWide / 2;
    //    // �^�񒆂͕K�����H�ɂȂ�
    //    area[center, center] = _road;
    //    // ���̋�悪�[�����肷��
    //    if (xPos != 0)
    //        SetWordToDirection(area, _road, Direction.Left);
    //    if (xPos != MapWidth - 1)
    //        SetWordToDirection(area, _road, Direction.Right);
    //    if (zPos != 0)
    //        SetWordToDirection(area, _road, Direction.Up);
    //    if (zPos != MapHeight - 1)
    //        SetWordToDirection(area, _road, Direction.Down);

    //    return area;
    //}

    ///// <summary>����Ή��������X�g�ɐU�蕪����</summary>
    //void AddPosList(int z, int x)
    //{
    //    // �㉺���E�̕ӏ�̋��
    //    if (GetConnectCount(z, x) < 4)
    //    {
    //        // TODO:�[����0�Ԗڂ�1�Ԗڂ�e����������7�}�X�ɂ����Ή����Ă��Ȃ�
    //        if (!(z == 2 || x == 2 || z == 6 || x == 6)) return;

    //        _edgeAreaList.Add((z, x));
    //    }
    //    // �����̋��
    //    else
    //    {
    //        _innerAreaList.Add((z, x));
    //    }
    //}

    ///// <summary>��擯�m�̐ڑ��������_���ɍ폜����</summary>
    //void CutConnectRandom(int min, int max)
    //{
    //    // ���z�̍폜��
    //    int ideal = Random.Range(min, max + 1);
    //    int count = 0;

    //    // �S�Ă̋��̒����烉���_���ɐڑ����폜�ł��邩���ׂ�
    //    foreach ((int, int) pos in _innerAreaList.OrderBy(_ => System.Guid.NewGuid()))
    //    {
    //        int z = pos.Item1;
    //        int x = pos.Item2;

    //        List<Direction> list = new List<Direction>()
    //            { Direction.Up, Direction.Down, Direction.Right, Direction.Left };

    //        foreach (Direction dir in list.OrderBy(_ => System.Guid.NewGuid()))
    //        {
    //            // �ڑ�����1�ɂȂ��Ă��܂��ꍇ������������
    //            // �폜��̋�悪4�����ɐڑ�����Ă���ꍇ�̂ݍ폜����
    //            (int, int) pair = GetDirTuple(dir);
    //            if (GetConnectCount(z + pair.Item1, x + pair.Item2) == 4)
    //            {
    //                SetWordOnMapEdge(z, x, _non, dir);
    //                count++;
    //                break;
    //            }
    //        }

    //        // �폜���𖞂����Ă����炱��ȏ�폜����̂���߂�
    //        if (count == ideal) break;
    //    }
    //}

    ///// <summary>�O���ɑ������H�𐶐�����</summary>
    //void SetWideRoadOnGround((int z, int x) current, (int z, int x) goal)
    //{
    //    // �X�^�[�g����S�[���܂ł̋������v�Z����
    //    int diffZ = current.z - goal.z;
    //    int diffX = current.x - goal.x;
    //    int diff = Mathf.Abs(diffZ) + Mathf.Abs(diffX);

    //    if      (current.z == 0)             Process(Direction.Left, Direction.Down, isVertEdge: true);
    //    else if (current.z == MapHeight - 1) Process(Direction.Right, Direction.Up, isVertEdge: true);
    //    else if (current.x == 0)             Process(Direction.Down, Direction.Right, isVertEdge: false);
    //    else if (current.x == MapWidth - 1)  Process(Direction.Up, Direction.Left, isVertEdge: false);

    //    // ���������Ⴄ�̂ŏ�����؂�o����
    //    void Process(Direction edgeDir, Direction innerDir, bool isVertEdge)
    //    {
    //        // �ŏ���1��͕K���ӂɉ����Ĉړ�����
    //        SetWideRoad(edgeDir, out (int z, int x) firstStep);
    //        // ��ƂȂ���W���X�V����
    //        current.z = firstStep.z;
    //        current.x = firstStep.x;

    //        // 2��ڂ���Ō�1�O�܂ł̓����_���ɂǂ��炩�ɐi��
    //        for (int i = 0; i < diff - 2; i++)
    //        {
    //            List<Direction> list = new List<Direction>() { edgeDir,innerDir };
    //            // �����L�тĂ�������������_���ɕԂ�
    //            Direction dir = list.OrderBy(_ => System.Guid.NewGuid())
    //                              .Where(d => CheckExistRoad(current.z, current.x, d))
    //                              .FirstOrDefault();
    //            // ��ƂȂ���W���X�V����
    //            SetWideRoad(dir, out (int z, int x) next);
    //            current.z = next.z;
    //            current.x = next.x;
    //        }

    //        // �Ōォ��ЂƂO�̏�Ԃŕӏ�ɂ��邩�ǂ����Ŏ��Ɍ�����������ς���
    //        // �c�����̕ӂɂ��邩�`�F�b�N����ꍇ��x���W�A�����ł͂Ȃ��ꍇ��z���W���`�F�b�N����
    //        int checkCurrent = isVertEdge ? current.x : current.z;
    //        int checkGoal = isVertEdge ? goal.x : goal.z;

    //        if (checkCurrent - checkGoal != 0)
    //            SetWideRoad(edgeDir, out (int, int) _);
    //        else
    //            SetWideRoad(innerDir, out (int, int) _);
    //    }

    //    // �C�ӂ̕����ɑ������H�𐶐�����Aout�ɂ͎��̊�ƂȂ���W������
    //    void SetWideRoad(Direction dir, out (int, int) next)
    //    {
    //        (int z, int x) vec = GetDirTuple(dir);
    //        (int z, int x) to = (current.z + vec.z, current.x + vec.x);
    //        int center = AreaWide / 2;
    //        _areaMap[current.z, current.x]._roadStrs[center, center] = _wRoad;
    //        SetWordOnMapEdge(current.z, current.x, _wRoad, dir);

    //        next = to;
    //    }
    //}

    ///// <summary>�C�ӂ̕ӂ̕�����u��������</summary>
    //void SetWordOnMapEdge(int z, int x, string str, Direction dir)
    //{
    //    // �ׂ̋��͊�ƂȂ������W�Œu�����������Ƒ΂ɂȂ�����ɒu������
    //    Direction revDir = GetReverseDir(dir);
    //    (int, int) pair = GetDirTuple(dir);
    //    int nextZ = z + pair.Item1;
    //    int nextX = x + pair.Item2;

    //    SetWordToDirection(_areaMap[z, x]._roadStrs, str, dir);
    //    SetWordToDirection(_areaMap[nextZ, nextX]._roadStrs, str, revDir);
    //}

    ///// <summary>���̒�����}�X�悩��w�肵�������̕�����u��������</summary>
    //void SetWordToDirection(string[,] strs, string str, Direction dir)
    //{
    //    int center = AreaWide / 2;
    //    (int, int) pair = GetDirTuple(dir);

    //    // ���S����[�܂ł̋�����"�S��/2"�ŋ��߂邱�Ƃ��o����
    //    for (int i = 1; i <= AreaWide / 2; i++)
    //    {
    //        int addZ = pair.Item1 * i;
    //        int addX = pair.Item2 * i;
    //        strs[center + addZ, center + addX] = str;
    //    }
    //}

    ///// <summary>���̐ڑ�����Ԃ�</summary>
    //int GetConnectCount(int z, int x)
    //{
    //    int center = AreaWide / 2;
    //    string[,] area = _areaMap[z, x]._roadStrs;

    //    int count = 0;
    //    if (area[center - 1, center] == _road) count++;
    //    if (area[center + 1, center] == _road) count++;
    //    if (area[center, center - 1] == _road) count++;
    //    if (area[center, center + 1] == _road) count++;

    //    return count;
    //}

    ///// <summary>��悪���̕����ɓ���L�΂��Ă��邩��Ԃ�</summary>
    //bool CheckExistRoad(int z, int x, Direction dir)
    //{
    //    int center = AreaWide / 2;
    //    string[,] area = _areaMap[z, x]._roadStrs;
    //    (int, int) pair = GetDirTuple(dir);
    //    int addZ = pair.Item1;
    //    int addX = pair.Item2;

    //    return area[center + addZ, center + addX] != _non;
    //}

    ///// <summary>�t�����̕�����Ԃ�</summary>
    //Direction GetReverseDir(Direction dir)
    //{
    //    if      (dir == Direction.Up)    return Direction.Down;
    //    else if (dir == Direction.Down)  return Direction.Up;
    //    else if (dir == Direction.Right) return Direction.Left;
    //    else    /* Direction.Left */     return Direction.Right;
    //}

    ///// <summary>���̕�����int�^�̃y�A��Ԃ�</summary>
    //(int, int) GetDirTuple(Direction dir)
    //{
    //    if      (dir == Direction.Up)    return (-1, 0);
    //    else if (dir == Direction.Down)  return (1, 0);
    //    else if (dir == Direction.Right) return (0, 1);
    //    else    /* Direction.Left */     return (0, -1);
    //}

    ///// <summary>�f�o�b�O�p:"�[�������X�g"��"�������X�g"�̒��g��\������</summary>
    //void DebugLogListContent()
    //{
    //    Debug.Log("�[�������X�g�̒��g");
    //    _edgeAreaList.ForEach(t => Debug.Log(t));
    //    Debug.Log("�������X�g�̒��g");
    //    _innerAreaList.ForEach(t => Debug.Log(t));
    //}

    ///// <summary>�f�o�b�O�p:�S�Ă̋��̐ڑ�����\������</summary>
    //void DebugLogAllConnect()
    //{
    //    for (int j = 0; j < MapWidth; j++)
    //        for (int k = 0; k < MapHeight; k++)
    //        {
    //            Debug.Log(GetConnectCount(j, k));
    //        }
    //}
}
