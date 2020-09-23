using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class FishInfo : Singleton<FishInfo>
{
    /// <summary>
    /// 
    /// </summary>
    public ScrollRect scrollrect;
    /// <summary>
    /// 
    /// </summary>
    public Transform viewport;
    /// <summary>
    /// 
    /// </summary>
    public Transform content;
    /// <summary>
    /// 
    /// </summary>
    public Transform btnInfoPrefab;
    /// <summary>
    /// 
    /// </summary>
    public Transform contentPrefab;
    /// <summary>
    /// 
    /// </summary>
    public void DeleteAllInfo()
    {
        StartCoroutine(c_DeleteAllInfo());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_DeleteAllInfo()
    {
        if (Application.isPlaying) DestroyImmediate(content.gameObject);
        else Destroy(content.gameObject);
        content = Instantiate(contentPrefab, viewport);
        content.gameObject.SetActive(true);
        content.localPosition = Vector3.zero;
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    public void CreateInfo(Skillsheet.ListKeyValue data)
    {
        StartCoroutine(c_CreateInfo(data));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    IEnumerator c_CreateInfo(Skillsheet.ListKeyValue data)
    {
        foreach (Skillsheet.KeyValuePair valuePair in data.list)
        {
            FishInfoComponent fishInfoComponent = Instantiate(btnInfoPrefab.gameObject, content).GetComponent<FishInfoComponent>();
            fishInfoComponent.gameObject.SetActive(true);
            fishInfoComponent.currentInfo = valuePair;                
        }
        yield break;
    }    
}
