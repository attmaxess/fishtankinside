using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class FishesCountComponent : MonoBehaviour
{
    /// <summary>
    /// Fishes : <mark=#ffff00aa>_x_</mark>
    /// Inrange : <mark=#ffff00aa>_y_</mark>
    /// </summary>
    public TextMeshProUGUI text;
    /// <summary>
    /// 
    /// </summary>
    public int fishescount
    {
        get { return _fishescount; }
        set { _fishescount = value; SetText(); }
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] int _fishescount;
    /// <summary>
    /// 
    /// </summary>
    public int fishesinrange
    {
        get { return _fishesinrange; }
        set { _fishesinrange = value; SetText(); }
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] int _fishesinrange;
    /// <summary>
    /// 
    /// </summary>
    void SetText()
    {
        text.text = "Fishes : <mark=#ffff00aa>_" + _fishescount + "_</mark>\n" +
            "Inrange : <mark=#ffff00aa>_" + _fishesinrange + "_</mark>";
    }
}
/// <summary>
/// 
/// </summary>
public static class FisherCount
{
    /// <summary>
    /// 
    /// </summary>
    static FishesCountComponent fishcomp
    {
        get
        {
            if (_fishcomp == null) _fishcomp = GameObject.FindObjectOfType<FishesCountComponent>();
            return _fishcomp;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    static FishesCountComponent _fishcomp;
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static int GetFishesCount()
    {
        if (fishcomp == null) return 0;
        return fishcomp.fishescount;
    }
    /// <summary>
    /// 
    /// </summary>
    public static void SetFishesCount(int count)
    {
        if (fishcomp == null) return;
        fishcomp.fishescount = count;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static int GetFishesInrange()
    {
        if (fishcomp == null) return 0;
        return fishcomp.fishesinrange;
    }
    /// <summary>
    /// 
    /// </summary>
    public static void SetFishesInrange(int count)
    {
        if (fishcomp == null) return;
        fishcomp.fishesinrange = count;
    }
}