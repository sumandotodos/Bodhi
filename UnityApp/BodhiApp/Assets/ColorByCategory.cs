using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorByCategoryItem
{
    public Color color;
    public int Category;
}

public class ColorByCategory : MonoBehaviour
{
    static ColorByCategory instance;

    public List<ColorByCategoryItem> CategoryColors;

    private void Awake()
    {
        instance = this;
    }

    public static ColorByCategory GetSingleton()
    {
        return instance;
    }

    public Color ResolveColor(int cat)
    {
        int Index = FGUtils.findInList<ColorByCategoryItem>(CategoryColors, (item) => { return item.Category == cat; });
        if (Index != -1)
        {
            return CategoryColors[Index].color;
        }
        else return Color.white;
    }

    public Color ResolveColor(string id)
    {
        string[] fields = id.Split(':');
        int cat;
        int.TryParse(fields[0], out cat);
        return ResolveColor(cat);
    }
}
