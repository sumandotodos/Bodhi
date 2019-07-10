using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetGameController : MonoBehaviour
{

    static PlanetGameController planetGameController = null;

    public UIFader fader;
    public OrbitalCamera orbitalCamera_A = null;

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
        yield return new WaitForSecondsRealtime(2.5f);

        Time.timeScale = 1.0f;
        fader.fadeToTransparent();
        orbitalCamera_A.SetZDistance(17.0f); 
    }

    public void ClickOnPlanet(Planet planet)
    {
        orbitalCamera_A.SetZoomInConfiguration();
        orbitalCamera_A.SetPosition(planet.GetComponentInChildren<SphereCollider>().gameObject.transform.position);
        orbitalCamera_A.SetZDistance(1.0f);
        PlanetsToSweeperSequence();
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
        yield return SceneManager.LoadSceneAsync("Minesweeper_Factories");
    }


}
