using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItem : MonoBehaviour
{
    DragController dragController;
    public Slab slab;

    void Awake()
    {
        dragController = FindObjectOfType<DragController>();
    }

    public void OnTouch()
    {
        dragController.BeginTouch(slab.Index);
    }

    public void OnRelease()
    {
        dragController.EndTouch(slab.Index);
    }
}
