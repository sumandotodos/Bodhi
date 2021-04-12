using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class VersionLabel : MonoBehaviour
{

    Text theText;

    private void Start()
    {
        theText = GetComponent<Text>();
        theText.text = VersionController.getVersion();
    }

}
