using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンドを管理するマネージャー
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // 使うデータを開始時にリソースフォルダから拾ってくるサウンドマネージャー
    // サウンドのデータはSOで作る

    readonly string BGMVolumeKey = "BGMVolumeKey";
    readonly string SEVolumeKey = "SEVolumeKey";
    readonly float BGMVolumeDefault = 1.0f;
    readonly float SEVolumeDefault = 1.0f;

    readonly string BGMPath = "Sound/BGM";
    readonly string SEPath = "Sound/SE";

    // フェード時間
    readonly float FadeHigh = 0.9f;
    readonly float FadeLow = 0.3f;

    // 次に流す音源
    string _nextBGMName;
    string _nextSEName;

    // BGMをフェードアウト中か
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

        // 追加したAudioSourceを取得して初期設定を行う
        AudioSource[] sources = GetComponents<AudioSource>();

        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].playOnAwake = false;

            // 配列の先頭をBGM用にする
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

            // リソースフォルダから音を取得してくる
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
            // 作りかけ
        }
    }
}
