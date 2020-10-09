using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(TriggerManager))]
public class DriftDependTrigger : MonoBehaviour
{
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
    private void OnEnable()
    {
        triggerManager.onTriggerListChange += DriftApart;
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        triggerManager.onTriggerListChange -= DriftApart;
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] bool isDrifting = false;
    /// <summary>
    /// 
    /// </summary>
    void DriftApart()
    {
        if (!triggerManager.isTrigger()) return;

        if (!isDrifting) StartCoroutine(c_DriftApart());
        else
        {
            StopAllCoroutines();
            isDrifting = false;
            StartCoroutine(c_DriftApart());
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_DriftApart()
    {
        isDrifting = true;

        Vector3 center = triggerManager.triggerCenter();
        Vector3 dir = (center != transform.position)
            ? (transform.position - center).normalized
            : new Vector3(Random.Range(1f, 9f), 0f, Random.Range(1f, 9f)).normalized;
        dir *= 0.01f;

        while (triggerManager.isTrigger())
        {
            transform.Translate(dir);
            yield return new WaitForEndOfFrame();
        }

        isDrifting = false;
        yield break;
    }
}
