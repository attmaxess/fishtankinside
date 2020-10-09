using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class TriggerManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public bool isDebug = false;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] List<string> triggerLayers = new List<string>();
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] List<GameObject> triggers = new List<GameObject>();
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector3 triggerCenter()
    {
        return triggerLayers.Count >= 1 ?
            //(new Vector3(
            //    triggers.Average(go => go.transform.position.x),
            //    triggers.Average(go => go.transform.position.y),
            //    triggers.Average(go => go.transform.position.z)) + transform.position) / 2f
            (triggerSumup() + transform.position) / (triggerLayers.Count + 1)
            : transform.position;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector3 triggerSumup()
    {
        return triggers.Aggregate(new Vector3(0, 0, 0), (s, v) => s + v.transform.position);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    void AddTrigger(GameObject go)
    {
        if (triggers.Contains(go)) return;
        triggers.Add(go);
        if (onTriggerListChange != null) onTriggerListChange.Invoke();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    void RemoveTrigger(GameObject go)
    {
        if (!triggers.Contains(go)) return;
        triggers.Remove(go);
        if (onTriggerListChange != null) onTriggerListChange.Invoke();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool isTrigger()
    {
        return triggers.Count != 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (isDebug) Debug.Log(other.name);
        if (!triggerLayers.Contains(LayerMask.LayerToName(other.gameObject.layer))) return;
        AddTrigger(other.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //if (isDebug) Debug.Log(other.name);
        //if (!triggerLayers.Contains(LayerMask.LayerToName(other.gameObject.layer))) return;
        //AddTrigger(other.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        RemoveTrigger(other.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    public delegate void OnTriggerListChange();
    /// <summary>
    /// 
    /// </summary>
    public OnTriggerListChange onTriggerListChange;
}
