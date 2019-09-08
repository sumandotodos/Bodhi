using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FavItem
{
    public string id;
    public Color color;

    public FavItem(string _id, Color _color)
    {
        id = _id;
        color = _color;
    }
}

[System.Serializable]
public class ListItem
{
    public string id;
    public Color color;
    public string content;

    public ListItem(string _id, Color _color, string _content)
    {
        id = _id;
        color = _color;
        content = _content;
    }
}

[System.Serializable]
public class Favorites_REST
{
    public List<string> favorites;
}

public class ItemsController : MonoBehaviour
{
    public static ItemsController instance;
    //public FavItem[] favItems;

    public ContentsManager contentsManager;
    public DragController dragController;
    public ListController listController;
    public ItemPopulator itemPopulator;

    public float MinSlabHeight = 200.0f;

    public Transform SlabsParent;
    public Transform SlabsScroll;
    GameObject SlabPrefab;
    public UIFader fader;
    float CurrentDestinationY = 665.0f;

    void Awake()
    {
        instance = this;
    }

    public static ItemsController GetSingleton()
    {
        return instance;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {

        SlabPrefab = itemPopulator.SlabPrefab;

        TypeOfContent contentFilter;

        contentFilter = Heart.FavTypeFromString(PlayerPrefs.GetString("FavoriteType"));

        fader.Start();
        yield return new WaitForSeconds(0.1f);
        List<ListItem> listItems = new List<ListItem>();
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);

        //--
        /*yield return API.GetSingleton().GetFavoritesList(PlayerPrefs.GetString("UserId"), (err, text) =>
        {
            Favorites_REST favs = JsonUtility.FromJson<Favorites_REST>(text);
            for(int i = 0; i < favs.favorites.Count; ++i)
            {
                Color col = ColorByCategory.GetSingleton().ResolveColor(favs.favorites[i]);
                listItems.Add(new ListItem(favs.favorites[i], col, ""));
            }
        });*/
        //--
        yield return itemPopulator.GetItems((_listItems) => { listItems = _listItems; });

        foreach(ListItem item in listItems)
        {
            if (ContentsManager.GetSingleton().TypeFromId(item.id) == contentFilter)
            {
                if (contentFilter != TypeOfContent.Message)
                {
                    if (!ContentsManager.IsLocalContent(item.id) && item.content == "")
                    {
                        yield return API.GetSingleton().GetItemContent(item.id, (err, text) =>
                        {
                            Item _item = JsonUtility.FromJson<Item>(text);
                            item.content = _item.content;
                            if (!_item.validated)
                            {
                                item.color = Color.gray;
                            }
                        });
                    }
                    else if (ContentsManager.IsLocalContent(item.id))
                    {
                        item.content = GetText(item);
                    }
                }

                listController.AddSlab(SpawnSlab(item));
                yield return new WaitForSeconds(0.15f);
            }
        }
        fader.fadeToTransparent();
    }

    Slab SpawnSlab(ListItem item)
    {
        Slab newSlab = SpawnSlab(new Vector2(11.0f, CurrentDestinationY), new Vector2(11.0f, CurrentDestinationY - 1850.0f));
        newSlab.SetColor(item.color);
        newSlab.Index = listController.GetNumberOfSlabs();
        newSlab.id = item.id;
        float h = 0.0f;
        if (item.content == "")
        {
            h = newSlab.SetText(GetText(item));
        }
        else
        {
            h = newSlab.SetText(item.content);
        }
        h = Mathf.Max(h, MinSlabHeight);
        newSlab.SetHeight(h);
        CurrentDestinationY -= Slab.Adjust(h);
        return newSlab;
    }



    string GetText(ListItem item)
    {
        if(item.id.StartsWith("_"))
        {
            return "Not yet";
        }
        else
        {
            string[] fields = item.id.Split(':');
            int cat;
            int topic;
            int table;
            int row;
            int.TryParse(fields[0], out cat);
            int.TryParse(fields[1], out topic);
            int.TryParse(fields[2], out table);
            int.TryParse(fields[3], out row);
            string res = contentsManager.RetrieveText(cat, topic, table, row);
            return res;
        }
    }

    public void NotifyDragEnd(int pos1, int pos2)
    {
        API.GetSingleton().ReorderFavorite(PlayerPrefs.GetString("UserId"), pos1, pos2);
    }

    Slab SpawnSlab(Vector2 destination, Vector2 initialPosition)
    {
        GameObject newGO = (GameObject)Instantiate(SlabPrefab);
        newGO.transform.SetParent(SlabsParent);
        newGO.transform.localScale = Vector3.one;
        newGO.transform.localPosition = initialPosition;
        newGO.GetComponent<Magnetor>().CurrentPosition = initialPosition;
        newGO.GetComponent<Magnetor>().Destination = destination;
        newGO.GetComponent<Magnetor>().Going = true;
        return newGO.GetComponent<Slab>();
    }

    public void TouchGoBackButton()
    {
        StartCoroutine(GoBackCoroutine());
    }

    IEnumerator GoBackCoroutine()
    {
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync("Planets");
    }

    // @TODO I'm not very fond of this redundancy, but it saves cycles...
    public void Dislike(string id, int index)
    {
        // do something with id and save to file
        listController.DismissItem(index);
        //API.GetSingleton().DestroyFavorite(PlayerPrefs.GetString("UserId"), id);
        itemPopulator.DeleteItemCallback(id);
    }

}
