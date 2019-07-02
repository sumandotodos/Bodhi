using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentsController : MonoBehaviour
{

    public FGTable contentsTable;

    public Text contentsText;



   public bool isShowingText;

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
        ChooseTopic();
        isShowingText = false;
    }


    public static ContentsController GetSingleton()
    {
        return contentsController;
    }

    public void ChooseTopic()
    {
        contentsTable = ContentsManager.GetSingleton().ChooseTopic();
    }

    public int NumberOfBichos()
    {
        return contentsTable.nRows();
    }

    public void PrepareNextText()
    {
        isShowingText = true;
        contentsText.text = "\n" +
            (string)contentsTable.getElement(0, contentsTable.getNextRowIndex())
            + "\n\n";
    }

}
