using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonProfilePopulator : ItemPopulator
{
    public GameObject HeaderPrefab;
    public GameObject ContributionPrefab;
    public GameObject PersonsFavoritePrefab;
    public GameObject FollowSlab;
    public GameObject EmptySlab;

    public ContentsManager contentsManager;

    override public Coroutine GetItems(System.Action<List<ListItem>> callback)
    {

        return StartCoroutine(PopulateProfileContribsAndFavoritesCoroutine(callback));

    }

    IEnumerator PopulateProfileContribsAndFavoritesCoroutine(System.Action<List<ListItem>> Callback)
    {
        List<ListItem> listItems = new List<ListItem>();
        string OtherUserHandle = PlayerPrefs.GetString("OtherUserHandle");
        string OtherUserId = PlayerPrefs.GetString("OtherUserId");

        // Follow / Unfollow
        listItems.Add(new ListItem(OtherUserId, Color.grey, "", "", "", FollowSlab));


        // Profile header ...
        listItems.Add(new ListItem("", Color.grey, "Perfil de " + OtherUserHandle, "", "", HeaderPrefab));//, 120.0f));
        // ... and profile
        yield return API.GetSingleton().GetProfile(OtherUserId, (err, profile) =>
        {
            if (profile.about == "")
            {
                listItems.Add(new ListItem("", Color.gray, "Sin perfil", "", "", EmptySlab));
            }
            else
            {
                listItems.Add(new ListItem(OtherUserId, Color.cyan, profile.about, "", "", SlabPrefab));
            }
        });


        // Contributions header ...
        listItems.Add(new ListItem("", Color.grey, "Contribuciones de " + OtherUserHandle, "", "", HeaderPrefab));
        // ... and contributions
        yield return API.GetSingleton().GetContributionsList(OtherUserId, (err, result) =>
        {
            if (result.result.Count == 0)
            {
                listItems.Add(new ListItem("", Color.gray, "No hay contribuciones", "", "", EmptySlab));
            }
            else
            {
                for (int i = 0; i < result.result.Count; ++i)
                {
                    Color col = ColorByCategory.GetSingleton().ResolveColor(result.result[i]._id);
                    if (!result.result[i].validated)
                    {
                        col = Color.gray;
                    }
                    listItems.Add(new ListItem(result.result[i]._id, col, result.result[i].content, "", "", ContributionPrefab));
                }
            }
        });

        // Favorites headers ...
        listItems.Add(new ListItem("", Color.grey, "Favoritos de " + OtherUserHandle, "", "", HeaderPrefab));
        // ... and favorites
        List<string> idsToLoad = null;
        yield return API.GetSingleton().GetFavoritesList(OtherUserId, (err, result) =>
        {
            idsToLoad = result.favorites;
        });
        if (idsToLoad.Count == 0)
        {

            listItems.Add(new ListItem("", Color.gray, "Ningún favorito", "", "", EmptySlab));
        }
        else
        {
            foreach (string id in idsToLoad)
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
                listItems.Add(new ListItem(id, col, content, "", "", PersonsFavoritePrefab));
            }
        }

        // With all items ready, callback(listItems)
        Callback(listItems);
    }

    public override void PostInstancing()
    {
        FollowSlab fSlab = FindObjectOfType<FollowSlab>();
        if (fSlab != null)
        {
            fSlab.SetFollowState(FollowState.Following);
        }
    }

    override public void DeleteItemCallback(string id)
    {
       
    }

    public void TouchOnAnswer(int index)
    {

    }
}
