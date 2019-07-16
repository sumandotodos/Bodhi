using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabItem
{
    public string name;
    public GameObject prefab;
}

public class PrefabSelector : MonoBehaviour
{

    public List<PrefabItem> Prefabs;

    public GameObject GetPrebafByPlayerPrefs()
    {
        return GetPrefabByName(PlayerPrefs.GetString("MinesweeperPrefab"));
    }

    public GameObject GetPrefabByIndex(int n)
    {
        return Prefabs[n].prefab;
    }

    public GameObject GetPrefabByName(string s)
    {
        int index = FGUtils.findInList<PrefabItem>(Prefabs, (item) => (item.name == s));
        if (index == -1)
        {
            Debug.Log("Warning: Prefab cell named \"" + s + "\" not found");
            return Prefabs[Prefabs.Count-1].prefab;
        }
        return Prefabs[index].prefab;
    }

}
