using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Planet : MonoBehaviour
{
    public bool OuterRing;
    public bool MiddleRing;
    public bool InnerRing;

    public Transform ScaleObject;
    public Transform RadiusTransform;

    public string MinesweeperType;
    public string Category;
    public string Topic;

    public GameObject OuterRingObject;
    public GameObject MiddleRingObject;
    public GameObject InnerRingObject;
    public GameObject PlanetObject;

    bool started = false;

    // Start is called before the first frame update
    public void Start()
    {
        if (started) return;
        started = true;
        if (OuterRingObject != null)
        {
            OuterRingObject.SetActive(OuterRing);
        }
        if (MiddleRingObject != null)
        {
            MiddleRingObject.SetActive(MiddleRing);
        }
        if (InnerRingObject != null)
        {
            InnerRingObject.SetActive(InnerRing);
        }
    }

    public void SetLabel(string NewLabel)
    {
        SetLabel(NewLabel, Color.white);
    }

    public void SetLabel(string NewLabel, Color color)
    {
        TextMeshPro textComponent = GetComponentInChildren<TextMeshPro>();
        textComponent.text = NewLabel;
        textComponent.color = color;
    }

    public void SetCategory(string NewCat)
    {
        SetCategoryAndTopic(NewCat, "");
    }

    public void SetCategoryAndTopic(string NewCat, string NewTopic)
    {
        TextMeshPro textComponent = GetComponentInChildren<TextMeshPro>();
        if (NewTopic != "")
        {
            textComponent.text = NewTopic; 
            Category = NewCat;
            Topic = NewTopic;
        }
        else
        {
            textComponent.text = Category = NewCat;
            Topic = "";
        }
    }

    public void SetScale(float NewScale)
    {
        ScaleObject.transform.localScale = NewScale * Vector3.one;
    }

    public void SetRadius(float NewRadius)
    {
        RadiusTransform.localPosition = NewRadius * Vector3.left;
    }

    public virtual void MakePlanetControllerProceedToNextScreen(PlanetGameController controller)
    {
        PlayerPrefs.SetString("MinesweeperPrefab", MinesweeperType);
        PlayerPrefs.SetString("ContentType", Category);
        PlayerPrefs.SetString("ContentTopic", Topic);
        controller.PlanetsToSweeperSequence();
    }

    public void SetMaterial(Material m)
    {
        OuterRingObject.GetComponent<MeshRenderer>().material = m;
        MiddleRingObject.GetComponent<MeshRenderer>().material = m;
        InnerRingObject.GetComponent<MeshRenderer>().material = m;
        PlanetObject.GetComponent<MeshRenderer>().material = m;
    }


}
