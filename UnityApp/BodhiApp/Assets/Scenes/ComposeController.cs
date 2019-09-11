using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComposeController : MonoBehaviour
{
    public UIFader fader;
    public UITextFader LabelFader;
    public UIScaleFader IdeaButtonScaler;
    public UIScaleFader QuestionButtonScaler;
    public UIMoverTwoPoints scrollMover;
    public TMPro.TMP_InputField inputField;
    public Text notebookText;

    public Color IdeaColor;
    public Color QuestionColor;

    public Image NotebookImage;

    public string ContentPrefix;

    public float HorizontalDisplacement = 1152.0f;
    public GameObject IdeasButtons;
    public GameObject PreguntasButtons;

    public System.Action GoBackAction;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowButtons());
        GoBackAction = ReturnFromLeftPanel; 
    }

    IEnumerator ShowButtons()
    {
        yield return new WaitForEndOfFrame();
        fader.fadeToTransparent();
        yield return new WaitForSeconds(1.0f);
        LabelFader.fadeToOpaque();
        yield return new WaitForSeconds(0.6f);
        IdeaButtonScaler.scaleIn();
        yield return new WaitForSeconds(0.15f);
        QuestionButtonScaler.scaleIn();
    }

    public void TouchIdea()
    {
        NotebookImage.color = IdeaColor;
        IdeasButtons.SetActive(true);
        PreguntasButtons.SetActive(false);
        StartCoroutine(FirstPanelCoroutine());
    }

    public void TouchQuestion()
    {
        NotebookImage.color = QuestionColor;
        IdeasButtons.SetActive(false);
        PreguntasButtons.SetActive(true);
        StartCoroutine(FirstPanelCoroutine());
    }

    IEnumerator FirstPanelCoroutine()
    {
        GoBackAction = ReturnFromCenterPanel;
        yield return new WaitForSeconds(0.25f);
        scrollMover.PointA = new Vector2(0.0f, 0.0f);
        scrollMover.PointB = new Vector2(-HorizontalDisplacement, 0.0f);
        scrollMover.Go();
    }

    public void TouchClear()
    {
        inputField.text = "";
        notebookText.text = "";
    }

    public void TouchSubmit()
    {
        StartCoroutine(SubmitCoroutine());
    }

    IEnumerator SubmitCoroutine()
    {
        yield return API.GetSingleton().PostComment(
        PlayerPrefs.GetString("UserId"),
        notebookText.text,
        ContentPrefix,
            (err, text) =>
             {
                 MessagesController.GetSingleton().ShowMessage("Tu comentario se ha enviado exitosamente", Color.blue);
                 TouchClear();
             });
    }

    public void TouchOnMejoraPersonal()
    {
        ContentPrefix = CategoryCodes.EncodeCategoryPrefix(CategoryCodes.Category_MejoraPersonal, true);
        StartCoroutine(SecondPanelCoroutine());
    }

    public void TouchOnMejoraMundo()
    {
        ContentPrefix = CategoryCodes.EncodeCategoryPrefix(CategoryCodes.Category_MejoraMundo, true);
        StartCoroutine(SecondPanelCoroutine());
    }

    public void TouchOnAutoconocimiento()
    {
        ContentPrefix = CategoryCodes.EncodeCategoryPrefix(CategoryCodes.Category_Autoconocimiento, true);
        StartCoroutine(SecondPanelCoroutine());
    }

    public void TouchOnTrascendencia()
    {
        ContentPrefix = CategoryCodes.EncodeCategoryPrefix(CategoryCodes.Category_Trascendencia, true);
        StartCoroutine(SecondPanelCoroutine());
    }

    public void TouchOnAgobios()
    {
        ContentPrefix = CategoryCodes.EncodeCategoryPrefix(CategoryCodes.Category_Agobios, true);
        StartCoroutine(SecondPanelCoroutine());
    }

    IEnumerator SecondPanelCoroutine()
    {
        GoBackAction = ReturnFromRightPanel;
        yield return new WaitForSeconds(0.25f);
        scrollMover.PointA = new Vector2(-HorizontalDisplacement, 0.0f);
        scrollMover.PointB = new Vector2(-2.0f * HorizontalDisplacement, 0.0f);
        scrollMover.Go();
    }

    public void ReturnFromRightPanel()
    {
        scrollMover.PointA = new Vector2(-2.0f * HorizontalDisplacement, 0.0f);
        scrollMover.PointB = new Vector2(-HorizontalDisplacement, 0.0f);
        scrollMover.Go();
        GoBackAction = ReturnFromCenterPanel;
    }
    public void ReturnFromCenterPanel()
    {
        scrollMover.PointA = new Vector2(-HorizontalDisplacement, 0.0f);
        scrollMover.PointB = new Vector2(0.0f, 0.0f);
        scrollMover.Go();
        GoBackAction = ReturnFromLeftPanel;
    }
    public void ReturnFromLeftPanel()
    {
        StartCoroutine(GoBackCoroutine());
    }

    public void GoBackButton()
    {
        GoBackAction();
    }

    IEnumerator GoBackCoroutine()
    {
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync("Planets");
    }
}
