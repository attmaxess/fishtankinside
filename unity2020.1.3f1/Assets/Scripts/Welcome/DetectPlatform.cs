using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectPlatform : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public float waitLoadPlatform = .5f;

    /// <summary>
    /// Scenepaths
    /// </summary>
    public string pcScene;
    public string androidScene;
    public string iosScene;    

    void Start()
    {
        Debug.Log(Application.platform);
        StartCoroutine(LoadSceneByPlatform());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSceneByPlatform()
    {
        yield return new WaitForSeconds(waitLoadPlatform);
#if UNITY_ANDROID
        SceneManager.LoadScene(androidScene);
#elif UNITY_IOS
        SceneManager.LoadScene(iosScene);
#elif UNITY_STANDALONE
        SceneManager.LoadScene(pcScene);
#endif
        yield break;
    }
}
