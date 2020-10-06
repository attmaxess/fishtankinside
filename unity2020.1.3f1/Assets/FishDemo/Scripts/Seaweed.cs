using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Seaweed : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] bool isInit = false;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float originalXScale = 1f;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float originalTankMass = .1f;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float maxAge = 100f;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float ageStep = .5f;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float ageTimeStep = .5f;
    /// <summary>
    /// 
    /// </summary>
    float age
    {
        get { return _age; }
        set { HandleAge(value); }
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float _age = 1f;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    void HandleAge(float value)
    {
        _age = value;
    }
    /// <summary>
    /// 
    /// </summary>
    Rigidbody rbody { get { if (_rbody == null) _rbody = GetComponentInChildren<Rigidbody>(); return _rbody; } }
    /// <summary>
    /// 
    /// </summary>
    Rigidbody _rbody;
    /// <summary>
    /// 
    /// </summary>
    TriggerManager triggerManager { get { if (_triggerManager == null) _triggerManager = GetComponent<TriggerManager>(); return _triggerManager; } }
    /// <summary>
    /// 
    /// </summary>
    TriggerManager _triggerManager;
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {               
        if (!isInit) Init();
        StartCoroutine(c_Grow());
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("Init")]
    public void Init()
    {
        originalXScale = transform.localScale.x;

        maxAge = Random.Range(3f, 100f);
        ageStep = Random.Range(0.001f * originalXScale, 0.01f * originalXScale);
        ageTimeStep = Random.Range(.5f, 2f);

        isInit = true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_Grow()
    {
        ///
        yield return new WaitUntil(() => triggerManager.isTrigger() == true);

        ///
        while (age <= maxAge)
        {
            age += ageTimeStep;
            Vector3 selfBig = transform.localScale + new Vector3(ageStep, ageStep, ageStep);
            transform.localScale = Vector3.Lerp(transform.localScale, selfBig, ageTimeStep);
            yield return new WaitForSeconds(ageTimeStep);
        }
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("CurrentAgeSet")]
    public void CurrentAgeSet()
    {
        age = _age;
    }
}
