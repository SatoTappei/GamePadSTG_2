using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �Q�[���{�҂�UI���Ǘ�����
/// </summary>
public class PlaySceneUIManager : MonoBehaviour
{
    /// <summary>�_���[�W���󂯂������������Q�[�W</summary>
    [SerializeField] Transform _damageGauge;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// HP�Q�[�W��ω�������
    /// </summary>
    /// <param name="max">�v���C���[�̍ő�̗�</param>
    /// <param name="current">�v���C���[�̌��݂̗̑�</param>
    public void SetLifeGauge(int max, int current)
    {
        float xValue = 1 - (current * 1.0f / max * 1.0f);
        _damageGauge.DOScaleX(xValue, 0.5f);
    }
}
