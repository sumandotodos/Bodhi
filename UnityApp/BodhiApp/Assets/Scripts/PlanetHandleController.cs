using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetHandleController : MonoBehaviour
{
    public TextMeshPro AliasLabel;

    public void ShowHandle()
    {
        LoginConfigurations.init();
        API.GetSingleton().GetHandle(PlayerPrefs.GetString("UserId"), (err, handle) =>
        {
            AliasLabel.text = handle + "#" + PlayerPrefs.GetString("UserId");
            AliasLabel.enabled = true;
        });
    }

    public void HideHandle()
    {
        AliasLabel.enabled = false;
    }
}
