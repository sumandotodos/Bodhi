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
    public GameObject Spinner;

    public UIFader fader;

    Vector3 CameraTarget;

    private void Awake()
    {
        if(orbitalCamera_A==null)
        {
            orbitalCamera_A = FindObjectOfType<OrbitalCamera>();
        }
        Spinner.SetActive(true);
    }

    private void Start()
    {
        fader.Start();
        fader.fadeToTransparent();
        Spinner.SetActive(false);
        PlayerPrefs.SetInt("SkipUsers", 0);
    }

    public void TouchOnPersonsStar()
    {
        PlayerPrefs.SetInt("PagesType", 0);
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
        Spinner.SetActive(true);
        yield return SceneManager.LoadSceneAsync("Title");
    }
}
