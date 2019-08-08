using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowButtons());   
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
        StartCoroutine(TouchIdeaCoroutine());
    }

    IEnumerator TouchIdeaCoroutine()
    {
        yield return new WaitForSeconds(0.25f);
        NotebookImage.color = IdeaColor;
        scrollMover.Go();
    }

    public void TouchQuestion()
    {
        StartCoroutine(TouchQuestionCoroutine());
    }

    IEnumerator TouchQuestionCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        NotebookImage.color = QuestionColor;
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
        yield return API.GetSingleton().PostComment(PlayerPrefs.GetString("UserId"), 
        notebookText.text, 
            (err, text) =>
             {
                 Debug.Log("Post comment request returned: " + text);
                 TouchClear();
                 return 0;
             });
    }
}
