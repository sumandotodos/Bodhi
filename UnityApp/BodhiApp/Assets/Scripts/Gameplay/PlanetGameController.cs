using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetGameController : MonoBehaviour
{

    static PlanetGameController planetGameController = null;

    public UIFader fader;
    public OrbitalCamera orbitalCamera_A = null;

    public GameObject NormalPlanetPrefab;
    public GameObject FavPrefab;
    public GameObject IdeaPrefab;

    public float ZDistance = 25.0f;
    public float PitchAngle = 20.0f;

    private void Awake()
    {
        if(planetGameController == null)
        {
            planetGameController = this;
            if(orbitalCamera_A == null)
            {
                orbitalCamera_A = FindObjectOfType<OrbitalCamera>();
            }
        }
    }

    public static PlanetGameController GetSingleton()
    {
        return planetGameController;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        fader.Start();
        Time.timeScale = 30.0f;
        orbitalCamera_A.Start();
        orbitalCamera_A.SetZDistanceImmediate(40.0f);
        orbitalCamera_A.SetXAngleImmediate(PitchAngle);
        yield return new WaitForSecondsRealtime(2.5f);

        Time.timeScale = 1.0f;
        fader.fadeToTransparent();
        orbitalCamera_A.SetZDistance(ZDistance); 
    }


    public void ClickOnPlanet(Planet planet)
    {
        orbitalCamera_A.SetZoomInConfiguration();
        orbitalCamera_A.SetPosition(planet.GetComponentInChildren<SphereCollider>().gameObject.transform.position);
        orbitalCamera_A.SetZDistance(1.0f);
        planet.MakePlanetControllerProceedToNextScreen(this);
    }

    public void PlanetsToSweeperSequence()
    {
        StartCoroutine(_PlanetsToSweeperSequence());
    }

    IEnumerator _PlanetsToSweeperSequence()
    {
        yield return new WaitForSeconds(0.3f);
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync("Minesweeper");
    }

    public void PlanetsToSceneSequence(string scene, TypeOfContent favType)
    {
        PlayerPrefs.SetString("FavoriteType", Heart.FavTypeToString(favType));
        StartCoroutine(_PlanetsToFavoritesSequence(scene));
    }

    IEnumerator _PlanetsToFavoritesSequence(string scene)
    {
        yield return new WaitForSeconds(0.3f);
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync(scene);
    }

    public void PlanetsToComposeSequence()
    {
        StartCoroutine(_PlanetsToComposeSequence());
    }

    IEnumerator _PlanetsToComposeSequence()
    {
        yield return new WaitForSeconds(0.3f);
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync("Compose");
    }

    public void PlanetsToSequence(string Seq)
    {
        StartCoroutine(_PlanetsToSequence(Seq));
    }

    IEnumerator _PlanetsToSequence(string Seq)
    {
        yield return new WaitForSeconds(0.3f);
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync(Seq);
    }

    public void TouchReload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        FindObjectOfType<PlanetSpawner>().NextUsersPage();
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
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
        yield return SceneManager.LoadSceneAsync("MainMenu");
    }


}
