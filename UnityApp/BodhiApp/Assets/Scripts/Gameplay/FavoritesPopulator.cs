using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoritesPopulator : ItemPopulator
{
    public GameObject EmptyPrefab;
    public ContentsManager contentsManager;
    override public Coroutine GetItems(System.Action<List<ListItem>> callback)
    {
        TypeOfContent content = Heart.FavTypeFromString(PlayerPrefs.GetString("FavoriteType"));

        return API.GetSingleton().GetFavoritesList(PlayerPrefs.GetString("UserId"), (err, favs) =>
        {
            int numberOfActualItems = 0;
            List<ListItem> listItems = new List<ListItem>();
            for(int i = 0; i < favs.favorites.Count; ++i)
            {
                if (contentsManager.TypeFromId(favs.favorites[i]) == content)
                {
                    ++numberOfActualItems;
                }
            }

            if (numberOfActualItems > 0)
            {
                Debug.Log("Favorites is > 0: " + favs.favorites.Count);
                for (int i = 0; i < favs.favorites.Count; ++i)
                {
                    Color col = ColorByCategory.GetSingleton().ResolveColor(favs.favorites[i]);
                    listItems.Add(new ListItem(favs.favorites[i], col, "", "", "", "", SlabPrefab));
                }
            }
            else
            {
                Debug.Log("Favorites is == 0, adding emptyprefab");
                listItems.Add(new ListItem("", Color.gray, "Sin favoritos", "", "", "", EmptyPrefab));
            }
            callback(listItems);
        });
    }

    override public void DeleteItemCallback(string id, string extra)
    {
        API.GetSingleton().DestroyFavorite(PlayerPrefs.GetString("UserId"), id);
    }
}
