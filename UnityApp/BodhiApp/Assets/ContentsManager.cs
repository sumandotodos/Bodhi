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

    public FGTable ChooseTopic()
    {
        int index = Random.Range(0, category.Count);
        return ChooseTopic(category[index].name);
    }

    public FGTable ChooseTopic(string catName)
    {
        int CatIndex = FGUtils.findInList<Category>(category, (e) => (e.name == catName));
        if (CatIndex != -1)
        {
            List<Topic> topic = category[CatIndex].topics;
            double r = random.NextDouble();
            int to = Mathf.FloorToInt((float)(((double)topic.Count) * r));
            r = random.NextDouble();
            int ta = Mathf.FloorToInt((float)(((double)topic[to].tables.Count) * r));
            return topic[to].tables[ta];
        }
        else return null;
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