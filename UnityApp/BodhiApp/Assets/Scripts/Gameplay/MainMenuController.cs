using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public OrbitalCamera orbitalCamera_A;
    public GameObject PersonasStar;
    public GameObject IdeasStar;
    public GameObject PreguntasStar;
    public GameObject YoStar;

    public UIFader fader;

    Vector3 CameraTarget;

    private void Awake()
    {
        if(orbitalCamera_A==null)
        {
            orbitalCamera_A = FindObjectOfType<OrbitalCamera>();
        }
    }

    private void Start()
    {
        fader.Start();
        fader.fadeToTransparent();
    }

    public void TouchOnPersonsStar()
    {
        PlayerPrefs.SetString("TypeOfMenu", "Persons");
        CameraTarget = PersonasStar.transform.position;
        StartCoroutine(GoToNextScreen());
    }


    public void TouchOnMeStar()
    {
        PlayerPrefs.SetString("TypeOfMenu", "Home");
        CameraTarget = YoStar.transform.position;
        StartCoroutine(GoToNextScreen());
    }

    public void TouchOnIdeasStar()
    {
        PlayerPrefs.SetString("TypeOfMenu", "Ideas");
        CameraTarget = IdeasStar.transform.position;
        StartCoroutine(GoToNextScreen());
    }

    public void TouchOnQuestionsStar()
    {
        PlayerPrefs.SetString("TypeOfMenu", "Questions");
        CameraTarget = PreguntasStar.transform.position;
        StartCoroutine(GoToNextScreen());
    }

    IEnumerator GoToNextScreen()
    {
        orbitalCamera_A.SetPosition(CameraTarget);
        orbitalCamera_A.SetZoomInConfiguration();
        orbitalCamera_A.SetZDistance(5.0f);
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.2f);
        yield return SceneManager.LoadSceneAsync("Planets");

    }

    public void TouchGoBack()
    {
        StartCoroutine(GoBackCoroutine());
    }

    IEnumerator GoBackCoroutine()
    {
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync("Title");
    }
}
