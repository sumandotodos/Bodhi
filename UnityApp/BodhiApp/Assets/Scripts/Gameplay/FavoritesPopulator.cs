using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoritesPopulator : ItemPopulator
{
    override public Coroutine GetItems(System.Action<List<ListItem>> callback)
    {
        return API.GetSingleton().GetFavoritesList(PlayerPrefs.GetString("UserId"), (err, text) =>
        {
            List<ListItem> listItems = new List<ListItem>();
            Favorites_REST favs = JsonUtility.FromJson<Favorites_REST>(text);
            for (int i = 0; i < favs.favorites.Count; ++i)
            {
                Color col = ColorByCategory.GetSingleton().ResolveColor(favs.favorites[i]);
                listItems.Add(new ListItem(favs.favorites[i], col, ""));
            }
            callback(listItems);
        });
    }
}
