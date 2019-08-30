using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string _id;
    public string _userid;
    public int upvotes;
    public int downvotes;
    public int views;
    public int favoritized;
    public string type;
    public string content;
    public bool validated;
}

[System.Serializable]
public class ItemListResult
{
    public List<Item> result;
}