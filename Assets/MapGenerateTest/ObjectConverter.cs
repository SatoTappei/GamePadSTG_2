using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 渡された文字列を建物に変換してマップにする
/// </summary>
public class ObjectConverter : MonoBehaviour
{
    /// <summary>マップに配置するオブジェクト</summary>
    [System.Serializable]
    public class Building
    {
        public char _char;
        public GameObject _object;
    }

    // 1つの区域
    public struct Area
    {
        // 文字列型の区域の二次元配列
        public string[,] strs;
    }

    AreaGenerator _areaGenerator;
    /// <summary>マップ上に設置する建築物のリスト</summary>
    [SerializeField] List<Building> _buildingList;
    /// <summary>建築物を検索する用の辞書型</summary>
    Dictionary<char, Building> _buildingDic = new Dictionary<char, Building>();

    void Awake()
    {
        _areaGenerator = GetComponent<AreaGenerator>();
        _buildingList.ForEach(b => _buildingDic.Add(b._char, b));
    }

    void Start()
    {
        // 各区域を並べることででかいマップにする
        // まずは区域に道路を生成する、道路には2種類(幅1マス、幅2マス)ある
        // 建物は幅2マスを考慮して作る
        // 区域の二次元配列を作成する二次元配列の二次元配列

        // 区域の元になる二次元配列を生成する
        Area area = new Area();
        area.strs = _areaGenerator.Generate();
        // 文字列から区域を生成する
        ConvertToObject(area.strs);
    }

    void Update()
    {
        
    }

    // 二次元配列をオブジェクトに変換する
    void ConvertToObject(string[,] strMap)
    {
        for (int i = 0; i < strMap.GetLength(0); i++)
            for (int j = 0; j < strMap.GetLength(1); j++)
            {
                char key = strMap[i, j][0];
                GameObject obj = _buildingDic[key]._object;

                Instantiate(obj, new Vector3(i, 0, j), Quaternion.identity);
            }
    }
}
