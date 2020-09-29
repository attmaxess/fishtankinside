using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class FishSlap : Singleton<FishSlap>
{
    /// <summary>
    /// 
    /// </summary>
    List<FishKoi> fishKois;
    /// <summary>
    /// 
    /// </summary>
    public float slapdistance = 0.05f;
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("SlapAllFish")]
    public void SlapAllFish()
    {
        fishKois = FindObjectsOfType<FishKoi>().ToList();
        foreach (FishKoi fishKoi in fishKois)
            if ((Camera.main.transform.position - fishKoi.transform.position).sqrMagnitude <= slapdistance)
                fishKoi.CameraSlap();
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("DebugDistance")]
    public void DebugDistance()
    {
        fishKois = FindObjectsOfType<FishKoi>().ToList();
        foreach (FishKoi fishKoi in fishKois)
            Debug.Log(fishKoi.gameObject.name + " " + ((Camera.main.transform.position - fishKoi.transform.position).sqrMagnitude));
    }
}
