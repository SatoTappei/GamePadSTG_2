using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // 1�̋��
    public struct Area
    {
        // ������^�̋��̓񎟌��z��
        public string[,] strs;
    }

    AreaGenerator _areaGenerator;
    /// <summary>�}�b�v��ɐݒu���錚�z���̃��X�g</summary>
    [SerializeField] List<Building> _buildingList;
    /// <summary>���z������������p�̎����^</summary>
    Dictionary<char, Building> _buildingDic = new Dictionary<char, Building>();

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

        // ���̌��ɂȂ�񎟌��z��𐶐�����
        Area area = new Area();
        area.strs = _areaGenerator.Generate();
        // �����񂩂���𐶐�����
        ConvertToObject(area.strs);
    }

    void Update()
    {
        
    }

    // �񎟌��z����I�u�W�F�N�g�ɕϊ�����
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
