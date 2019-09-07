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
    public QuestionController questionController;
    public PersonsAvatarController personsAvatarController;

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
        int MaxUsers = 6;
        List<Person> persons = new List<Person>();
        int page = PlayerPrefs.GetInt("PersonsPage");
        /*yield return API.GetSingleton().GetFollows(PlayerPrefs.GetString("UserId"),
                (err, list) =>
                {
                    foreach (User u in list.result)
                    {
                        persons.Add(new Person(u._id, u.favoritized, u.upvotes));
                    }
                });
        int startIndex = page * MaxPersonsPerPage;
        int endIndex = (page + 1) * MaxPersonsPerPage;
        if(startIndex > persons.Count-1)
        {
            // all random users
        }
        else if(endIndex < persons.Count)
        {
            // add some random users
        }*/
        int myUserIndex = 0;
        List<User> myListOfUsers = null;
        //yield return API.GetSingleton().GetUserIndex(PlayerPrefs.GetString("UserId"),
        //    (text, index) => {
        //        myUserIndex = index;
        //        }
        //     );
        yield return API.GetSingleton().GetRandomUsers(PlayerPrefs.GetString("UserId"),
            "session",
            myUserIndex,
            MaxUsers,
            (text, userlist) =>
            {
                myListOfUsers = userlist.result;
                int matIndex = 0;
                int userIndex = 0;
                foreach(User u in userlist.result)
                {
                    if(userIndex == 0) 
                        u.favquestion = "¿De dónde surgen las emociones? ¿Surgen de los pensamientos?";
                    if (userIndex == 1)
                        u.favquestion = "¿Puede el hecho de practicar meditación y observar nuestro cuerpo, ayudarnos a detectar los momentos en los que estamos a punto de perder los nervios?";
                    if (userIndex == 2)
                        u.favquestion = "¿Cómo te gustaría ser en el futuro?";
                    if (userIndex == 3)
                        u.favquestion = "El hecho de que cada uno nos tomemos las cosas de una manera, ¿hace compatible la libertad personal con la ley del karma?  ";
                    if (userIndex == 4)
                        u.favquestion = "¿Si hubieras nacido en otro país con otra religión o creencias, tu visión del mundo sería la misma que tienes ahora?";
                    if (userIndex == 5)
                        u.favquestion = "¿Qué percepción tendemos a tener sobre lo felices o infelices que son los demás? ¿Pensamos que son más felices de lo que lo son realmente, o al revés? ¿Por qué?";
                    GameObject newGO = (GameObject)Instantiate(NormalPlanetPrefab);
                    newGO.transform.SetParent(PlanetsParent);
                    newGO.transform.localScale = Vector3.one;
                    Planet newPlanet = newGO.GetComponent<Planet>();
                    newPlanet.SetMaterial(planetMats[matIndex]);
                    matIndex = (matIndex + 1) % 4;
                    newPlanet.InnerRing = false;
                    newPlanet.OuterRing = false;
                    newPlanet.MiddleRing = false;
                    newPlanet.Start();
                    newGO.transform.position = Vector3.zero;
                    newGO.transform.rotation = Quaternion.Euler(0.0f + Random.Range(-12.0f, 12.0f), (360.0f / (float)MaxUsers) * (userIndex++), 1.0f);
                    newPlanet.SetLabel(u.handle);
                    newPlanet.SetScale(0.8f);
                    newPlanet.SetRadius(3.8f + Random.Range(-0.8f, 0.4f));
                    newPlanet.MinesweeperType = "Lighthouse";
                }
            });

        questionController.SetListOfUsers(myListOfUsers);
        personsAvatarController.SetListOfUsers(myListOfUsers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
