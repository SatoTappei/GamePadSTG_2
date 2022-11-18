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

    AreaRoadGenerator _areaGenerator;
    BuildingGenerator _buildingGenerator;
    /// <summary>マップ上に設置する建築物のリスト</summary>
    [SerializeField] List<Building> _buildingList;
    /// <summary>建築物を検索する用の辞書型</summary>
    Dictionary<char, Building> _buildingDic = new Dictionary<char, Building>();

    //static readonly int MapHeight = MapGenerateUtility.MapHeight;
    //static readonly int MapWidth = MapGenerateUtility.MapWidth;
    //static readonly int AreaWide = MapGenerateUtility.AreaWide;

    void Awake()
    {
        _areaGenerator = GetComponent<AreaRoadGenerator>();
        _buildingGenerator = GetComponent<BuildingGenerator>();
        _buildingList.ForEach(b => _buildingDic.Add(b._char, b));
    }

    void Start()
    {
        /* マップの生成処理をここから書く */

        // 高さと幅を指定してマップを作成
        Map map = new Map(Map.Height, Map.Width);
        _areaGenerator.Generate(map.Areas);
        _buildingGenerator.Generate(map.Areas);
        /* マップの生成処理ここまで */

        // 文字型の二次元配列から区域を生成する
        for (int z = 0; z < Map.Height; z++)
            for (int x = 0; x < Map.Width; x++)
            {
                char[,] strMap = map.Areas[z, x].GetCharArray();
                GameObject areaRoot = BuildingFromArray(strMap);
                areaRoot.transform.position = new Vector3(z * Area.Wide * 4, 0, x * Area.Wide * 4);
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
                    int offsetZ = -1 * (Map.Height / 2) - 1;
                    int offsetX =  -1 * (Map.Width / 2) - 1;
                    Vector3 pos = new Vector3(i + offsetX, 0, j + offsetZ);
                    Quaternion rot = Quaternion.identity;
                    Transform parent = root.transform;

                    Instantiate(value._object, pos, rot, parent);
                }
            }

        root.transform.localScale = Vector3.one * 4;

        return root;
    }
}
