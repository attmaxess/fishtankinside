using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class ToggleObj : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public List<Transform> objs;
    /// <summary>
    /// 
    /// </summary>
    /// <param name=""></param>
    public void SetValue(bool value)
    {
        foreach (Transform transform in objs)
            transform.gameObject.SetActive(value);
    }
    /// <summary>
    /// 
    /// </summary>
    public void ToggleComponentObj()
    {
        if (objs == null) return;
        foreach (Transform transform in objs)
            transform.gameObject.SetActive(!transform.gameObject.activeSelf);
    }    
}
