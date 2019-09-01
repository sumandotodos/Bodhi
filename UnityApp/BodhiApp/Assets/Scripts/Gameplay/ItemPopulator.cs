using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopulator : MonoBehaviour
{
    public GameObject SlabPrefab;

    virtual public Coroutine GetItems(System.Action<List<ListItem>> callback)
    {
        return null;
    }

    virtual public void DeleteItemCallback(string id)
    {

    }
}
