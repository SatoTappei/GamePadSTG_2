using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 区域の生成を行う
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

    /// マップの1辺は奇数かつ負荷的に大丈夫な5で固定
    readonly int MapWidth = 5;
    readonly int MapHeight = 5;
    /// <summary>区域の一辺の幅、奇数でいい感じの値である7で固定</summary>
    readonly int AreaWide = 7;

    /// <summary>通常の道路</summary>
    readonly string _road = "r";
    /// <summary>幅の広い道路</summary>
    readonly string _wRoad = "R";
    /// <summary>何も無し</summary>
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
    /// 7*7の区域を生成する
    /// 道路のみ生成する
    /// </summary>
    public Area[,] Generate(/*Area[,] areas*/)
    {
        _areaMap = new Area[5, 5];

        for (int z = 0; z < 5; z++)
            for (int x = 0; x < 5; x++)
            {
                // デフォルトの文字で埋める
                string[,] areaStrs = Init();
                // 十字状の道路を生成する
                SetBaseRoad(areaStrs, z, x);
                _areaMap[z, x]._roadStrs = areaStrs;

                // 区域の座標を端か内側かでリストに振り分ける
                AddPosList(z, x);
            }

        // 端から2番目から幅-2のマス
        //端っこリストの座標のうち上下の端なら左右、左右の端なら上下をカットする


        ///* 外周の辺を一部カットする */
        //// カットする際の基準にする向きを正負で決める
        //bool isPositive = Random.Range(0, 2) == 1 ? true : false;

        ////// 辺をカットする際に基準となる区域の候補(上下左右の辺の真ん中に位置する区域)
        ////(int, int)[] edgeCenters = { (0, 2), (4, 2), (2, 0), (2, 4) };

        //// 見栄え的に1回もしくは2回カットするといい感じ
        //int count = Random.Range(1, 3);
        //for (int i = 0; i < count; i++)
        //{
        //    // カットする基準になる区域をリストからランダムに取得
        //    int r = Random.Range(0, edgeCenters.Length);
        //    int posZ = edgeCenters[r].Item1;
        //    int posX = edgeCenters[r].Item2;
        //    // 対になる辺では正の方向が反対になるので下と左の場合は反転させる
        //    // 下と左の場合はXもしくはZが最大なので、足すと一辺の長さを超える
        //    bool cutPositive = posZ + posX > 5 ? !isPositive : isPositive;

        //    // 上下の辺の場合
        //    if (posX == 2)
        //    {
        //        // 左右どちらかにカットする
        //        if (cutPositive)
        //            CutMapEdge(_areaMap, (posZ, posX), Direction.Right);
        //        else
        //            CutMapEdge(_areaMap, (posZ, posX), Direction.Left);
        //    }
        //    // 左右の辺の場合
        //    else
        //    {
        //        // 上下どちらかにカットする
        //        if (cutPositive)
        //            CutMapEdge(_areaMap, (posZ, posX), Direction.Up);
        //        else
        //            CutMapEdge(_areaMap, (posZ, posX), Direction.Down);
        //    }
        //}

        ///* 穴を開ける */
        //// 任意の箇所からランダムな方向を取得する
        //// その方向が3方向に繋がっているか調べる
        //// マップの端はカットするとおかしくなるので除く

        int cutAmount = Random.Range(1, 4);
        CutConnectRandom(cutAmount);


        //int cutCount = 3;
        //int current = 0;
        //for (int i = 0; i < 100; i++)
        //{
        //    int rz = Random.Range(1, 5 - 1);
        //    int rx = Random.Range(1, 5 - 1);
        //    // その区域が4方向に繋がっていない場合は処理をしない
        //    if (GetConnectCount(_areaMap[rz, rx]) < 4) continue;
        //    Direction dir = (Direction)Random.Range(0, 4);

        //    // その区域が4方向に繋がっている場合は
        //    // 上下にカットするときは左右を調べてカットされていないか調べる
        //    // 左右にカットするときは上下を調べてカットされていないか調べる
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

    /// <summary>正方形の区域(文字列の二次元配列)を作り、何もなしの文字で埋める</summary>
    string[,] Init()
    {
        string[,] area = new string[AreaWide, AreaWide];
        for (int i = 0; i < AreaWide; i++)
            for (int j = 0; j < AreaWide; j++)
                area[i, j] = _non;

        return area;
    }

    /// <summary>区域に十字型の道路を配置する</summary>
    string[,] SetBaseRoad(string[,] area, int zPos, int xPos)
    {
        int center = AreaWide / 2;
        // 真ん中は必ず道路になる
        area[center, center] = _road;
        // その区域が端か判定する
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

    /// <summary>座標を対応したリストに振り分ける</summary>
    void AddPosList(int z, int x)
    {
        // 上下左右の辺上の区域
        if (GetConnectCount(z, x) < 4)
        {
            // TODO:端から0番目と1番目を弾く処理が幅7マスにしか対応していない
            if (!(z == 2 || x == 2 || z == 6 || x == 6)) return;

            _edgePosList.Add((z, x));
        }
        // 内側の区域
        else
        {
            _innerPosList.Add((z, x));
        }
    }

    /// <summary>指定された回数だけ区域同士の接続をランダムに削除する</summary>
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

    /// <summary>任意の辺をカットする</summary>
    void CutMapEdge(int z, int x, Direction dir)
    {
        // 隣の区域は基準となった座標で置換した向きと対になる向きに置換する
        Direction revDir = GetReverseDir(dir);
        (int, int) pair = GetDirTuple(dir);
        int nextZ = z + pair.Item1;
        int nextX = x + pair.Item2;

        SetWordToDirection(_areaMap[z, x]._roadStrs, _non, dir);
        SetWordToDirection(_areaMap[nextZ, nextX]._roadStrs, _non, revDir);
    }

    /// <summary>区域の中央一マス先から指定した方向の文字を置き換える</summary>
    void SetWordToDirection(string[,] strs, string str, Direction dir)
    {
        int center = AreaWide / 2;
        (int, int) pair = GetDirTuple(dir);

        // 中心から端までの距離は"全体/2"で求めることが出来る
        for (int i = 1; i <= AreaWide / 2; i++)
        {
            int addZ = pair.Item1 * i;
            int addX = pair.Item2 * i;
            strs[center + addZ, center + addX] = str;
        }
    }

    /// <summary>区域の接続数を返す</summary>
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

    /// <summary>逆向きの方向を返す</summary>
    Direction GetReverseDir(Direction dir)
    {
        if      (dir == Direction.Up)    return Direction.Down;
        else if (dir == Direction.Down)  return Direction.Up;
        else if (dir == Direction.Right) return Direction.Left;
        else    /* Direction.Left */     return Direction.Right;
    }

    /// <summary>その方向のint型のペアを返す</summary>
    (int, int) GetDirTuple(Direction dir)
    {
        if      (dir == Direction.Up)    return (-1, 0);
        else if (dir == Direction.Down)  return (1, 0);
        else if (dir == Direction.Right) return (0, 1);
        else    /* Direction.Left */     return (0, -1);
    }

    /// <summary>デバッグ用:"端っこリスト"と"内側リスト"の中身を表示する</summary>
    void DebugLog()
    {
        Debug.Log("端っこリストの中身");
        _edgePosList.ForEach(t => Debug.Log(t));
        Debug.Log("内側リストの中身");
        _innerPosList.ForEach(t => Debug.Log(t));
    }
}
