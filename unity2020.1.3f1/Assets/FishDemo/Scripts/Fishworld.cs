using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
/// <summary>
/// 
/// </summary>
public class Fishworld : Singleton<Fishworld>
{
    /// <summary>
    /// 
    /// </summary>
    public Transform fishWorldBody;
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
#if UNITY_EDITOR
        EnableFishTank();
#elif UNITY_IOS || UNITY_ANDROID
        fishWorldBody.gameObject.SetActive(false);
        StartCoroutine(c_WaitForReadyToPlace());
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_WaitForReadyToPlace()
    {
        while (!fishWorldBody.gameObject.activeSelf)
        {
            ARPlane[] arPlanes = FindObjectsOfType<ARPlane>();
            if (arPlanes.Length == 0) yield return new WaitForSeconds(1f);            
            else
            {
                transform.position = arPlanes[0].transform.position;
                UserInterface.Instance.SetSceneStatusReady();
                fishWorldBody.position = Camera.main.transform.position + Camera.main.transform.forward / 10f;
                EnableFishTank();
                yield break;
            }
        }
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("PlaceFishtank")]
    public void PlaceFishtank()
    {
        ARPlane[] arPlanes = FindObjectsOfType<ARPlane>();
        Transform camera = FindObjectOfType<ARCameraManager>().transform;
        ARPlane closestARPlane = null;
        float closestDistance = float.MaxValue;
        foreach (ARPlane arPlane in arPlanes)
        {
            if ((arPlane.transform.position - camera.position).magnitude < closestDistance)
            {
                closestDistance = (arPlane.transform.position - camera.position).magnitude;
                closestARPlane = arPlane;
            }
        }
        if (closestARPlane == null) return;
        fishWorldBody.position = closestARPlane.centerInPlaneSpace;
        EnableFishTank();
    }
    /// <summary>
    /// 
    /// </summary>
    void EnableFishTank()
    {
        fishWorldBody.gameObject.SetActive(true);
        Fishtemplate.Instance?.DuplicateAll();
    }
}
