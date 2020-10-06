using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(TriggerManager))]
public class RenderDependTrigger : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float onTriggerAlpha = 1f;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float offTriggerAlpha = 0;
    /// <summary>
    /// 
    /// </summary>
    float Alpha
    {
        get { return _Alpha; }
        set { HandleSetAlpha(value); }
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float _Alpha = 1f;
    /// <summary>
    /// 
    /// </summary>
    void HandleSetAlpha(float value)
    {
        _Alpha = value;
        foreach (Material _mat in _mats)
        {
            Color color = _mat.color;
            _mat.color = new Color(color.r, color.g, color.b, _Alpha);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("HandleCurrentAlpha")]
    public void HandleCurrentAlpha()
    {
        Alpha = _Alpha;
    }
    /// <summary>
    /// 
    /// </summary>
    TriggerManager triggerManager { get { if (_triggerManager == null) _triggerManager = GetComponent<TriggerManager>(); return _triggerManager; } }
    /// <summary>
    /// 
    /// </summary>
    TriggerManager _triggerManager;
    /// <summary>
    /// 
    /// </summary>
    List<Renderer> rens
    {
        get
        {
            //if (_rens == null || _rens.Count == 0) _rens = GetComponentsInChildren<Renderer>().ToList();
            if (_rens.Count == 0) Debug.Log("Gameobject has 0 ren.");
            return _rens;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    List<Renderer> _rens;
    /// <summary>
    /// 
    /// </summary>
    List<Material> mats
    {
        get
        {
            //if (_mats == null || _mats.Count == 0)
            //    foreach (Renderer ren in rens)
            //        foreach (Material mat in ren.materials)
            //            _mats.Add(mat);
            if (_mats.Count == 0) Debug.Log("Gameobject has 0 mat.");
            return _mats;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    List<Material> _mats;
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        _rens = GetComponentsInChildren<Renderer>().ToList();
        _mats = new List<Material>();
        foreach (Renderer _ren in _rens)
            foreach (Material mat in _ren.materials)
                _mats.Add(mat);

    }
    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        triggerManager.onTriggerListChange += HandleTriggersListChange;
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        triggerManager.onTriggerListChange -= HandleTriggersListChange;
    }
    /// <summary>
    /// 
    /// </summary>
    void HandleTriggersListChange()
    {
        if (_rens == null || _mats == null || _rens.Count == 0 || _mats.Count == 0) return;
        Alpha = triggerManager.isTrigger() ? offTriggerAlpha : onTriggerAlpha;
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("CollectDump")]
    public void CollectDump()
    {
        GC.Collect();
    }
}
