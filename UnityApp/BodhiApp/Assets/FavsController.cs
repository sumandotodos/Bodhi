using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FavItem
{
    public string id;
    public Color color;
}

public class FavsController : MonoBehaviour
{

    public FavItem[] favItems;

    public ContentsManager contentsManager;
    public DragController dragController;
    public ListController listController;

    public Transform SlabsParent;
    public Transform SlabsScroll;
    public GameObject SlabPrefab;
    public UIFader fader;
    float CurrentDestinationY = 665.0f;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        fader.Start();
        fader.fadeToTransparent();
        foreach(FavItem item in favItems)
        {
            listController.AddSlab(SpawnSlab(item));
            yield return new WaitForSeconds(0.15f);
        }
    }



    Slab SpawnSlab(FavItem item)
    {
        Slab newSlab = SpawnSlab(new Vector2(11.0f, CurrentDestinationY), new Vector2(11.0f, CurrentDestinationY - 1850.0f));
        newSlab.SetColor(item.color);
        newSlab.Index = listController.GetNumberOfSlabs();
        newSlab.id = item.id;

        float h = newSlab.SetText(GetText(item));
        h = Mathf.Max(h, 180.0f);
        newSlab.SetHeight(h);
        CurrentDestinationY -= (h + 15.0f + h / 6);
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
        if(Input.GetKeyDown(KeyCode.L))
        {
            Slab newSlab = SpawnSlab(new Vector2(11.0f, CurrentDestinationY), new Vector2(11.0f, CurrentDestinationY - 1850.0f));
            newSlab.SetColor(Color.cyan);

            float h = newSlab.SetText("Me cago en todo lo que se menea. La verdad sea dicha. Todo lo que un médico puede llegar a ser en Asturias. Esto es una cosa abracadabrante que te cagas por las patas abajo. Nada puede ser todo o todo puede ser algo aguna vez. No me cuenters más ghistorias. Ne me parece nada divertido lo que estás haciendo con los geranios de Uma, la abogada del primo de ferran el manitas del cuarto");
            Debug.Log("<color=purple>" + h + "</color>");
            h = Mathf.Max(h, 225.0f);
            newSlab.SetHeight(h);
            CurrentDestinationY -= (h + 30.0f + h/4);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Slab newSlab = SpawnSlab(new Vector2(11.0f, CurrentDestinationY), new Vector2(11.0f, CurrentDestinationY - 1850.0f));
            newSlab.SetColor(Color.green);

            float h = newSlab.SetText("Tampoco hacía falta...");
            Debug.Log("<color=purple>" + h + "</color>");
            h = Mathf.Max(h, 225.0f);
            newSlab.SetHeight(h);
            CurrentDestinationY -= (h + 30.0f + h / 4);
        }
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
