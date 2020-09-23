using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class FishInfoComponent : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public TextMeshProUGUI text;
    /// <summary>
    /// 
    /// </summary>
    public TMP_InputField ipf;
    /// <summary>
    /// 
    /// </summary>
    public Skillsheet.KeyValuePair currentInfo
    {
        get { return _currentInfo; }
        set { HandleSetInfo(value); }
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] Skillsheet.KeyValuePair _currentInfo;
    /// <summary>
    /// 
    /// </summary>
    void HandleSetInfo(Skillsheet.KeyValuePair value)
    {
        SetText(value.tooltip, value.valueString);
        ipf.text = value.valueString;
        _currentInfo = value;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="valueString"></param>
    void SetText(string tooltip, string valueString)
    {
        text.text = tooltip + " : " + " <color=red><mark=#ffff00aa>_" + valueString + "_</mark></color>";
    }
    /// <summary>
    /// 
    /// </summary>
    public void OnEndEdit()
    {
        currentInfo.valueString = ipf.text;
        SetText(currentInfo.tooltip, currentInfo.valueString);
        currentInfo.skillsheet.ChangeForAll();
    }
}
