using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonProfilePopulator : ItemPopulator
{
    public GameObject HeaderPrefab;
    public GameObject ContributionPrefab;
    public GameObject PersonsFavoritePrefab;

    public ContentsManager contentsManager;

    override public Coroutine GetItems(System.Action<List<ListItem>> callback)
    {
        /*return API.GetSingleton().GetContributionsList(PlayerPrefs.GetString("UserId"), (err, text) =>
        {
            List<ListItem> listItems = new List<ListItem>();
            ItemListResult result = JsonUtility.FromJson<ItemListResult>(text);
            for (int i = 0; i < result.result.Count; ++i)
            {
                Color col = ColorByCategory.GetSingleton().ResolveColor(result.result[i]._id);
                if (!result.result[i].validated)
                {
                    col = Color.gray;
                }
                listItems.Add(new ListItem(result.result[i]._id, col, result.result[i].content, SlabPrefab));
            }
            callback(listItems);
        }
        );*/

        return StartCoroutine(PopulateProfileContribsAndFavoritesCoroutine(callback));

    }

    IEnumerator PopulateProfileContribsAndFavoritesCoroutine(System.Action<List<ListItem>> Callback)
    {
        List<ListItem> listItems = new List<ListItem>();
        string OtherUserHandle = PlayerPrefs.GetString("OtherUserHandle");
        string OtherUserId = PlayerPrefs.GetString("OtherUserId");

        /*
         *  listItems.Add(new ListItem("", Color.grey, "Perfil de " + OtherUserHandle, HeaderPrefab, 120.0f));
        listItems.Add(new ListItem("", Color.cyan, "Este es mi perfil, y es el más guay de todos los rincones tibetanos del mundo de la galaxia 4..... y me parece superguay tener un perfil así. si quieres conocerme ,yo qué sé....", SlabPrefab));
        listItems.Add(new ListItem("", Color.grey, "Contribuciones de " + OtherUserHandle, HeaderPrefab, 120.0f));
        listItems.Add(new ListItem("", Color.grey, "Favoritos de " + OtherUserHandle, HeaderPrefab, 120.0f));*/

        listItems.Add(new ListItem("", Color.grey, "Perfil de " + OtherUserHandle, HeaderPrefab, 120.0f));
        yield return API.GetSingleton().GetProfile(OtherUserId, (err, profile) =>
        {
            listItems.Add(new ListItem(OtherUserId, Color.cyan, profile.about, SlabPrefab));
        });
        listItems.Add(new ListItem("", Color.grey, "Contribuciones de " + OtherUserHandle, HeaderPrefab, 120.0f));
        yield return API.GetSingleton().GetContributionsList(OtherUserId, (err, result) =>
        {
            for (int i = 0; i < result.result.Count; ++i)
            {
                Color col = ColorByCategory.GetSingleton().ResolveColor(result.result[i]._id);
                if (!result.result[i].validated)
                {
                    col = Color.gray;
                }
                listItems.Add(new ListItem(result.result[i]._id, col, result.result[i].content, ContributionPrefab));
            }
        });
        listItems.Add(new ListItem("", Color.grey, "Favoritos de " + OtherUserHandle, HeaderPrefab, 120.0f));
        List<string> idsToLoad = null;
        yield return API.GetSingleton().GetFavoritesList(OtherUserId, (err, result) =>
        {
            idsToLoad = result.favorites;
        });
        foreach(string id in idsToLoad)
        {
            Color col = ColorByCategory.GetSingleton().ResolveColor(id);
            string content = "";
            if (ContentsManager.IsLocalContent(id))
            {
                content = contentsManager.GetLocalContentFromId(id);
            }
            else
            {
                yield return API.GetSingleton().GetItemContent(id, (err, cont) =>
                 {
                     content = cont;
                 });
            }
            listItems.Add(new ListItem(id, col, content, PersonsFavoritePrefab));
        }
        Callback(listItems);
    }

    override public void DeleteItemCallback(string id)
    {
       
    }

    public void TouchOnAnswer(int index)
    {

    }
}
