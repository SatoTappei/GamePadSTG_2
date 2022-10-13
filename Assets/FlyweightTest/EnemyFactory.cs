using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    /// <summary>敵の各パラメータが書かれたSOのリスト</summary>
    [SerializeField] List<ParamSO> _paramList = new List<ParamSO>();
    /// <summary>生成する敵</summary>
    [SerializeField] GameObject _enemyPrefab;
    /// <summary>敵の生成場所</summary>
    [SerializeField] Transform _enemyContainer;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>ボタンをクリックすると敵が生成される</summary>
    public void OnCreateEnemy(int idx)
    {
        // ゲームオブジェクトを生成する
        GameObject enemy = Instantiate(_enemyPrefab, _enemyContainer);
        // コンポーネントを取得する
        FlyWeightEnemyAI enemyAI = enemy.GetComponent<FlyWeightEnemyAI>();
        // その敵のパラメータをリストから選択する
        enemyAI.Param = _paramList[idx];
        // その敵が狙うターゲットを取得してくる
        enemyAI.Player = GameObject.FindGameObjectWithTag("Player").transform;

        // この後に生成された敵のStartメソッドが呼ばれる
        // ここで生成した敵のParamを設定したので反映される
    }
}
