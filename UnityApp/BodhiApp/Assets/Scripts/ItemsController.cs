﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public string questionid;
    public string question;
    public string fromuserid;
    public string fromuserhandle;
    public string extra;
    public GameObject Prefab;
    //public float fixedSize = -1.0f;

    public ListItem(string _id, Color _color, string _content, string _questionid, string _question, string _extra, GameObject _Prefab)//, float FixedSize = -1.0f)
    {
        id = _id;
        color = _color;
        content = _content;
        Prefab = _Prefab;
        questionid = _questionid;
        question = _question;
        extra = _extra;
       // fixedSize = FixedSize;
    }
    public ListItem(string _id, string _fromuserid, Color _color, string _content, string _questionid, string _question, string _extra, GameObject _Prefab)//, float FixedSize = -1.0f)
    {
        id = _id;
        fromuserid = _fromuserid;
        color = _color;
        content = _content;
        Prefab = _Prefab;
        question = _question;
        questionid = _questionid;
        extra = _extra;
        // fixedSize = FixedSize;
    }
    public ListItem(string _id, string _fromuserid, string _fromuserhandle, Color _color, string _content, string _questionid, string _question, string _extra, GameObject _Prefab)//, float FixedSize = -1.0f)
    {
        id = _id;
        fromuserid = _fromuserid;
        color = _color;
        content = _content;
        Prefab = _Prefab;
        fromuserhandle = _fromuserhandle;
        question = _question;
        questionid = _questionid;
        extra = _extra;
        // fixedSize = FixedSize;
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
    public GameObject EmptyPrefab;
    public UIScaleFader DislikeConfirmMenu;

    public float MinSlabHeight = 200.0f;

    public Transform SlabsParent;
    public Transform SlabsScroll;

    public UIFader fader;
    public float CurrentDestinationY = 665.0f;

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

        TypeOfContent contentFilter;

        contentFilter = Heart.FavTypeFromString(PlayerPrefs.GetString("FavoriteType"));

        listController.HeaderFrameMustBeColored = (contentFilter == TypeOfContent.Question);

        fader.Start();
        yield return new WaitForSeconds(0.1f);
        List<ListItem> listItems = new List<ListItem>();
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);

        yield return itemPopulator.GetItems((_listItems) => { listItems = _listItems; });

        //@TODO the next block of code is a nightmare, refactor!!
        foreach(ListItem item in listItems)
        {
            if (contentFilter == TypeOfContent.Any || ContentsManager.GetSingleton().TypeFromId(item.id) == contentFilter)
            {
                if (contentFilter == TypeOfContent.Question || contentFilter == TypeOfContent.Idea)
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
            else
            {
                if(item.Prefab == EmptyPrefab)
                {
                    listController.AddSlab(SpawnSlab(item));
                    yield return new WaitForSeconds(0.15f);
                }
            }
        }

        itemPopulator.PostInstancing();

        fader.fadeToTransparent();
    }

    Slab SpawnSlab(ListItem item)
    {
        Slab newSlab = SpawnSlab(item.Prefab, 
            new Vector2(11.0f, CurrentDestinationY), 
            new Vector2(11.0f, CurrentDestinationY - 1850.0f));
        newSlab.SetColor(item.color);
        newSlab.Index = listController.GetNumberOfSlabs();
        newSlab.id = item.id;
        newSlab.fromuserid = item.fromuserid;
        newSlab.questionid = item.questionid;
        newSlab.question = item.question;
        newSlab.extra = item.extra;
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
       
        CurrentDestinationY -= newSlab.Adjust(h);
        return newSlab;
    }



    string GetText(ListItem item)
    {
        if(item.id == "")
        {
            return "";
        }
        else if (item.id.IndexOf(":") == -1)
        {
            return "";
        }
        else if(item.id.StartsWith("_"))
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

    Slab SpawnSlab(GameObject prefab, Vector2 destination, Vector2 initialPosition)
    {
        GameObject newGO = (GameObject)Instantiate(prefab);
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

    int ConfirmIndex;
    string ConfirmId;

    // @TODO I'm not very fond of this redundancy, but it saves cycles...
    // should renamed from Dislike to Dismiss, maybe....
    public void Dislike(string id, int index, bool Confirm)
    {
        if(Confirm)
        {
            DislikeConfirmMenu.scaleIn();
            ConfirmIndex = index;
            ConfirmId = id;
        }
        else
        {
            itemPopulator.DeleteItemCallback(id, listController.GetSlab(index).extra);
            listController.DismissItem(index);
        }

    }

    public void GoAheadWithDislike()
    {
        itemPopulator.DeleteItemCallback(ConfirmId,listController.GetSlab(ConfirmIndex).extra);
        DislikeConfirmMenu.scaleOut();
        listController.DismissItem(ConfirmIndex);
    }

}
