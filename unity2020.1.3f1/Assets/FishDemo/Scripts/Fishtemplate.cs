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
    public void DuplicateAll()
    {
        StartCoroutine(c_DuplicateAll());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator c_DuplicateAll()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (GameObject fish in templates)
            {
                Duplicate(fish);
                yield return new WaitForSeconds(2.5f);
            }
        }
        yield break;
    }    
    /// <summary>
    /// 
    /// </summary>
    public void Duplicate(GameObject tr)
    {
        FishKoi newFish = GameObject.Instantiate(tr.gameObject, null).GetComponent<FishKoi>();
        newFish.transform.localScale = newFish.originScale;
        newFish.transform.SetParent(transform);
        newFish.Revive();

        Hungry hungry = newFish.gameObject.GetComponentInChildren<Hungry>();
        hungry.full = 1;

        newFish.transform.position = Camera.main.transform.position + Camera.main.transform.forward / 10f;
        newFish.SetOffAllOutlines();
    }
}
