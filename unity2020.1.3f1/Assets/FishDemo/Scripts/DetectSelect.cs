using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class DetectSelect : MonoBehaviour
{
    /* -- Check if fish is selected --  */
    /// <summary>
    /// 
    /// </summary>
    public bool isSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; Output(); }
    }
    /// <summary>
    /// 
    /// </summary>
    [Header("Check if selected")]
    [SerializeField] bool _isSelected = false;
    /// <summary>
    /// 
    /// </summary>
    private void OnMouseDown()
    {
        isSelected = true;
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnMouseEnter()
    {
        isSelected = true;
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnMouseUp()
    {
        isSelected = false;
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnMouseExit()
    {
        isSelected = false;
    }
    /// <summary>
    /// 
    /// </summary>
    public Transform output;
    /// <summary>
    /// 
    /// </summary>
    void Output()
    {
        FishSelectionStatic.SetLastSelect(output);
    }
}
