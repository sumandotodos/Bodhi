using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Topic
{
    public string name;
    public FGTable[] tables;
}

public class ContentsManager : MonoBehaviour
{
    static ContentsManager contentsManager = null;

    public Topic[] topic;
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
        double r = random.NextDouble();
        int to = Mathf.FloorToInt( (float)(((double)topic.Length) *  r) );
        r = random.NextDouble();
        int ta = Mathf.FloorToInt((float)(((double)topic[to].tables.Length) * r));
        return topic[to].tables[ta];
    }
}