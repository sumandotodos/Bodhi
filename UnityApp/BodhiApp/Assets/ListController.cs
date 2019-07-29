using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListController : MonoBehaviour
{

    List<Slab> slabs;

    public int GetNumberOfSlabs()
    {
        return slabs.Count;
    }

    public void AddSlab(Slab slab)
    {
        slabs.Add(slab);
    }

    // Start is called before the first frame update
    void Awake()
    {
        slabs = new List<Slab>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        float EffectiveHeight = (HeightToRemove + 15.0f + HeightToRemove / 6.0f);
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
}
