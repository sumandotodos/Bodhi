using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FavsController : MonoBehaviour
{
    public Transform SlabsParent;
    public GameObject SlabPrefab;
    public UIFader fader;
    float CurrentDestinationY = 665.0f;
    // Start is called before the first frame update
    void Start()
    {
        fader.Start();
        fader.fadeToTransparent();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            Slab newSlab = SpawnSlab(new Vector2(11.0f, CurrentDestinationY), new Vector2(11.0f, CurrentDestinationY - 750.0f));
            newSlab.SetColor(Color.cyan);

            float h = newSlab.SetText("Me cago en todo lo que se menea. La verdad sea dicha. "); //Todo lo que un médico puede llegar a ser en Asturias. Esto es una cosa abracadabrante que te cagas por las patas abajo. Nada puede ser todo o todo puede ser algo aguna vez. No me cuenters más ghistorias");
            Debug.Log("<color=purple>" + h + "</color>");
            h = Mathf.Max(h, 220.0f);
            newSlab.SetHeight(h);
            CurrentDestinationY -= (h + 30.0f);
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
}
