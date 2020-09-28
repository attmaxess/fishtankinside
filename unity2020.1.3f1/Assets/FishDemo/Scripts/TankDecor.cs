using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
/// <summary>
/// 
/// </summary>
public class TankDecor : Singleton<TankDecor>
{
    /// <summary>
    /// 
    /// </summary>
    public List<GameObject> templates;
    /// <summary>
    /// 
    /// </summary>
    public void PlaceRandom()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        StartCoroutine(c_CheckNewArPlaneAndFetchOrigin());
    }
    /// <summary>
    /// 
    /// </summary>
    List<ARPlane> arPlanes = new List<ARPlane>();
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_CheckNewArPlaneAndFetchOrigin()
    {
        while (true)
        {
            arPlanes = FindObjectsOfType<ARPlane>().ToList();
            foreach (ARPlane aR in arPlanes)
            {
                OriginalDecor originalDecor = aR.gameObject.GetComponent<OriginalDecor>();

                if (aR.GetComponent<OriginalDecor>() == null)
                {
                    originalDecor = aR.gameObject.AddComponent<OriginalDecor>();
                    originalDecor.SelfBirth(templates[Random.Range(0, templates.Count)]);
                }
                else if (originalDecor.decor == null)
                {
                    originalDecor.SelfBirth(templates[Random.Range(0, templates.Count)]);
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
