using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Fishworld : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public Transform fishWorldBody;

    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("PlaceFishtank")]
    public void PlaceFishtank()
    {
        Fishworld fishworld = FindObjectOfType<Fishworld>();
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
        fishworld.transform.position = closestARPlane.centerInPlaneSpace;
        fishWorldBody.gameObject.SetActive(true);
    }
}
