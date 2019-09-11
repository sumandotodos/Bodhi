using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetsUIController : MonoBehaviour
{
    public GameObject GoBackButtonLeft;
    public GameObject GoBackButtonRight;
    public GameObject ReloadButton;
    public GameObject Question;
    public GameObject Button;
    public UIScaleFader ButtonScaler;
    public SpriteRenderer AvatarSpriteRenderer;
    public UIFader AvatarFader;

    public void SetupPersonsConfiguration()
    {
        GoBackButtonLeft.SetActive(true);
        GoBackButtonRight.SetActive(false);
        ReloadButton.SetActive(true);
        Question.SetActive(true);
        Button.SetActive(true);
        Question.GetComponent<Text>().text = "";
        Question.GetComponent<UITextFader>().Start();
        Question.GetComponent<UITextFader>().fadeToTransparentImmediately();
    }

    public void SetupNonPersonsConfiguration()
    {
        GoBackButtonLeft.SetActive(false);
        GoBackButtonRight.SetActive(true);
        ReloadButton.SetActive(false);
        Question.SetActive(false);
        Button.SetActive(false);
    }

    public void changeQuestion(string newQuestion)
    {
        StartCoroutine(QuestionChangeCoroutine(newQuestion));
    }

    IEnumerator QuestionChangeCoroutine(string newQuestion)
    {
        Question.GetComponent<UITextFader>().fadeToTransparent();
        yield return new WaitForSeconds(0.5f);
        Question.GetComponent<Text>().text = newQuestion;
        Question.GetComponent<UITextFader>().fadeToOpaque();
        if(newQuestion == "")
        {
            ButtonScaler.scaleOut();
        }
        else
        {
            ButtonScaler.scaleIn();
        }
    }

    public void changeAvatar(Texture2D NewAvatar)
    {
        StartCoroutine(AvatarChangeCoroutine(NewAvatar));
    }

    IEnumerator AvatarChangeCoroutine(Texture2D NewAvatar)
    {
        AvatarFader.fadeToTransparent();
        yield return new WaitForSeconds(0.5f);
        Sprite avatarSprite = Sprite.Create(NewAvatar, new Rect(0, 0, NewAvatar.width, NewAvatar.height), new Vector2(0.5f, 0.5f));
        AvatarSpriteRenderer.sprite = avatarSprite;
        AvatarSpriteRenderer.enabled = true;
        AvatarFader.fadeToOpaque();
    }
}
