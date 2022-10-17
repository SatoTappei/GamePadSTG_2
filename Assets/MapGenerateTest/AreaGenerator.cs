using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 区域の生成を行う
/// </summary>
public class AreaGenerator : MonoBehaviour
{
    // 区域の一辺、奇数でいい感じの値である7で固定
    readonly int AreaWide = 7;

    enum Direction
    {
        Up,
        Down,
        Right,
        Left,
    }

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
    public Area[,] Generate(Area[,] areas)
    {
        for (int z = 0; z < 5; z++)
            for (int x = 0; x < 5; x++)
            {
                // 1区域分の文字列型の二次元配列を作ってデフォルトの文字で埋める
                string[,] area = new string[AreaWide, AreaWide];
                for (int i = 0; i < AreaWide; i++)
                    for (int j = 0; j < AreaWide; j++)
                        area[i, j] = "g";

                areas[z, x]._roadStrs = area;
                // 区域に十字型の道路を生成する
                areas[z, x]._roadStrs = SetBaseRoad(area, z, x);
            }

        /* 外周の辺を一部カットする */
        // カットする際の基準にする向きを正負で決める
        bool isPositive = Random.Range(0, 2) == 1 ? true : false;
        // 辺をカットする際に基準となる区域の候補(上下左右の辺の真ん中に位置する区域)
        (int, int)[] edgeCenters = { (0, 2), (4, 2), (2, 0), (2, 4) };
        
        // 見栄え的に1回もしくは2回カットするといい感じ
        int count = Random.Range(1, 3);
        for (int i = 0; i < count; i++)
        {
            // カットする基準になる区域をリストからランダムに取得
            int r = Random.Range(0, edgeCenters.Length);
            int posZ = edgeCenters[r].Item1;
            int posX = edgeCenters[r].Item2;
            // 対になる辺では正の方向が反対になるので下と左の場合は反転させる
            // 下と左の場合はXもしくはZが最大なので、足すと一辺の長さを超える
            bool cutPositive = posZ + posX > 5 ? !isPositive : isPositive;

            // 上下の辺の場合
            if (posX == 2)
            {
                // 左右どちらかにカットする
                if (cutPositive)
                    CutMapEdge(areas, (posZ, posX), Direction.Right);
                else
                    CutMapEdge(areas, (posZ, posX), Direction.Left);
            }
            // 左右の辺の場合
            else
            {
                // 上下どちらかにカットする
                if (cutPositive)
                    CutMapEdge(areas, (posZ, posX), Direction.Up);
                else
                    CutMapEdge(areas, (posZ, posX), Direction.Down);
            }
        }

        /* 穴を開ける */
        // 任意の箇所からランダムな方向を取得する
        // その方向が3方向に繋がっているか調べる
        // マップの端はカットするとおかしくなるので除く
        int rz = Random.Range(1, 5 - 1);
        int rx = Random.Range(1, 5 - 1);
        Direction dir = (Direction)Random.Range(0, 4);

        if (dir == Direction.Up)
        {
            int connect = 0;
            if (areas[rz - 1, rx]._roadStrs[2, 3] == "r") connect++;
            if (areas[rz - 1, rx]._roadStrs[4, 3] == "r") connect++;
            if (areas[rz - 1, rx]._roadStrs[3, 2] == "r") connect++;
            if (areas[rz - 1, rx]._roadStrs[3, 4] == "r") connect++;

            if (connect >= 3)
                CutMapEdge(areas, (rz, rx), dir);
        }
        else if (dir == Direction.Down)
        {
            int connect = 0;
            if (areas[rz + 1, rx]._roadStrs[2, 3] == "r") connect++;
            if (areas[rz + 1, rx]._roadStrs[4, 3] == "r") connect++;
            if (areas[rz + 1, rx]._roadStrs[3, 2] == "r") connect++;
            if (areas[rz + 1, rx]._roadStrs[3, 4] == "r") connect++;

            if (connect >= 3)
                CutMapEdge(areas, (rz, rx), dir);
        }
        else if (dir == Direction.Right)
        {
            int connect = 0;
            if (areas[rz, rx + 1]._roadStrs[2, 3] == "r") connect++;
            if (areas[rz, rx + 1]._roadStrs[4, 3] == "r") connect++;
            if (areas[rz, rx + 1]._roadStrs[3, 2] == "r") connect++;
            if (areas[rz, rx + 1]._roadStrs[3, 4] == "r") connect++;

            if (connect >= 3)
                CutMapEdge(areas, (rz, rx), dir);
        }
        else if (dir == Direction.Left)
        {
            int connect = 0;
            if (areas[rz, rx - 1]._roadStrs[2, 3] == "r") connect++;
            if (areas[rz, rx - 1]._roadStrs[4, 3] == "r") connect++;
            if (areas[rz, rx - 1]._roadStrs[3, 2] == "r") connect++;
            if (areas[rz, rx - 1]._roadStrs[3, 4] == "r") connect++;

            if (connect >= 3)
                CutMapEdge(areas, (rz, rx), dir);
        }

        return areas;
    }

