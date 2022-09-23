using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �Q�[���{�҂�UI���Ǘ�����
/// </summary>
public class PlaySceneUIManager : MonoBehaviour
{
    /// <summary>�_���[�W���󂯂������������Q�[�W</summary>
    [SerializeField] Transform _damageGauge;
    /// <summary>�X�R�A��\������e�L�X�g</summary>
    [SerializeField] Text _scoreText;
    
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
        xValue = Mathf.Clamp01(xValue);
        _damageGauge.DOScaleX(xValue, 0.5f);
    }

    /// <summary>
    /// �X�R�A���Z�b�g����
    /// </summary>
    /// <param name="score">�Z�b�g����X�R�A</param>
    public void SetScore(int score)
    {
        int prev = int.Parse(_scoreText.text.Replace(",", ""));
        _scoreText.DOCounter(prev, score, 0.5f);
    }
}
