using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ŒãX—Ç‚¢Š´‚¶‚ÌêŠ‚ÉˆÚ‚·
/// <summary>“G‚ğ¯•Ê‚·‚é‚½‚ß‚Ìƒ^ƒO‚Æ‚µ‚Äg‚¤</summary>
public enum EnemyTag
{
    BlueSoldier,
    Tank,
    BossTank,
}

/// <summary>
/// “G‚Ìî•ñ‚ğEnemyManager‚É“o˜^‚·‚é
/// </summary>
public class EnemySubjecter : MonoBehaviour
{
    EnemyAIBase _aiBase;
    [SerializeField] EnemyTag _tag;

    public EnemyTag Tag { get; private set; }

    void Awake()
    {
        _aiBase = GetComponent<EnemyAIBase>();
    }

    void Start()
    {
        // ‹@”\‚³‚¹‚é‚©‚Ç‚¤‚©‚ğŠÇ—‚µ‚Ä‚à‚ç‚¤‚½‚ß‚É©g‚ğ“o˜^‚·‚é
        FindObjectOfType<EnemyManager>().AddEnemyList(this);
    }

    void Update()
    {
        
    }

    // “G‚ğ‹N‚±‚·
    public void WakeUp()
    {
        _aiBase.WakeUp();
    }
}
