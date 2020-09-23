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
    public Transform trnTextSceneStatus;
    /// <summary>
    /// 
    /// </summary>
    public TextMeshProUGUI txtSceneStatus;
    /// <summary>
    /// 
    /// </summary>
    static readonly string textGettingReady = "Không gian AR cần được khởi tạo. \n" +
        "Hãy xoay camera xung quanh khắp phòng đi nào. \n" +
        "...Chưa sẵn sàng...";
    /// <summary>
    /// 
    /// </summary>
    static readonly string textReady = "Đã sẵn sàng.";
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        txtSceneStatus.text = textGettingReady;
#if UNITY_EDITOR
        trnTextSceneStatus.gameObject.SetActive(false);
#elif UNITY_IOS || UNITY_ANDROID
        trnTextSceneStatus.gameObject.SetActive(true);        
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
        txtSceneStatus.text = textReady;
        yield return new WaitForSeconds(2f);
        trnTextSceneStatus.gameObject.SetActive(false);
        yield break;
    }
}
