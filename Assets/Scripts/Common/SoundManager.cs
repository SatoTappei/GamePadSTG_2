using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �T�E���h���Ǘ�����}�l�[�W���[
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // �g���f�[�^���J�n���Ƀ��\�[�X�t�H���_����E���Ă���T�E���h�}�l�[�W���[
    // �T�E���h�̃f�[�^��SO�ō��

    readonly string BGMVolumeKey = "BGMVolumeKey";
    readonly string SEVolumeKey = "SEVolumeKey";
    readonly float BGMVolumeDefault = 1.0f;
    readonly float SEVolumeDefault = 1.0f;

    readonly string BGMPath = "Sound/BGM";
    readonly string SEPath = "Sound/SE";

    // �t�F�[�h����
    readonly float FadeHigh = 0.9f;
    readonly float FadeLow = 0.3f;

    // ���ɗ�������
    string _nextBGMName;
    string _nextSEName;

    // BGM���t�F�[�h�A�E�g����
    bool _isFadeOut = false;

    List<AudioSource> _sourceList = new List<AudioSource>();
    readonly int Length = 10;

    Dictionary<string, AudioClip> _bgmDic = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> _seDic = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < Length; i++)
            gameObject.AddComponent<AudioSource>();

        // �ǉ�����AudioSource���擾���ď����ݒ���s��
        AudioSource[] sources = GetComponents<AudioSource>();

        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].playOnAwake = false;

            // �z��̐擪��BGM�p�ɂ���
            if (i == 0)
            {
                sources[i].loop = true;
                sources[i].volume = PlayerPrefs.GetFloat(BGMVolumeKey, BGMVolumeDefault);
                _sourceList.Add(sources[i]);
            }
            else
            {
                sources[i].volume = PlayerPrefs.GetFloat(SEVolumeKey, SEVolumeDefault);
                _sourceList.Add(sources[i]);
            }

            // ���\�[�X�t�H���_���特���擾���Ă���
            AudioClip[] bgmList = Resources.LoadAll(BGMPath) as AudioClip[];
            AudioClip[] seList = Resources.LoadAll(SEPath) as AudioClip[];

            foreach (AudioClip bgm in bgmList)
                _bgmDic[bgm.name] = bgm;
            foreach (AudioClip se in seList)
                _seDic[se.name] = se;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlaySE(string name, float delay = 0.0f)
    {
        if (_seDic.ContainsKey(name))
        {
            //_nextSEName
            // ��肩��
        }
    }
}
