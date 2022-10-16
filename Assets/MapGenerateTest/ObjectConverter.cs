using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�����̓��H</summary>
public struct Road
{
    GameObject _object;
}

/// <summary>�����̌���</summary>
public struct Build
{
    GameObject _object;
}

public struct Area
{
    //Road _road;
    //Build _build;
    public string[,] _roadStrs;

    // 7*7�̃}�X�ɓ��H�𐶐�����
    // �󂢂��}�X�Ɍ����𐶐�����

    // ����1�}�X�̓��H�̏ꍇ��3*3�̋󂫂�����
    // ����2�}�X�̓��H�̏ꍇ��2.5*2.5�̋󂫂�����
}

/// <summary>
/// �n���ꂽ������������ɕϊ����ă}�b�v�ɂ���
/// </summary>
public class ObjectConverter : MonoBehaviour
{
    /// <summary>�}�b�v�ɔz�u����I�u�W�F�N�g</summary>
    [System.Serializable]
    public class Building
    {
        public char _char;
        public GameObject _object;
    }   

    /// <summary>
    /// �}�b�v�͂���1�����Ȃ��̂�static�N���X�ɂ���
    /// ���H���C���[�ƌ������C���[����Ȃ�
    /// </summary>
    public static class Map
    {
        /// <summary>���̓񎟌��z��</summary>
        public static Area[,] _areas = new Area[MapWidth, MapHeight];

    }

    AreaGenerator _areaGenerator;
    /// <summary>�}�b�v��ɐݒu���錚�z���̃��X�g</summary>
    [SerializeField] List<Building> _buildingList;
    /// <summary>���z������������p�̎����^</summary>
    Dictionary<char, Building> _buildingDic = new Dictionary<char, Building>();

    // �}�b�v�̑傫���͊����Ȃ��Ƌ����Y��ɕ��ׂ邱�Ƃ��o���Ȃ�
    // �傫������ƕ��ׂ�������(����)�̂ōő�ł�5*5�ɂ��Ă���
    static readonly int MapWidth = 5;
    static readonly int MapHeight = 5;

    void Awake()
    {
        _areaGenerator = GetComponent<AreaGenerator>();
        _buildingList.ForEach(b => _buildingDic.Add(b._char, b));
    }

    void Start()
    {
        // �e������ׂ邱�Ƃłł����}�b�v�ɂ���
        // �܂��͋��ɓ��H�𐶐�����A���H�ɂ�2���(��1�}�X�A��2�}�X)����
        // �����͕�2�}�X���l�����č��
        // ���̓񎟌��z����쐬����񎟌��z��̓񎟌��z��

        // ���𐶐����ē񎟌��z��Ɋi�[����
        _areaGenerator.Generate(Map._areas);

        //// ���^�̓񎟌��z����쐬����
        //Area[,] map = new Area[MapWidth, MapHeight];

        for (int i = 0; i < MapWidth; i++)
            for (int j = 0; j < MapHeight; j++)
            {
                // ������^�̓񎟌��z�񂩂���𐶐�����
                GameObject areaRoot = BuildingFromArray(Map._areas[i,j]._roadStrs);
                areaRoot.transform.position = new Vector3(i * 7, 0, j * 7);
            }
    }

    void Update()
    {
        
    }

    /// <summary>������^�̓񎟌��z�񂩂猚�z���𐶐����āA���Ƃ��ĕԂ�</summary>
    GameObject BuildingFromArray(string[,] strMap)
    {
        // �����������z�������Ƃ��Đݒ肷��
        GameObject root = new GameObject();
        root.name = "AreaRoot";

        for (int i = 0; i < strMap.GetLength(0); i++)
            for (int j = 0; j < strMap.GetLength(1); j++)
            {
                char key = strMap[i, j][0];
                bool isExist = _buildingDic.TryGetValue(key, out Building value);

                // �Ή����镶��������ΐ�������
                if (isExist)
                {
                    // ���̒����Ƌ����̐^�񒆂̃I�u�W�F�N�g�̈ʒu�����킹�邽�߃I�t�Z�b�g�𑫂�
                    int offsetX =  -1 * (MapWidth / 2) - 1;
                    int offsetY = -1 * (MapHeight / 2) - 1;
                    GameObject go = Instantiate(value._object, new Vector3(i + offsetX, 0, j + offsetY), Quaternion.identity);
                    go.transform.SetParent(root.transform);
                }
            }

        return root;
    }
}
