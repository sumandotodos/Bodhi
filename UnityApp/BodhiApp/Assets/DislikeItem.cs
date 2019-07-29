using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DislikeItem : MonoBehaviour
{
    public Slab slab;
    FavsController favsController;

    private void Awake()
    {
        favsController = FindObjectOfType<FavsController>();
    }

    public void Dislike()
    {
        favsController.Dislike(slab.id, slab.Index);
    }
}
