using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(TriggerManager))]
public class ReleaseConstraintDependTrigger : MonoBehaviour
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
    Rigidbody rbody { get { if (_rbody == null) _rbody = GetComponentInChildren<Rigidbody>(); return _rbody; } }
    /// <summary>
    /// 
    /// </summary>
    Rigidbody _rbody;
    /// <summary>
    /// 
    /// </summary>
    Collider col { get { if (_col == null) _col = GetComponentInChildren<Collider>(); return _col; } }
    /// <summary>
    /// 
    /// </summary>
    Collider _col;
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("StartRelease")]
    public void StartRelease()
    {
        StartCoroutine(c_StartRelease());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_StartRelease()
    {
        yield return new WaitUntil(() => triggerManager.isTrigger() == false);
        col.isTrigger = false;
        rbody.constraints = RigidbodyConstraints.None;
        yield break;
    }
}
