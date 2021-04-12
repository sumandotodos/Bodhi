using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public UIFader fader;

    public LoginController loginController;
       
    public UITextFader textFader;
    // Start is called before the first frame update
    void Start()
    {
        fader.Start();
        loginController.ForceInitialize();
        StartCoroutine(_ShowTitleSequence());
    }

    IEnumerator _ShowTitleSequence()
    {
        yield return new WaitForSeconds(0.5f);
        fader.fadeToTransparent();
        yield return new WaitForSeconds(1.5f);
        loginController.ShowInterface();
        yield return new WaitForSeconds(1.6f);
        textFader.fadeToOpaque();
    }

    public void TouchOnGoForward()
    {
        StartCoroutine(GoToNextScreen());
    }

    IEnumerator GoToNextScreen()
    {
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.2f);
        yield return SceneManager.LoadSceneAsync("MainMenu");
    }
}
