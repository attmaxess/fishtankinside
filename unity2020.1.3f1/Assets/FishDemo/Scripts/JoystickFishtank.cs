using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class JoystickFishtank : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public void btnCallFish()
    {
        Transform targetCall = FishSelectionStatic.fishSelection.lastSelect;
        if (targetCall == null) return;
        FishKoi fishKoi = targetCall.GetComponent<FishKoi>();
        fishKoi.HeadToObject(Camera.main.transform);
    }
    /// <summary>
    /// 
    /// </summary>
    public void btnKickFishStraight()
    {
        Transform targetCall = FishSelectionStatic.fishSelection.lastSelect;
        if (targetCall == null) return;
        FishKoi fishKoi = targetCall.GetComponent<FishKoi>();
        fishKoi.Turn180();
    }
}
