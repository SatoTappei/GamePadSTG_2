using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// ステージ上の敵を管理する
/// </summary>
public class EnemyManager : MonoBehaviour
{
    // SO周りの処理だけ抜き出して別のスクリプトで管理することも考慮しておく
    /// <summary>敵の種類ごとの共通したデータの参照先のSO</summary>
    [SerializeField] ActorDataSO[] _enemyDatas;
    /// <summary>敵の共通したデータの参照先となるSOを値として保存する辞書型</summary>
    Dictionary<CharacterTag, ActorDataSO> _enemyDataDic = new Dictionary<CharacterTag, ActorDataSO>();

    /// <summary>ステージ上に存在する敵のリスト</summary>
    List<EnemySubjecter> _enemyList = new List<EnemySubjecter>();
    /// <summary>生存しているターゲットのリスト</summary>
    ReactiveCollection<EnemySubjecter> _targets = new ReactiveCollection<EnemySubjecter>();

    /// <summary>ターゲットが"減ったら"行う処理を登録する</summary>
    public IObservable<CollectionRemoveEvent<EnemySubjecter>> TargetsObservable => _targets.ObserveRemove();

    void Awake()
    {
        foreach(ActorDataSO so in _enemyDatas)
        {
            _enemyDataDic.Add(so.Tag, so);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>初期化処理</summary>
    public void Init(CharacterTag tag)
    {
        // タグでシーン上の敵を全部取得してリストに追加する
        // TODO:現状タグで検索してコンポーネントを取得しているので他の方法がないか模索する
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemySubjecter es = go.GetComponent<EnemySubjecter>();
            _enemyList.Add(es);

            // 渡されたタグと同じだったらターゲットのリストに追加し
            // 非表示になった(倒された)らリストから削除されるようにすることでターゲットビューの更新を呼ぶ
            if (es.EnemyTag == tag)
            {
                _targets.Add(es);
                es.gameObject.OnDisableAsObservable().Subscribe(_ => _targets.Remove(es));
            }
        }
    }

    /// <summary>残りのターゲット数を返す</summary>
    public int GetTargetAmount() => _targets.Count;

    /// <summary>タグを渡すと対応したSOを返す</summary>
    public ActorDataSO GetEnemyData(CharacterTag tag) => _enemyDataDic[tag];

    /// <summary>全ての敵を起こす</summary>
    public void WakeUpEnemyAll() => _enemyList.ForEach(e => e.WakeUp());

    ///// <summary>生成された敵側がこのメソッドを呼んで自身を登録する</summary>
    //public void AddEnemyList(EnemySubjecter enemy) => _enemyList.Add(enemy);

    /// <summary>ステージ内の敵からタグで倒すターゲットを取得する</summary>
    //public List<GameObject> GetTarget(CharacterTag tag)
    //{
    //    _targetList = _enemyList
    //        .Where(e => e.EnemyTag == tag).Select(e => e.gameObject).ToList();

    //    // ターゲットが減ったらの処理をここに書いておくが後で適切な場所に置く
    //    _targetList.ForEach(go => go.OnDisableAsObservable().Subscribe(_=> DecreaseTargetCount()));

    //    _rem.Value = _targetList.Count;

    //    return _targetList;
    //}
}
