using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFader_anchor : MonoBehaviour {

    public float minOpacity;
    public float maxOpacity;

    Image imageComponent;
    Color baseColor;

    public MonoBehaviour parent;

    // Use this for initialization
    void Start () {
        imageComponent = this.GetComponent<Image>();
        baseColor = imageComponent.color;
	}

    bool parked = false;

    // Update is called once per frame
    void Update()
    {
        float para = ((UITwoPointEffect)parent).getNormalizedParameter();
        if (para > 0.0f)
        {
            if (parked == true)
            {
                parked = false;
                imageComponent.enabled = true;
            }
            float opacity = Mathf.Lerp(minOpacity, maxOpacity, para);
            baseColor.a = opacity;
            imageComponent.color = baseColor;
        }
        else
        {
            if(parked == false)
            {
                parked = true;
                imageComponent.enabled = false;
            }
        }
    }
}
