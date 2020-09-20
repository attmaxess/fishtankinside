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
    /// <summary>
    /// 
    /// </summary>
    public void btnDuplicateFish()
    {
        if (FishSelectionStatic.fishSelection.lastSelect == null) return;
        Fishtemplate.Instance.Duplicate(FishSelectionStatic.fishSelection.lastSelect);
    }
    /// <summary>
    /// 
    /// </summary>
    public void btnDeleteFish()
    {
        if (FishSelectionStatic.fishSelection.lastSelect == null) return;
        if (Application.isPlaying) DestroyImmediate(FishSelectionStatic.fishSelection.lastSelect.gameObject);
        else Destroy(FishSelectionStatic.fishSelection.lastSelect.gameObject);
    }
}
