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

    AreaGenerator _areaGenerator;
    BuildingGenerator _buildingGenerator;
    /// <summary>マップ上に設置する建築物のリスト</summary>
    [SerializeField] List<Building> _buildingList;
    /// <summary>建築物を検索する用の辞書型</summary>
    Dictionary<char, Building> _buildingDic = new Dictionary<char, Building>();

    static readonly int MapHeight = MapGenerateUtility.MapHeight;
    static readonly int MapWidth = MapGenerateUtility.MapWidth;
    static readonly int AreaWide = MapGenerateUtility.AreaWide;

    void Awake()
    {
        _areaGenerator = GetComponent<AreaGenerator>();
        _buildingGenerator = GetComponent<BuildingGenerator>();
        _buildingList.ForEach(b => _buildingDic.Add(b._char, b));
    }

    void Start()
    {
        /* マップの生成処理をここから書く */

        // 高さと幅を指定してマップを作成
        Map map = new Map(MapHeight, MapWidth);
        _areaGenerator.Generate();

        /* マップの生成処理ここまで */

        // 文字型の二次元配列から区域を生成する
        for (int z = 0; z < MapHeight; z++)
            for (int x = 0; x < MapWidth; x++)
            {
                char[,] strMap = map.Areas[z, x].GetStringArray();
                GameObject areaRoot = BuildingFromArray(strMap);
                areaRoot.transform.position = new Vector3(z * AreaWide, 0, x * AreaWide);
            }
    }

    void Update()
    {
        
    }

    /// <summary>文字列型の二次元配列から建築物を生成して、区域として返す</summary>
    GameObject BuildingFromArray(char[,] strMap)
    {
        // 生成した建築物を区域として設定する
        GameObject root = new GameObject();
        root.name = "AreaRoot";

        for (int i = 0; i < strMap.GetLength(0); i++)
            for (int j = 0; j < strMap.GetLength(1); j++)
            {
                char key = strMap[i, j];
                bool isExist = _buildingDic.TryGetValue(key, out Building value);

                // 対応する文字があれば生成する
                if (isExist)
                {
                    // 区域の中央と区域内の真ん中のオブジェクトの位置を合わせるためオフセットを足す
                    int offsetX =  -1 * (MapWidth / 2) - 1;
                    int offsetY = -1 * (MapHeight / 2) - 1;
                    GameObject go = Instantiate(value._object, new Vector3(i + offsetX, 0, j + offsetY), Quaternion.identity);
                    go.transform.SetParent(root.transform);
                }
            }

        return root;
    }
}
