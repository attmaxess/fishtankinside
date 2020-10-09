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
    GameObject Plane { get { if (_Plane == null) _Plane = transform.Find("Plane").gameObject; return _Plane; } }
    /// <summary>
    /// 
    /// </summary>
    GameObject _Plane;
    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
#if UNITY_EDITOR
        Plane.SetActive(true);
#endif
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS || UNITY_WP_8 || UNITY_WP_8_1)        
        Plane.SetActive(false);
#endif

    }
    /// <summary>
    /// 
    /// </summary>
    public Decor decor;
    /// <summary>
    /// 
    /// </summary>
    public Seaweed seaweed;
    /// <summary>
    /// 
    /// </summary>
    public void SelfBirthDecor(GameObject decor)
    {
        this.decor = Instantiate(decor, null).GetComponent<Decor>();

        this.decor.transform.localScale = this.decor.originScale;
        this.decor.transform.localRotation = this.decor.originQuaternion;
        this.decor.transform.position = aR.center + this.decor.originPositionOffset;

        this.decor.gameObject.SetActive(true);
    }
    /// <summary>
    /// 
    /// </summary>
    public void SelfBirthSeaweed(GameObject seaweed)
    {
        StartCoroutine(c_SelfBirthSeaweed(seaweed));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="seaweed"></param>
    /// <returns></returns>
    IEnumerator c_SelfBirthSeaweed(GameObject seaweed)
    {
        this.seaweed = Instantiate(seaweed, null).GetComponent<Seaweed>();
        Decor decorSeaweed = seaweed.GetComponent<Decor>();

        decorSeaweed.transform.localScale = decorSeaweed.originScale;
        decorSeaweed.transform.localRotation = decorSeaweed.originQuaternion;
        decorSeaweed.transform.position = aR.center + decorSeaweed.originPositionOffset;

        this.seaweed.gameObject.SetActive(true);

        TriggerManager newSeaweedTrigger = this.seaweed.GetComponent<TriggerManager>();
        yield return new WaitUntil(() => newSeaweedTrigger.isTrigger() == false);
        this.seaweed.GetComponent<ReleaseConstraintDependTrigger>().StartRelease();
        yield break;
    }
}
