using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Person
{
    public string id;
    public int favoritized;
    public int upvotes;
    public Texture2D avatar;
    public string question;
    public string profile;

    public Person(string _id, int _favoritized, int _upvotes)
    {
        id = _id;
        favoritized = _favoritized;
        upvotes = _upvotes;
        avatar = null;
        question = "";
        profile = "";
    }
}

public class PlanetSpawner : MonoBehaviour
{
    public GameObject NormalPlanetPrefab;
    public GameObject PersonPlanetPrefab;
    public GameObject IdeasPrefab;
    public GameObject CameraPrefab;
    public GameObject FavsPrefab;
    public GameObject IdeaFavsPrefab;
    public GameObject QuestionsFavsPrefab;
    public GameObject WritePrefab;
    public GameObject MessagesPrefab;
    public GameObject MyIdeasPrefab;
    public GameObject MyQuestionsPrefab;
    public Transform PlanetsParent;
    public SpriteRenderer spriteRenderer;
    public AvatarTaker avatarTaker;
    public Material[] planetMats;
    public PlanetHandleController planetHandleController;
    public OtherUsersPlanetsController otherUsersPlanetsController;

    public string DefaultCase = "";

    public PlanetsUIController planetsUIController;

    public int MaxPersonsPerPage = 6;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.enabled = false;
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
        planetsUIController.SetupNonPersonsConfiguration();

