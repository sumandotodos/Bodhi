using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public GameObject NormalPlanetPrefab;
    public GameObject IdeasPrefab;
    public GameObject FavsPrefab;
    public GameObject WritePrefab;
    public GameObject MessagesPrefab;
    public Transform PlanetsParent;

    public string DefaultCase = "";

    // Start is called before the first frame update
    void Start()
    {
        string TypeOfMenu = PlayerPrefs.GetString("TypeOfMenu");
        if(TypeOfMenu == "") 
        {
            SetupScene(DefaultCase);
        }
        else
        {
            SetupScene(TypeOfMenu);
        }
    }

    public void SetupScene(string TypeOfMenu)
    { 
        switch (TypeOfMenu)
        {
            case "":
                SetUpIdeas();
                break;
            case "Home":
                SetUpHome();
                break;
            case "Ideas":
                SetUpIdeas();
                break;
            case "Questions":
                SetUpQuestions();
                break;
            case "Persons":
                SetUpPersons();
                break;
         }
    }

    public void SetUpHome()
    {
        GameObject newGO = (GameObject)Instantiate(FavsPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        Planet newPlanet = newGO.GetComponent<Planet>();
        newPlanet.InnerRing = true;
        newPlanet.OuterRing = true;
        newPlanet.MiddleRing = true;
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(6.0f, 100.0f, 0.0f);
        newPlanet.SetLabel("Favoritos");
        newPlanet.SetScale(0.8f);
        newPlanet.SetRadius(3.8f);

        newGO = (GameObject)Instantiate(NormalPlanetPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<Planet>();
        newPlanet.SetLabel("Favoritos");
        newPlanet.MinesweeperType = "Factory";
        newPlanet.InnerRing = false;
        newPlanet.OuterRing = false;
        newPlanet.MiddleRing = false;
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(-3.0f, -32.0f, 0.0f);
        newPlanet.SetScale(1.0f);
        newPlanet.SetRadius(4.0f);
    }

    public void SetUpIdeas()
    {
        GameObject newGO = (GameObject)Instantiate(NormalPlanetPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        Planet newPlanet = newGO.GetComponent<Planet>();
        newPlanet.InnerRing = true;
        newPlanet.OuterRing = true;
        newPlanet.MiddleRing = true;
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(7.0f, 100.0f, 0.0f);
        newPlanet.SetCategoryAndTopic("Arreglar el mundo", "Mejora personal");
        newPlanet.SetScale(0.8f);
        newPlanet.SetRadius(3.8f);
        newPlanet.MinesweeperType = "Lighthouse";

        newGO = (GameObject)Instantiate(NormalPlanetPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<Planet>();
        newPlanet.SetCategoryAndTopic("Arreglar el mundo", "Mejora del mundo");
        newPlanet.MinesweeperType = "Factory";
        newPlanet.InnerRing = false;
        newPlanet.OuterRing = false;
        newPlanet.MiddleRing = false;
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(-4.0f, -42.0f, 0.0f);
        newPlanet.SetScale(1.0f);
        newPlanet.SetRadius(4.0f);
    }

    public void SetUpQuestions()
    {
        GameObject newGO = (GameObject)Instantiate(NormalPlanetPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        Planet newPlanet = newGO.GetComponent<Planet>();
        newPlanet.InnerRing = true;
        newPlanet.OuterRing = true;
        newPlanet.MiddleRing = true;
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(12.0f, 100.0f, 1.0f);
        newPlanet.SetCategory("Autoconocimiento");
        newPlanet.SetScale(0.8f);
        newPlanet.SetRadius(3.8f);
        newPlanet.MinesweeperType = "Lighthouse";

        newGO = (GameObject)Instantiate(NormalPlanetPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<Planet>();
        newPlanet.SetCategory("Agobios");
        newPlanet.MinesweeperType = "Apartment";
        newPlanet.InnerRing = false;
        newPlanet.OuterRing = false;
        newPlanet.MiddleRing = false;
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(-7.0f, -42.0f, -2.0f);
        newPlanet.SetScale(1.0f);
        newPlanet.SetRadius(4.0f);

        newGO = (GameObject)Instantiate(NormalPlanetPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<Planet>();
        newPlanet.SetCategory("Trascendencia");
        newPlanet.MinesweeperType = "Temple";
        newPlanet.InnerRing = false;
        newPlanet.OuterRing = true;
        newPlanet.MiddleRing = true;
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(-4.0f, -82.0f, -1.0f);
        newPlanet.SetScale(0.75f);
        newPlanet.SetRadius(4.2f);
    }

    public void SetUpPersons()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
