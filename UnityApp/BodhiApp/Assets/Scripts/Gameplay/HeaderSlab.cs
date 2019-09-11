using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderSlab : Slab
{
    public float SlabHeight = 120.0f;
    public float SlabMargin = 30.0f;

    public override void SetHeight(float _Height)
    {
        _Height = SlabHeight;
        base.SetHeight(_Height);
    }
    public override float Adjust(float H)
    {
        return SlabHeight + SlabMargin; //base.Adjust(H) + 70.0f;
    }
}
