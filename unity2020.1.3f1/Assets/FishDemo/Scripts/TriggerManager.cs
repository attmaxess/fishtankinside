using System.Collections;
using System.Collections.Generic;
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
