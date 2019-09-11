using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmptySlab : Slab
{
    public float SlabHeight = 340.0f;
    public float SlabMargin = 30.0f;

    public override float SetText(string Text)
    {
        TextComponent.text = Text;
        return SlabHeight;
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
