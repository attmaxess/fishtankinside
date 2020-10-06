using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
/// <summary>
/// 
/// </summary>
public class TankSeaweed : Singleton<TankSeaweed>
{
    /// <summary>
    /// 
    /// </summary>
    public List<GameObject> seaweeds;
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
                    originalDecor.SelfBirthSeaweed(seaweeds[Random.Range(0, seaweeds.Count)]);
                }
                else if (originalDecor.seaweed == null)
                {
                    originalDecor.SelfBirthSeaweed(seaweeds[Random.Range(0, seaweeds.Count)]);
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("DuplicateRandomAt")]
    public void DuplicateRandom()
    {
        StartCoroutine(c_DuplicateRandom());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_DuplicateRandom()
    {
        List<Seaweed> seaweeds = FindObjectsOfType<Seaweed>().ToList();
        Seaweed seaweedRandom = seaweeds[Random.Range(0, seaweeds.Count)];
        if (seaweedRandom == null) yield break;
        if (offsetDirs.Count == 0) yield break;
        offsetCenter.position = seaweedRandom.transform.position + new Vector3(0, 0, 0.01f);
        CastAllRay();
        offsetDirs.Sort((a, b) => a.sqrMagnitude.CompareTo(b.sqrMagnitude));
        Vector3 shortDir = offsetDirs[0];
        Seaweed newSeaweed = Instantiate(seaweedRandom.gameObject, null).GetComponent<Seaweed>();
        newSeaweed.transform.position = offsetCenter.position + shortDir * 2.5f;
        TriggerManager newSeaweedTrigger = newSeaweed.GetComponent<TriggerManager>();
        while (newSeaweedTrigger.isTrigger())
        {
            newSeaweed.transform.Translate(shortDir.normalized);
            yield return new WaitForEndOfFrame();
        }
        newSeaweed.GetComponent<ReleaseConstraintDependTrigger>().StartRelease();
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("CastAllRay")]
    public void CastAllRay()
    {
        int layer_mask = LayerMask.GetMask("Seaweed");
        for (int i = 0; i < numberOfRay; i++)
        {
            Ray ray = new Ray(offsetCenter.position + offsetDirs[i] * 200f, -offsetDirs[i] * 200f);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, 200f, layer_mask);
            if (hitInfo.transform != null) offsetDirs[i] = hitInfo.point - offsetCenter.position;
            else offsetDirs[i] = offsetDirs[i].normalized;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnDrawGizmos()
    {
        DrawDebugAids();
    }
    /// <summary>
    /// Draws visual debug aids that can be seen in the editor viewport.
    /// </summary>
    [Header("DrawDebugAids")]
    public bool isDrawDebugAids = false;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] int numberOfRay = 5;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] Transform offsetCenter;
    /// <summary>
    /// 
    /// </summary>    
    [SerializeField] List<Vector3> offsetDirs;
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("CreateListQuaOffset")]
    public void CreateListQuaOffset()
    {
        CreateNewOffCenter();
        for (int i = 0; i < numberOfRay; i++)
        {
            Transform go = new GameObject().transform;
            go.position = offsetCenter.position + Vector3.forward;
            offsetCenter.Rotate(0, 360 / numberOfRay, 0);
            go.SetParent(offsetCenter);
            go.localScale = Vector3.one;
            go.localRotation = Quaternion.identity;
        }
        offsetCenter.localRotation = Quaternion.identity;
        offsetDirs = new List<Vector3>();
        for (int i = 0; i < numberOfRay; i++)
            offsetDirs.Add((offsetCenter.GetChild(i).position - offsetCenter.position).normalized);
        CreateNewOffCenter();
    }
    /// <summary>
    /// 
    /// </summary>
    void CreateNewOffCenter()
    {
        offsetCenter = transform.Find("offsetCenter");
        if (offsetCenter != null)
            if (Application.isPlaying) Destroy(offsetCenter.gameObject);
            else DestroyImmediate(offsetCenter.gameObject);
        offsetCenter = new GameObject("offsetCenter").transform;
        offsetCenter.SetParent(this.transform);
        offsetCenter.localPosition = Vector3.zero;
        offsetCenter.localScale = Vector3.one;
        offsetCenter.localRotation = Quaternion.identity;
    }
    /// <summary>
    /// 
    /// </summary>
    void DrawDebugAids()
    {
        if (!isDrawDebugAids) return;
        if (offsetCenter == null) return;

        Color rayHasGround = Color.magenta;
        for (int i = 0; i < offsetDirs.Count; i++)
        {
            Debug.DrawRay(offsetCenter.position + offsetDirs[i], -offsetDirs[i], rayHasGround);
        }
    }
}