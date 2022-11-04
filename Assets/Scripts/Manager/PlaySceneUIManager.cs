using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

/// <summary>
/// �Q�[���{�҂�UI���Ǘ�����
/// </summary>
public class PlaySceneUIManager : MonoBehaviour
{
    [SerializeField] GameStartStag _gsStag;
    [SerializeField] Timer _timer;
    /// <summary>�_���[�W���󂯂������������Q�[�W</summary>
    [SerializeField] Transform _damageGauge;
    /// <summary>�X�R�A��\������e�L�X�g</summary>
    [SerializeField] Text _scoreText;
    /// <summary>�^�[�Q�b�g�̏���\������UI(�J�E���^�[)</summary>
    [SerializeField] Text _targetLabelCount;
    /// <summary>�^�[�Q�b�g�̏���\������UI(�A�C�R��)</summary>
    [SerializeField] Image _targetLabelIcon;
    
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

    /// <summary>�^�[�Q�b�g�̐��Ƃ��̃A�C�R�����Z�b�g����</summary>
    public void SetTargetLabel(int count, Sprite icon)
    {
        _targetLabelCount.text = count.ToString();
        _targetLabelIcon.sprite = icon;
    }

    /// <summary>�Q�[���J�n���̉��o���s��</summary>
    public async UniTask PlayGameStartStag()
    {
        await _gsStag.Play();
    }

    /// <summary>�R�[���o�b�N��n���ă^�C�}�[���X�^�[�g������</summary>
    public void TimerStart(UnityAction action = null)
    {
        _timer.TimeUpEvent += action;
        _timer.TimerStart();
    }
}
