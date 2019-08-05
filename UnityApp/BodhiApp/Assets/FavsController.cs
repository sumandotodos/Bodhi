using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FavItem
{
    public string id;
    public Color color;

    public FavItem(string _id, Color _color)
    {
        id = _id;
        color = _color;
    }
}

[System.Serializable]
public class Favorites_REST
{
    public List<string> favorites;
}

public class FavsController : MonoBehaviour
{

    //public FavItem[] favItems;

    public ContentsManager contentsManager;
    public DragController dragController;
    public ListController listController;

    public float MinSlabHeight = 200.0f;

    public Transform SlabsParent;
    public Transform SlabsScroll;
    public GameObject SlabPrefab;
    public UIFader fader;
    float CurrentDestinationY = 665.0f;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        fader.Start();
        yield return new WaitForSeconds(0.1f);
        List<FavItem> favItems = new List<FavItem>();
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        yield return API.GetSingleton().GetFavoritesList(PlayerPrefs.GetString("UserId"), (err, text) =>
        {
            Favorites_REST favs = JsonUtility.FromJson<Favorites_REST>(text);
            for(int i = 0; i < favs.favorites.Count; ++i)
            {
                favItems.Add(new FavItem(favs.favorites[i], Color.green));
            }
            return 0;
        });

        foreach(FavItem item in favItems)
        {
            listController.AddSlab(SpawnSlab(item));
            yield return new WaitForSeconds(0.15f);
        }
        fader.fadeToTransparent();
    }

    Slab SpawnSlab(FavItem item)
    {
        Slab newSlab = SpawnSlab(new Vector2(11.0f, CurrentDestinationY), new Vector2(11.0f, CurrentDestinationY - 1850.0f));
        newSlab.SetColor(item.color);
        newSlab.Index = listController.GetNumberOfSlabs();
        newSlab.id = item.id;

        float h = newSlab.SetText(GetText(item));
        h = Mathf.Max(h, MinSlabHeight);
        newSlab.SetHeight(h);
        CurrentDestinationY -= Slab.Adjust(h);
        return newSlab;
    }

    string GetText(FavItem item)
    {
        if(item.id.StartsWith("_"))
        {
            return "Not yet";
        }
        else
        {
            string[] fields = item.id.Split(':');
            int cat;
            int topic;
            int table;
            int row;
            int.TryParse(fields[0], out cat);
            int.TryParse(fields[1], out topic);
            int.TryParse(fields[2], out table);
            int.TryParse(fields[3], out row);
            string res = contentsManager.RetrieveText(cat, topic, table, row);
            return res;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    Slab SpawnSlab(Vector2 destination, Vector2 initialPosition)
    {
        GameObject newGO = (GameObject)Instantiate(SlabPrefab);
        newGO.transform.SetParent(SlabsParent);
        newGO.transform.localScale = Vector3.one;
        newGO.transform.localPosition = initialPosition;
        newGO.GetComponent<Magnetor>().CurrentPosition = initialPosition;
        newGO.GetComponent<Magnetor>().Destination = destination;
        newGO.GetComponent<Magnetor>().Going = true;
        return newGO.GetComponent<Slab>();
    }

    public void TouchGoBackButton()
    {
        StartCoroutine(GoBackCoroutine());
    }

    IEnumerator GoBackCoroutine()
    {
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync("Planets");
    }

    // @TODO I'm not very fond of this redundancy, but it saves cycles...
    public void Dislike(string id, int index)
    {
        // do something with id and save to file
        listController.DismissItem(index);
    }
}
