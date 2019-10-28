using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlab : Slab
{

    public float SlabHeight = 340.0f;
    public float SlabMargin = 30.0f;

    public RawImage ri;

    public void SetImage(Texture2D image)
    {
        ri.texture = image;
    }

    public override void SetHeight(float _Height)
    {
        Height = SlabHeight;
        //base.SetHeight(_Height);
    }
    public override float Adjust(float H)
    {
        return SlabHeight + SlabMargin; //base.Adjust(H) + 70.0f;
    }
}
