using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// �Q�[���J�n���̉��o���s��
/// </summary>
public class GameStartStag : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>���o���s��</summary>
    public async UniTask Play()
    {
        Text text = GetComponentInChildren<Text>();
        for (int i = 3; i >= 1; i--)
        {
            text.text = i.ToString();
            await UniTask.Delay(1000);
        }
        text.text = "START!";
        await UniTask.Delay(1000);

        // ���o���I��������\���ɂȂ�
        gameObject.SetActive(false);
    }
}
