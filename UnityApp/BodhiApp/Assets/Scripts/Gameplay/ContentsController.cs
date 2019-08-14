using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentsController : MonoBehaviour
{

    public string DefaultCategory;
    public string DefaultTopic;

    public FGTable contentsTable;

    public Text contentsText;

    public Favoritizer favoritizer;

    bool CategoryHasHeader;

   public bool isShowingText;

    string triId;
    string tetraId;

    static ContentsController contentsController = null;

    private void Awake()
    {
        if (contentsController == null)
        {
            contentsController = this;
        }
    }

    private void Start()
    {
        contentsText.text = "";
        //ChooseTopic();
        isShowingText = false;
    }


    public static ContentsController GetSingleton()
    {
        return contentsController;
    }

    public int ChooseTopic()
    {
        string ContentType = PlayerPrefs.GetString("ContentType");
        string ContentTopic = PlayerPrefs.GetString("ContentTopic");
        if(ContentType == "")
        {
            ContentType = DefaultCategory;
        }
        if(ContentTopic == "")
        {
            ContentTopic = DefaultTopic;
        }
        triId = ContentsManager.GetSingleton().GetCategoryTopicId(ContentType, ContentTopic);
        contentsTable = ContentsManager.GetSingleton().GetCategoryTopicTable(triId); //ContentType, ContentTopic);
        CategoryHasHeader = ContentsManager.GetSingleton().CategoryHasHeader(ContentType);
        return CategoryHasHeader ? contentsTable.nRows()-1 : contentsTable.nRows();
    }

    public int NumberOfBichos()
    {
        return contentsTable.nRows();
    }

    public string GetHeader()
    {
        if(CategoryHasHeader)
        {
            return (string)contentsTable.getElement(0, 0);
        }
        else
        {
            return "";
        }
    }

    public void PrepareNextText()
    {
        isShowingText = true;
        int Row = 0;
        do
        {
            Row = contentsTable.getNextRowIndex();
        } while ((Row == 0) && CategoryHasHeader);
        contentsText.text = 
            (string)contentsTable.getElement(0, Row)
            + "\n\n";
        tetraId = triId + ":" + Row;
        favoritizer.ResetIcon();
        favoritizer.ContentId = tetraId;
        API.GetSingleton().IsFavorite(PlayerPrefs.GetString("UserId"), tetraId, (isFav) =>
        {
            if(isFav)
            {
                favoritizer.SetIsFavorite();
            }
        });
    }

    public string RetrieveId()
    {
        return tetraId;
    }

}
