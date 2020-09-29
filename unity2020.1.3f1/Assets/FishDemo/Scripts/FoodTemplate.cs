using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class FoodTemplate : Singleton<FoodTemplate>
{
    /// <summary>
    /// 
    /// </summary>
    public List<GameObject> templates;
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("Duplicate")]
    public void Duplicate()
    {
        for (int i = 0; i < 10; i++)
        {
            FishFood newFood = GameObject.Instantiate(templates[0], transform).GetComponent<FishFood>();
            newFood.gameObject.SetActive(true);
            newFood.transform.position = Camera.main.transform.position + Camera.main.transform.forward / 10f;
        }
    }
}
