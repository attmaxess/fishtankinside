using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class FoodDetectCollide : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public bool isDebug = false;
    /// <summary>
    /// 
    /// </summary>
    FishFood fishFood { get { if (_fishFood == null) _fishFood = GetComponentInParent<FishFood>(); return _fishFood; } }
    /// <summary>
    /// 
    /// </summary>
    FishFood _fishFood;
    /// <summary>
    /// 
    /// </summary>
    public float life = 5f;
    /// <summary>
    /// 
    /// </summary>
    public bool isDead = false;
    /// <summary>
    /// 
    /// </summary>
    public float nutrition = .01f;
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        nutrition = Random.Range(0.01f, 1f);
        life = Random.Range(20f, 60f);
        StartCoroutine(c_SelfDestroy());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_SelfDestroy()
    {
        while (true)
        {
            life--;
            if (life < 0) Destroy(fishFood.gameObject);
            yield return new WaitForSeconds(1f);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    void CheckHitByMouth(GameObject go)
    {
        if (isDebug) Debug.Log("hit " + go.gameObject.name);
        FishKoi fishKoi = FindFishKoi(go.gameObject);
        if (fishKoi != null) Feeding(fishKoi);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //CheckHitByMouth(other.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        CheckHitByMouth(other.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        CheckHitByMouth(collision.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision)
    {
        //CheckHitByMouth(collision.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    FishKoi FindFishKoi(GameObject go)
    {
        FishKoi fishKoi = go.GetComponentInChildren<FishKoi>();
        if (fishKoi != null) return fishKoi;
        fishKoi = go.GetComponentInParent<FishKoi>();
        return fishKoi;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fishKoi"></param>
    void Feeding(FishKoi fishKoi)
    {
        if (isDead) return;
        isDead = true;
        Hungry hungry = fishKoi.gameObject.GetComponentInChildren<Hungry>();
        if (hungry.full < 1f)
        {
            if (isDebug) Debug.Log("nutrition " + fishKoi.gameObject.name + " with " + nutrition);
            hungry.full += nutrition;
            Destroy(fishFood.gameObject);
        }
    }
}
