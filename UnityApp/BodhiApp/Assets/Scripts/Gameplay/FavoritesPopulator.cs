using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoritesPopulator : ItemPopulator
{
    override public Coroutine GetItems(System.Action<List<ListItem>> callback)
    {
        return API.GetSingleton().GetFavoritesList(PlayerPrefs.GetString("UserId"), (err, favs) =>
        {
            List<ListItem> listItems = new List<ListItem>();
            for (int i = 0; i < favs.favorites.Count; ++i)
            {
                Color col = ColorByCategory.GetSingleton().ResolveColor(favs.favorites[i]);
                listItems.Add(new ListItem(favs.favorites[i], col, "", "", "", "", SlabPrefab));
            }
            callback(listItems);
        });
    }

    override public void DeleteItemCallback(string id, string extra)
    {
        API.GetSingleton().DestroyFavorite(PlayerPrefs.GetString("UserId"), id);
    }
}
