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
    [ContextMenu("SlapAllFish")]
    public void SlapAllFish()
    {
        fishKois = FindObjectsOfType<FishKoi>().ToList();
        foreach (FishKoi fishKoi in fishKois)
            fishKoi.CameraSlap();
    }
}
