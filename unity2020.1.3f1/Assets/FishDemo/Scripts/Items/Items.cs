using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Items : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public Transform uiItemList;
    /// <summary>
    /// 
    /// </summary>
    public List<Texture2D> textures;
    /// <summary>
    /// 
    /// </summary>
    public void ToggleUIItemList()
    {
        uiItemList.gameObject.SetActive(!uiItemList.gameObject.activeSelf);
    }
}
