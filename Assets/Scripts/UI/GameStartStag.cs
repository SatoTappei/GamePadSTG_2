using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public IEnumerator Play()
    {
        Text text = GetComponentInChildren<Text>();
        for (int i = 3; i >= 1; i--)
        {
            text.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        text.text = "START!";
        yield return new WaitForSeconds(1.0f);

        // ���o���I��������\���ɂȂ�
        gameObject.SetActive(false);
    }
}
