using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// €‚ñ‚¾‚Æ‚«‚Éo‚éƒ‰ƒOƒh[ƒ‹‚É—Í‚ğ‰Á‚¦‚é
/// </summary>
public class RagdollControl : MonoBehaviour
{
    /// <summary>—Í‚ğ‰Á‚¦‚é”wœ‚ÌRigidBody</summary>
    [SerializeField] Rigidbody _rbSpine;
    /// <summary>ˆê’èŠÔŒo‚Á‚½‚ç•¨—‰‰Z‚ğ‚â‚ß‚é‚½‚ß‚Ì‘¼‚ÌRigidBody</summary>
    [SerializeField] Rigidbody[] _rbOther;

    //async void Start()
    //{
    //    _rbSpine.AddForce(Vector3.up * 100, ForceMode.Impulse);
        
    //    // ˆ—•‰‰×‚ÌŒyŒ¸‚Ì‚½‚ß‚É3•bŒã‚É‘S‚Ä‚Ì•¨—‰‰Z‚ğ‚â‚ß‚é
    //    await UniTask.Delay(3000);
        
    //    _rbSpine.Sleep();
    //    foreach(Rigidbody rb in _rbOther)
    //    {
    //        rb.Sleep();
    //    }
    //}
    IEnumerator Start()
    {
        _rbSpine.AddForce(Vector3.up * 100, ForceMode.Impulse);

        // ˆ—•‰‰×‚ÌŒyŒ¸‚Ì‚½‚ß‚É3•bŒã‚É‘S‚Ä‚Ì•¨—‰‰Z‚ğ‚â‚ß‚é
        yield return new WaitForSeconds(3.0f);

        _rbSpine.Sleep();
        foreach (Rigidbody rb in _rbOther)
        {
            rb.Sleep();
        }
    }

    void Update()
    {

    }
}
