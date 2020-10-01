using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class Hungry : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public bool isDebug = false;
    /// <summary>
    /// 
    /// </summary>
    public FishKoi fishKoi { get { if (_fishKoi == null) _fishKoi = GetComponentInParent<FishKoi>(); return _fishKoi; } }
    /// <summary>
    /// 
    /// </summary>
    FishKoi _fishKoi;
    /// <summary>
    /// 
    /// </summary>
    public float full
    {
        get { return _full; }
        set { HandleFull(value); }
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float _full = 1f;
    /// <summary>
    /// 
    /// </summary>
    void HandleFull(float value)
    {
        if (value < 0) value = 0;
        if (value > 1f) value = 1f;
        _full = value;
        bar.fillAmount = _full;
    }
    /// <summary>
    /// 
    /// </summary>
    public Image bar;
    /// <summary>
    /// 
    /// </summary>
    public float hungryStep = 1f;
    /// <summary>
    /// 
    /// </summary>
    public float hungryTimeStep = 1f;
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        full = 1f;
        hungryStep = Random.Range(0.01f, 0.2f);
        hungryTimeStep = Random.Range(0.5f, 5f);

        StartCoroutine(c_BalanceUI());
        StartCoroutine(c_BeingHungry());
    }
    /// <summary>
    /// 
    /// </summary>
    public void Restart()
    {
        StopAllCoroutines();
        Start();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_BalanceUI()
    {
        while (true)
        {
            UILookAtCamera();
            yield return new WaitForEndOfFrame();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_BeingHungry()
    {
        while (true)
        {
            full -= hungryStep;
            if (full < autoSearchFoodAt && closeFood == null)
            {
                SearchFood();
                if (closeFood != null)
                {
                    fishKoi.Wander(closeFood, this.hungrySpeedMultiple);
                    if (isDebug)
                        Debug.Log(fishKoi.gameObject.name + " looking for " + closeFood.gameObject.name + " speedmultiple " + hungrySpeedMultiple);
                }
            }
            if (full <= 0)
            {
                fishKoi.Die();
                yield break;
            }
            yield return new WaitForSeconds(hungryTimeStep);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("LookAtCamera")]
    public void UILookAtCamera()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
    }
    /// <summary>
    /// 
    /// </summary>
    public float distanceSearchFood = 1f;
    /// <summary>
    /// 
    /// </summary>
    public float autoSearchFoodAt = .5f;
    /// <summary>
    /// 
    /// </summary>
    public GameObject closeFood;
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("SearchFood")]
    public void SearchFood()
    {
        List<FishFood> foods = FindObjectsOfType<FishFood>().ToList();
        if (foods.Count == 0) return;

        for (int i = 0; i < foods.Count; i++)
            if (GetDistanceToFood(foods[i]) > distanceSearchFood)
            {
                foods.RemoveAt(i);
                i--;
            }

        foods.Sort((a, b) => GetDistanceToFood(a).CompareTo(GetDistanceToFood(b)));
        if (closeFood != foods[0].gameObject) closeFood = foods[0].gameObject;

        if (closeFood != null && isDebug)
            Debug.Log(fishKoi.gameObject.name + " found food " + closeFood.gameObject.name);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="food"></param>
    /// <returns></returns>
    float GetDistanceToFood(FishFood food)
    {
        return (food.transform.position - fishKoi.transform.position).sqrMagnitude;
    }
    /// <summary>
    /// 
    /// </summary>
    public float hungrySpeedMultiple = 5f;
}
