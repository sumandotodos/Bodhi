using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

    public OrbitalCamera orbitalCamera_A;
    public GameObject PersonasStar;
    public GameObject IdeasStar;
    public GameObject PreguntasStar;
    public GameObject YoStar;

    public UIFader fader;

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
        Debug.Log("Persons!");
        orbitalCamera_A.SetPosition(PersonasStar.transform.position);
        orbitalCamera_A.SetZoomInConfiguration();
        orbitalCamera_A.SetZDistance(5.0f);
        fader.fadeToOpaque();
    }


    public void TouchOnMeStar()
    {
        Debug.Log("Me!");

    }

    public void TouchOnIdeasStar()
    {

        Debug.Log("Ideas!");
    }

    public void TouchOnQuestionsStar()
    {
        Debug.Log("Questions!");
    }
}