        GameObject newGO = (GameObject)Instantiate(IdeaFavsPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        Planet newPlanet = newGO.GetComponent<Heart>();
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(6.0f, 100.0f, 0.0f);
        //newPlanet.SetLabel("Favoritos");
        newPlanet.SetScale(0.8f);
        newPlanet.SetRadius(3.8f);

        newGO = (GameObject)Instantiate(QuestionsFavsPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<Heart>();
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(6.0f, 130.0f, 0.0f);
        //newPlanet.SetLabel("Favoritos");
        newPlanet.SetScale(0.8f);
        newPlanet.SetRadius(3.8f);

        newGO = (GameObject)Instantiate(WritePrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<Pencil>();
        newPlanet.Start();
        newPlanet.SetLabel("Redactar");
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(-3.0f, -32.0f, 0.0f);
        newPlanet.SetScale(1.0f);
        newPlanet.SetRadius(3.6f);

        newGO = (GameObject)Instantiate(MyQuestionsPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<Pencil>();
        newPlanet.Start();
        newPlanet.SetLabel("Mis preguntas");
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(3.0f, -04.0f, 0.0f);
        newPlanet.SetScale(1.0f);
        newPlanet.SetRadius(3.6f);

        newGO = (GameObject)Instantiate(MyIdeasPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<Pencil>();
        newPlanet.Start();
        newPlanet.SetLabel("Mis ides");
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(6.4f, 18.0f, 0.0f);
        newPlanet.SetScale(1.0f);
        newPlanet.SetRadius(3.6f);

        newGO = (GameObject)Instantiate(CameraPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<CameraPlanet>();
        newPlanet.Start();
        newPlanet.SetLabel("Avatar y perfil");
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(-16.0f, 60.0f, 0.0f);
        newPlanet.SetScale(1.0f);
        newPlanet.SetRadius(3.0f);

        newGO = (GameObject)Instantiate(MessagesPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<LetterPlanet>();
        newPlanet.Start();
        newPlanet.SetLabel("Mensajes");
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(-8.0f, 200.0f, 0.0f);
        newPlanet.SetScale(1.0f);
        newPlanet.SetRadius(3.5f);

        Texture2D avatarTex = avatarTaker.ApplyMaskTexture(avatarTaker.LoadAvatar());
        Sprite avatarSprite = Sprite.Create(avatarTex, new Rect(0, 0, avatarTex.width, avatarTex.height), new Vector2(0.5f, 0.5f));
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = avatarSprite;

        planetHandleController.ShowHandle();
    }

    public void SetUpIdeas()
    {
        planetsUIController.SetupNonPersonsConfiguration();

        GameObject newGO = (GameObject)Instantiate(NormalPlanetPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        Planet newPlanet = newGO.GetComponent<Planet>();
        newPlanet.SetMaterial(planetMats[0]);
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
        newPlanet.SetMaterial(planetMats[1]);
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

        planetHandleController.HideHandle();
    }

    public void SetUpQuestions()
    {
        planetsUIController.SetupNonPersonsConfiguration();

        GameObject newGO = (GameObject)Instantiate(NormalPlanetPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        Planet newPlanet = newGO.GetComponent<Planet>();
        newPlanet.SetMaterial(planetMats[2]);
        newPlanet.InnerRing = true;
        newPlanet.OuterRing = true;
        newPlanet.MiddleRing = true;
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(12.0f, 100.0f, 1.0f);
        newPlanet.SetCategoryAndTopic("Autoconocimiento", "Autoconocimiento");
        newPlanet.SetScale(0.8f);
        newPlanet.SetRadius(3.8f);
        newPlanet.MinesweeperType = "Lighthouse";

        newGO = (GameObject)Instantiate(NormalPlanetPrefab);
        newGO.transform.SetParent(PlanetsParent);
        newGO.transform.localScale = Vector3.one;
        newPlanet = newGO.GetComponent<Planet>();
        newPlanet.SetMaterial(planetMats[3]);
        newPlanet.SetCategoryAndTopic("Agobios", "Agobios");
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
        newPlanet.SetMaterial(planetMats[4]);
        newPlanet.SetCategoryAndTopic("Trascendencia", "Trascendencia");
        newPlanet.MinesweeperType = "Temple";
        newPlanet.InnerRing = false;
        newPlanet.OuterRing = true;
        newPlanet.MiddleRing = true;
        newPlanet.Start();
        newGO.transform.position = Vector3.zero;
        newGO.transform.rotation = Quaternion.Euler(-4.0f, -82.0f, -1.0f);
        newPlanet.SetScale(0.75f);
        newPlanet.SetRadius(4.2f);

        planetHandleController.HideHandle();
    }

    public void SetUpPersons()
    {
        planetsUIController.SetupPersonsConfiguration();

        StartCoroutine(SetUpPersonsCoroutine());
    }

    IEnumerator SetUpPersonsCoroutine()
    {
        int MaxUsers = 3;
        List<Person> persons = new List<Person>();
        int page = PlayerPrefs.GetInt("PersonsPage");

        otherUsersPlanetsController.PlanetMaterials = planetMats;

        List<User> myListOfUsers = null;

        List<ScaleFader> listOfScaleFaders = new List<ScaleFader>();

        int skip = PlayerPrefs.GetInt("SkipUsers");

        yield return API.GetSingleton().GetRandomUsers(PlayerPrefs.GetString("UserId"),
            "session",
            skip,
            MaxUsers,
            (text, userlist) =>
            {
                myListOfUsers = userlist.result;
                int matIndex = 0;
                int userIndex = 0;
                foreach (User u in userlist.result)
                {
                    GameObject newGO = (GameObject)Instantiate(PersonPlanetPrefab);
                    newGO.transform.SetParent(PlanetsParent);
                    newGO.transform.localScale = Vector3.one;
                    PersonPlanet newPlanet = newGO.GetComponent<PersonPlanet>();
                    newPlanet.SetMaterial(planetMats[matIndex]);
                    matIndex = (matIndex + 1) % 4;
                    newPlanet.InnerRing = false;
                    newPlanet.OuterRing = false;
                    newPlanet.MiddleRing = false;
                    newPlanet.handle = u.handle;
                    newPlanet.id = u._id;
                    newPlanet.Start();
                    newGO.transform.position = Vector3.zero;
                    newGO.transform.rotation = Quaternion.Euler(0.0f + Random.Range(-12.0f, 12.0f), (360.0f / (float)MaxUsers) * (userIndex++), 1.0f);
                    newPlanet.SetLabel(u.handle);
                    newPlanet.SetScale(0.8f);
                    newPlanet.SetRadius(3.8f + Random.Range(-0.8f, 0.4f));
                    newPlanet.MinesweeperType = "Lighthouse";
                    ScaleFader sf = newPlanet.GetComponentInChildren<ScaleFader>();
                    listOfScaleFaders.Add(sf);
                }
                int newskip = skip + MaxUsers;
                PlayerPrefs.SetInt("SkipUsers", newskip);
            });

        foreach(User u in myListOfUsers)
        {
            yield return API.GetSingleton().GetUserProfileAndQuestion(u._id, u, (res, user, profquest) =>
            {
                if (ContentsManager.IsLocalContent(profquest.favquestionid))
                {
                    user.favquestion = ContentsManager.GetSingleton().GetLocalContentFromId(profquest.favquestionid);
                }
                else
                {
                    user.favquestion = profquest.favquestion;
                }
            });
        }

        otherUsersPlanetsController.SetPlanetsScaleFaders(listOfScaleFaders);
        otherUsersPlanetsController.SetListOfUsers(myListOfUsers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
