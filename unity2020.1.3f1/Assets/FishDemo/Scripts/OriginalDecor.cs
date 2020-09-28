using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(ARPlane))]
public class OriginalDecor : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    ARPlane aR { get { if (_aR == null) _aR = GetComponent<ARPlane>(); return _aR; } }
    /// <summary>
    /// 
    /// </summary>
    ARPlane _aR;    
    /// <summary>
    /// 
    /// </summary>
    public Decor decor;
    /// <summary>
    /// 
    /// </summary>
    public void SelfBirth(GameObject decor)
    {
        this.decor = Instantiate(decor, null).GetComponent<Decor>();

        this.decor.transform.localScale = this.decor.originScale;
        this.decor.transform.localRotation = this.decor.originQuaternion;
        this.decor.transform.position = aR.center + this.decor.originPositionOffset;

        this.decor.gameObject.SetActive(true);        
    }
}
