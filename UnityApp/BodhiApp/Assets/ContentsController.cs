using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentsController : MonoBehaviour
{
    public Animation UIanimation;
    public FGTable contentsTable;

    public Text contentsText;

    public string SpringOutAnimationName = "SpringUpOK";
    public string SpringInAnimationName = "SpringDown";
    public string HiddenAnimationName = "Hidden";

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
        UIanimation.Play(HiddenAnimationName);
        contentsText.text = "";
        ChooseTopic();
        isShowingText = false;
    }

    private void Update()
    {
        if(isShowingText && Input.GetMouseButtonDown(0))
        {
            UIanimation.Play(SpringInAnimationName);
            isShowingText = false;
            Raycaster.GetSingleton().SetActive(true);
        }
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

    public void BichoFound()
    {
        isShowingText = true;
        contentsText.text = (string)contentsTable.getElement(0,
            contentsTable.getNextRowIndex());
        UIanimation.Play(SpringOutAnimationName);
    }

}
