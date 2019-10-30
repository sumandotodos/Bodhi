﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListController : MonoBehaviour
{
    public bool HeaderFrameMustBeColored = false;
    public bool AllFramesMustBeColoredNormal = false;

    List<Slab> slabs;

    public Color HeaderSlabColor = Color.magenta;
    public Color NormalSlabColor = Color.yellow;

    public int GetNumberOfSlabs()
    {
        return slabs.Count;
    }

    public void AddSlab(Slab slab)
    {
        if (HeaderFrameMustBeColored)
        {
            if (slabs.Count == 0)
            {
                slab.SetFrameColor(HeaderSlabColor);
            }
            else
            {
                slab.SetFrameColor(NormalSlabColor);
            }
        }
        else if(AllFramesMustBeColoredNormal)
        {
            slab.SetFrameColor(NormalSlabColor);
        }
        slabs.Add(slab);
    }

    // Start is called before the first frame update
    void Awake()
    {
        slabs = new List<Slab>();
    }

    public Slab GetSlab(int index)
    {
        return slabs[index];
    }

    public void SwapSlabs(int i, int j)
    {
        Slab TempSlab = slabs[i];
        slabs[i] = slabs[j];
        slabs[i].Index = i;
        slabs[j] = TempSlab;
        slabs[j].Index = j;
        if(HeaderFrameMustBeColored)
        { 
            if (i == 0 || j == 0)
            {
                slabs[j].SetFrameColor(HeaderSlabColor);
                slabs[i].SetFrameColor(NormalSlabColor);
            }
        }
    }

    public float GetTotalHeight()
    {
        float Total = 0.0f;
        foreach(Slab s in slabs)
        {
            Total += s.GetEffectiveHeight();
        }
        return Total;
    }

    public void DismissItem(int index)
    {
        Slab slabToRemove = slabs[index];
        float HeightToRemove = slabToRemove.GetHeight();
        float EffectiveHeight = slabToRemove.Adjust(HeightToRemove);//(HeightToRemove + 15.0f + HeightToRemove / 6.0f);
        slabs.RemoveAt(index);
        Destroy(slabToRemove.gameObject);
        StartCoroutine(DismissCoroutine(index, EffectiveHeight));
        for(int i = index; i < slabs.Count; ++i)
        {
            slabs[i].Index--;
        }
    }

    IEnumerator DismissCoroutine(int StartIndex, float EffectiveHeight)
    {
        for (int i = StartIndex; i < slabs.Count; ++i)
        {
            slabs[i].GetComponent<Magnetor>().DisplaceTargetPosition(new Vector2(0, EffectiveHeight));
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void listIds()
    {
        for(int i = 0; i < slabs.Count; ++i)
        {
            //Debug.Log("<color=red>"+slabs[i].id+"</color>");
        }
    }
}
