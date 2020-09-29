using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class Fishtemplate : Singleton<Fishtemplate>
{
    /// <summary>
    /// 
    /// </summary>
    public List<GameObject> templates;
    /// <summary>
    /// 
    /// </summary>
    public void Duplicate(Transform tr)
    {
        FishKoi newFish = GameObject.Instantiate(tr.gameObject, transform).GetComponent<FishKoi>();
        newFish.Revive();

        Hungry hungry = newFish.gameObject.GetComponentInChildren<Hungry>();
        hungry.full = 1;

        newFish.transform.position = Camera.main.transform.position + Camera.main.transform.forward / 10f;
        newFish.SetOffAllOutlines();
    }    
}
