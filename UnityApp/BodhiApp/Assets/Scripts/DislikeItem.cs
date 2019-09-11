using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DislikeItem : MonoBehaviour
{
    public Slab slab;
    ItemsController itemsController;

    private void Awake()
    {
        itemsController = FindObjectOfType<ItemsController>();
    }

    public void Dislike()
    {
        itemsController.Dislike(slab.id, slab.Index);
    }
}
