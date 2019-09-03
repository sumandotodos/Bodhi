using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsUIController : MonoBehaviour
{
    public GameObject GoBackButtonLeft;
    public GameObject GoBackButtonRight;
    public GameObject ReloadButton;
    public GameObject Question;

    public void SetupPersonsConfiguration()
    {
        GoBackButtonLeft.SetActive(true);
        GoBackButtonRight.SetActive(false);
        ReloadButton.SetActive(true);
        Question.SetActive(true);
        Question.GetComponent<UIFader>().Start();
        Question.GetComponent<UIFader>().fadeToTransparentImmediately();
    }

    public void SetupNonPersonsConfiguration()
    {
        GoBackButtonLeft.SetActive(false);
        GoBackButtonRight.SetActive(true);
        ReloadButton.SetActive(false);
        Question.SetActive(false);
    }
}
