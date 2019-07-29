using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfContent {  Question, Idea };

[System.Serializable]
public class Category
{
    public string name;
    public TypeOfContent typeOfContent;
    public bool hasHeader;
    public List<Topic> topics;
    public Category(string _name)
    {
        name = _name;
        typeOfContent = TypeOfContent.Idea;
        hasHeader = false;
        topics = new List<Topic>();
    }
}

[System.Serializable]
public class Topic
{
    public string name;
    public List<FGTable> tables;
    public Topic(string _name)
    {
        name = _name;
        tables = new List<FGTable>();
    }
}



public class ContentsManager : MonoBehaviour
{
    static ContentsManager contentsManager = null;
    int CurrentCategory;

    public List<Category> category;
    System.Random random;

    private void Awake()
    {
        if (contentsManager == null)
        {
            contentsManager = this;
        }
    }

    public static ContentsManager GetSingleton()
    {
        return contentsManager;
    }

    private void Start()
    {
        random = new System.Random();
    }

    public string RetrieveText(int CatIndex, int TopicIndex, int TableIndex, int RowIndex)
    {
        Category cat = category[CatIndex];
        Topic top = cat.topics[TopicIndex];
        FGTable tab = top.tables[TableIndex];
        string res = (string)tab.getElement(0, RowIndex);
        return res;
        //return (string)category[CatIndex].topics[TopicIndex].tables[TableIndex].getElement(0, RowIndex);
    }

    public FGTable GetCategoryTopicTable(string Cat, string Top)
    {
        int CatIndex = FGUtils.findInList<Category>(category, (e) => (e.name == Cat));
        if (CatIndex != -1)
        {
           return GetTopicTable(CatIndex, Top);
        }
        return null;
    }

    public FGTable GetTopicTable(int CatIndex, string TopicName)
    {
        if (CatIndex != -1)
        {
            int TopicIndex = 0;
            List<Topic> topic = category[CatIndex].topics;
            if (TopicName != "")
            {
                TopicIndex = FGUtils.findInList<Topic>(topic, (e) => (e.name == TopicName));

            }
            if (TopicIndex == -1)
            {
                TopicIndex = 0;
            }
            double r = random.NextDouble();
            int ta = Mathf.FloorToInt((float)(((double)topic[TopicIndex].tables.Count) * r));
            return topic[TopicIndex].tables[ta];
        }
        else
        {
            return null;
        }
    }

    public bool CategoryHasHeader(string catName)
    {
        int CatIndex = FGUtils.findInList<Category>(category, (e) => (e.name == catName));
        return CategoryHasHeader(CatIndex);
    }
    public bool CategoryHasHeader(int index)
    {
        if (index != -1)
        {
            return category[index].hasHeader;
        }
        return false;

    }

    public string[] GetQuestionCategories()
    {
        List<string> result = new List<string>();

        return result.ToArray();
    }

    public string[] GetIdeaCategories()
    {
        List<string> result = new List<string>();

        return result.ToArray();
    }

    public void clear()
    {
        category = new List<Category>();
    }
}