using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Skillsheet : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class KeyValuePair
    {
        /// <summary>
        /// 
        /// </summary>
        public Skillsheet skillsheet;
        /// <summary>
        /// 
        /// </summary>
        public string key;
        /// <summary>
        /// 
        /// </summary>
        public string tooltip;        
        /// <summary>
        /// 
        /// </summary>
        public string valueString;
        /// <summary>
        /// 
        /// </summary>
        ///[HideInInspector]
        [HideInInspector]
        public Component component;
        /// <summary>
        /// 
        /// </summary>
        [HideInInspector]
        public FieldInfo fieldInfo;
        /// <summary>
        /// 
        /// </summary>
        public KeyValuePair() { }
        /// <summary>
        /// 
        /// </summary>
        public KeyValuePair(
            Skillsheet skillsheet,
            Component component = null,
            FieldInfo fieldInfo = null)            
        {
            this.skillsheet = skillsheet;
            this.component = component; this.fieldInfo = fieldInfo;
            this.key = fieldInfo.Name;
            this.tooltip = fieldInfo.GetCustomAttribute<TooltipAttribute>().tooltip;
            this.valueString = fieldInfo.GetValue(component).ToString();            
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ListKeyValue
    {
        /// <summary>
        /// 
        /// </summary>
        public List<KeyValuePair> list;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KeyValuePair GetKeyValuePair(string key)
        {
            return list.Find((x) => x.key == key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToJson() { return JsonUtility.ToJson(this); }
    }
    /// <summary>
    /// 
    /// </summary>
    public ListKeyValue skillsheets;
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("RefreshFromAll")]
    public void RefreshFromAll()
    {
        this.skillsheets.list = new List<KeyValuePair>();
        Component[] myComponents = GetComponents(typeof(Component));
        foreach (Component myComp in myComponents)
        {
            Type myObjectType = myComp.GetType();
            FieldInfo[] fields = myObjectType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] properties = myObjectType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fields)
            {
                FishSkillAttribute fishskillAttribute = fieldInfo.GetCustomAttribute<FishSkillAttribute>();
                TooltipAttribute tooltipAttribute = fieldInfo.GetCustomAttribute<TooltipAttribute>();

                if (fishskillAttribute != null && tooltipAttribute != null)
                {                    
                    this.skillsheets.list.Add(new KeyValuePair(this, myComp, fieldInfo));
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("ChangeForAll")]
    public void ChangeForAll()
    {
        foreach (KeyValuePair keyValuePair in this.skillsheets.list)
        {
            keyValuePair.fieldInfo.SetValue(keyValuePair.component, Convert.ChangeType(keyValuePair.valueString, keyValuePair.fieldInfo.GetValue(keyValuePair.component).GetType()));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("DebugSkill")]
    public void DebugSkill()
    {
        Debug.Log(this.skillsheets.ToJson());
    }
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        RefreshFromAll();
    }
}
