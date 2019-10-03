using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesPopulator : ItemPopulator
{
    override public Coroutine GetItems(System.Action<List<ListItem>> callback)
    {
        return API.GetSingleton().GetMessagesList(PlayerPrefs.GetString("UserId"), (err, text) =>
        {
            List<ListItem> listItems = new List<ListItem>();
            MessageListResult result = JsonUtility.FromJson<MessageListResult>(text);
            for (int i = 0; i < result.result.Count; ++i)
            {
                Color col = ColorFromType(result.result[i].type);
                string question = result.result[i].content;
                listItems.Add(new ListItem(result.result[i]._id, col, 
                    MakeContent(result.result[i]),
                    question,
                    result.result[i].extra,
                    SlabPrefab));
            }
            callback(listItems);
        }
        );
    }

    private Color ColorFromType(string type)
    {
        switch(type)
        {
            case "Connect Request":
                return new Color(0.2f, 0.6f, 0.95f, 1.0f);

            case "Performance Report":
                return new Color(0.7f, 0.6f, 0.55f, 1.0f);

            case "Question Answered":
                return new Color(0.25f, 0.92f, 0.65f, 1.0f);

        }
        return Color.gray;
    }

    private string MakeContent(Message msg)
    {
        switch(msg.type)
        {
            case "Connect Request":
                return "El usuario <color=white>" + msg.extra + " </color>quiere conectar contigo";

            case "Question Answered":
                if(ContentsManager.IsLocalContent(msg.contentid))
                {
                    msg.content = ContentsManager.GetSingleton().GetLocalContentFromId(msg.contentid);
                }
                return "El usuario <color=white>"+msg.fromuserid+"</color> ha contestado a tu pregunta <color=yellow>"+
                    msg.content
                    + "</color>";

            case "Performance Report":
                PerformanceResult result = JsonUtility.FromJson<PerformanceResult>(msg.extra);
                if(result.favorites > 0 && result.upvotes > 0)
                {
                    return
                        "Enhorabuena, tus aportaciones " +
                        pluralizeWord(result.upvotes, "le", "les") +
                        " han gustado a " +
                        pluralize(result.upvotes, "persona", "personas") +
                        ", y han sido marcadas como favoritas " +
                        pluralize(result.favorites, "vez", "veces");
                }
                else if (result.favorites > 0)
                {
                    return
                        "Enhorabuena, tus aportaciones han sido marcadas como favoritas " +
                        pluralize(result.favorites, "vez", "veces");
                }
                else if (result.upvotes > 0)
                {
                    return
                        "Enhorabuena, tus aportaciones " +
                        pluralizeWord(result.upvotes, "le", "les") +
                        " han gustado a " +
                        pluralize(result.upvotes, "persona", "personas");
                }
                return "";
        }
        return "";
    }

    private string pluralize(int amount, string stem, string plural)
    {
        if(amount == 1)
        {
            return amount + " " + stem;
        }
        else
        {
            return amount + " " + plural;
        }
    }

    private string pluralizeWord(int amount, string singular, string plural)
    {
        if (amount == 1)
        {
            return singular;
        }
        else
        {
            return plural;
        }
    }

    public override void DeleteItemCallback(string id)
    {
        API.GetSingleton().DeleteMessage(PlayerPrefs.GetString("UserId"), id);
    }
}
