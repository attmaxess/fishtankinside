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
    public float slapdistance = 0.5f;
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("SlapAllFish")]
    public void SlapAllFish()
    {
        fishKois = FindObjectsOfType<FishKoi>().ToList();
        foreach (FishKoi fishKoi in fishKois)
            if ((fishKoi.isDead) && ((Camera.main.transform.position - fishKoi.transform.position).sqrMagnitude <= slapdistance))
            {
                fishKoi.SelfSlap();
                fishKoi.Revive();
                fishKoi.GetComponentInChildren<Hungry>().Restart();
            }
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