    /// <summary>基礎となる道路を引く</summary>
    string[,] SetBaseRoad(string[,] strs, int zPos, int xPos)
    {
        // 真ん中は必ず道路になる
        strs[3, 3] = "r";

        // その区域が端か判定する
        bool leftEdge = xPos == 0 ? true : false;
        bool rightEdge = xPos == 5 - 1 ? true : false;
        bool topEdge = zPos == 0 ? true : false;
        bool bottomEdge = zPos == 5 - 1 ? true : false;

        if (!leftEdge)
            SetCharToDirection(strs, "r", Direction.Left);
        if (!rightEdge)
            SetCharToDirection(strs, "r", Direction.Right);
        if (!topEdge)
            SetCharToDirection(strs, "r", Direction.Up);
        if (!bottomEdge)
            SetCharToDirection(strs, "r", Direction.Down);

        return strs;
    }

    /// <summary>任意の辺をカットする</summary>
    void CutMapEdge(Area[,] areas, (int, int) index, Direction dir)
    {
        int posZ = index.Item1;
        int posX = index.Item2;

        // 右方向を消して隣の区域の左方向を消す
        if (dir == Direction.Right)
        {
            SetCharToDirection(areas[posZ, posX]._roadStrs, "g", Direction.Right);
            SetCharToDirection(areas[posZ, posX + 1]._roadStrs, "g", Direction.Left);
        }
        // 左方向を消して隣の区域の右方向を消す
        else if (dir == Direction.Left)
        {
            SetCharToDirection(areas[posZ, posX]._roadStrs, "g", Direction.Left);
            SetCharToDirection(areas[posZ, posX - 1]._roadStrs, "g", Direction.Right);
        }
        // 下方向を消して隣の区域の上方向を消す
        else if (dir == Direction.Down)
        {
            SetCharToDirection(areas[posZ, posX]._roadStrs, "g", Direction.Down);
            SetCharToDirection(areas[posZ + 1, posX]._roadStrs, "g", Direction.Up);
        }
        // 上方向を消して隣の区域の下方向を消す
        else if (dir == Direction.Up)
        {
            SetCharToDirection(areas[posZ, posX]._roadStrs, "g", Direction.Up);
            SetCharToDirection(areas[posZ - 1, posX]._roadStrs, "g", Direction.Down);
        }
    }

    /// <summary>区域の中央一マス先から指定した方向の文字を置き換える</summary>
    void SetCharToDirection(string[,] strs, string str, Direction dir)
    {
        int center = 3;
        (int, int) pairZX;
        if      (dir == Direction.Up)    pairZX = (-1, 0);
        else if (dir == Direction.Down)  pairZX = (1, 0);
        else if (dir == Direction.Right) pairZX = (0, 1);
        else    /* Direction.Left */     pairZX = (0, -1);

        // 区域の中央から端までは3マスある
        for (int i = 1; i <= 3; i++)
        {
            int addZ = pairZX.Item1 * i;
            int addX = pairZX.Item2 * i;
            strs[center + addZ, center + addX] = str;
        }
    }
}
