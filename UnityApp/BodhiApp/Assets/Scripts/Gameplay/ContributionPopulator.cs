using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContributionPopulator : ItemPopulator
{
    override public Coroutine GetItems(System.Action<List<ListItem>> callback)
    {
        return API.GetSingleton().GetContributionsList(PlayerPrefs.GetString("UserId"), (err, text) =>
            {
                List<ListItem> listItems = new List<ListItem>();
                ItemListResult result = JsonUtility.FromJson<ItemListResult>(text);
                for (int i = 0; i < result.result.Count; ++i)
                {
                    Color col = ColorByCategory.GetSingleton().ResolveColor(result.result[i]._id);
                    if(!result.result[i].validated)
                    {
                        col = Color.gray;
                    }
                    listItems.Add(new ListItem(result.result[i]._id, col, result.result[i].content));
                }
                callback(listItems);
            }
        );
    }

    override public void DeleteItemCallback(string id)
    {
        API.GetSingleton().DeleteComment(PlayerPrefs.GetString("UserId"), id);
    }
}