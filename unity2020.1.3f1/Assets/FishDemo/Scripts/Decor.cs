using System.Collections;
using UnityEngine;
/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Decor : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public Rigidbody rbody
    {
        get { if (_rbody == null) _rbody = GetComponent<Rigidbody>(); return _rbody; }
        set { _rbody = value; }
    }
    /// <summary>
    /// 
    /// </summary>
    Rigidbody _rbody;
    // Flag to track whether an obstacle has been detected.
    public bool obstacleDetected = false;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float distToCheckGround = 0f;
    /// <summary>
    /// 
    /// </summary>
    public bool lastCheckIsGround = false;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float distToGround = 0f;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] Vector3 offset = Vector3.zero;
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsGround()
    {
        return Physics.Raycast(rbody.worldCenterOfMass + offset, -Vector3.up, distToGround);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool HasGround()
    {
        return Physics.Raycast(rbody.worldCenterOfMass + offset, -Vector3.up, distToCheckGround);
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
    void DrawDebugAids()
    {
        if (!isDrawDebugAids) return;

        Color rayHasGround = Color.green;
        Debug.DrawRay(rbody.worldCenterOfMass + offset, -Vector3.up * (distToCheckGround), rayHasGround);

        Color rayIsGround = Color.red;
        Debug.DrawRay(rbody.worldCenterOfMass + offset, -Vector3.up * (distToGround), rayIsGround);
    }
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        rbody.useGravity = false;
        obstacleDetected = HasGround();
        if (!obstacleDetected) SelfDestroy();
        else
        {
            rbody.useGravity = true;
            lastCheckIsGround = false;
            StartCoroutine(CheckGroundedOnce());
        }
    }
    /// <summary>
    /// 
    /// </summary>
    IEnumerator CheckGroundedOnce()
    {
        Vector3 posStart = transform.position;
        while (!lastCheckIsGround)
        {
            if ((transform.position - posStart).sqrMagnitude > distToCheckGround) SelfDestroy();
            lastCheckIsGround = IsGround();
            if (lastCheckIsGround)
            {
                isDrawDebugAids = false;
            }
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("CurrentHasGround")]
    public void CurrentHasGround()
    {
        obstacleDetected = HasGround();
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("CurrentIsGround")]
    public void CurrentIsGround()
    {
        lastCheckIsGround = IsGround();
    }
    /// <summary>
    /// 
    /// </summary>
    public void SelfDestroy()
    {
        StopAllCoroutines();
        if (Application.isPlaying) DestroyImmediate(this.gameObject);
        else Destroy(this.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    [Header("Original Decor")]
    public Vector3 originPositionOffset = Vector3.zero;
    /// <summary>
    /// 
    /// </summary>
    public Vector3 originScale = Vector3.one;
    /// <summary>
    /// 
    /// </summary>
    public Quaternion originQuaternion = Quaternion.identity;
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("DebugTransform")]
    public void DebugTransform()
    {
        Debug.Log("Global Quaternion " + transform.rotation);
        Debug.Log("Lossy Scale " + transform.lossyScale);
    }
}