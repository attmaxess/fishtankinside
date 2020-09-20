using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class UserInterface : Singleton<UserInterface>
{
    /// <summary>
    /// 
    /// </summary>
    public TextMeshProUGUI txtSceneStatus;
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
#if UNITY_EDITOR
        txtSceneStatus.gameObject.SetActive(false);
#elif UNITY_IOS || UNITY_ANDROID
        txtSceneStatus.gameObject.SetActive(true);        
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    public void SetSceneStatusReady()
    {
        StartCoroutine(c_SetSceneStatusReady());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_SetSceneStatusReady()
    {
        txtSceneStatus.text = "Đã sẵn sàng";
        yield return new WaitForSeconds(2f);
        txtSceneStatus.gameObject.SetActive(false);
        yield break;
    }
}
