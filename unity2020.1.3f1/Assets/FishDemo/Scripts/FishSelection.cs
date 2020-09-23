using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cakeslice;
using System.Linq;
/// <summary>
/// 
/// </summary>
public class FishSelection : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public Transform lastSelect
    {
        get { return _lastSelect; }
        set
        {
            Highlight(value);
            CreateInfoOnUI(value);
            _lastSelect = value;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] Transform _lastSelect;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="select"></param>
    void Highlight(Transform select)
    {
        cakeslice.Outline[] outlines;
        if (select == null)
        {
            if (_lastSelect != null)
            {
                outlines = _lastSelect.GetComponentsInChildren<cakeslice.Outline>();
                SetOutlines(outlines, false);
                _lastSelect = null;
            }
        }
        else
        {
            if (_lastSelect == null)
            {
                outlines = select.GetComponentsInChildren<cakeslice.Outline>();
                SetOutlines(outlines, true);
            }
            else
            {
                outlines = _lastSelect.GetComponentsInChildren<cakeslice.Outline>();
                SetOutlines(outlines, false);
                outlines = select.GetComponentsInChildren<cakeslice.Outline>();
                SetOutlines(outlines, true);
            }            
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void CreateInfoOnUI(Transform select)
    {        
        if (select == null) return;

        Skillsheet skillsheet = select.GetComponent<Skillsheet>();

        string fishname = skillsheet.skillsheets.GetKeyValuePair("fishname").valueString;
        ModelInfo.Instance.txtFishName.text = fishname;
        int countFish = FindObjectsOfType<FishKoi>().ToList()
            .Where(x => x.fishname == fishname).Count();
        ModelInfo.Instance.txtFishNumber.text = "Số lượng : " + countFish.ToString();

        if (FishInfo.Instance != null && FishInfo.Instance.gameObject.activeSelf)
        {
            FishInfo.Instance.StopAllCoroutines();
            FishInfo.Instance.DeleteAllInfo();
            FishInfo.Instance.CreateInfo(skillsheet.skillsheets);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="outlines"></param>
    /// <param name="OnEnable"></param>
    void SetOutlines(cakeslice.Outline[] outlines, bool OnEnable)
    {
        foreach (cakeslice.Outline outline in outlines)
            outline.enabled = OnEnable;
    }
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        SetOffAllOutlines();
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("SetOnAllOutlines")]
    public void SetOnAllOutlines()
    {
        cakeslice.Outline[] outlines = FindObjectsOfType<cakeslice.Outline>();
        if (outlines.Length == 0) return;
        SetOutlines(outlines, true);
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("SetOffAllOutlines")]
    public void SetOffAllOutlines()
    {
        cakeslice.Outline[] outlines = FindObjectsOfType<cakeslice.Outline>();
        if (outlines.Length == 0) return;
        SetOutlines(outlines, false);
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("ToggleAllOutlines")]
    public void ToggleAllOutlines()
    {
        cakeslice.Outline[] outlines = FindObjectsOfType<cakeslice.Outline>();
        if (outlines.Length == 0) return;
        bool currentEnable = outlines[0];
        SetOutlines(outlines, !currentEnable);
    }
}
/// <summary>
/// 
/// </summary>
public static class FishSelectionStatic
{
    /// <summary>
    /// 
    /// </summary>
    public static FishSelection fishSelection
    {
        get
        {
            if (_fishSelection == null) _fishSelection = GameObject.FindObjectOfType<FishSelection>();
            return _fishSelection;
        }
        set { _fishSelection = value; }
    }
    /// <summary>
    /// 
    /// </summary>
    static FishSelection _fishSelection;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="select"></param>
    public static void SetLastSelect(Transform select)
    {
        fishSelection.lastSelect = select;
    }
}