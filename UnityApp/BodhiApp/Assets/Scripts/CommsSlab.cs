using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommsSlab : Slab, OptionIndexReceiver
{
    public Color ChatColor;
    public Color AudioColor;
    public Color VideoColor;

    public Image ChatImage;
    public Image AudioImage;
    public Image VideoImage;

    public Text Explanation;
    public Text PhoneNumber;
    public Text MeansAgreement;

    public CommsChooser commsChooser;

    public string OtherUserId;

    int Activation = -1;

    public void SetPhoneNumber(string phone)
    {
        if(phone=="")
        {
            PhoneNumber.text = "(Tlf no especificado)";
        }
        else
        {
            PhoneNumber.text = phone;
        }

    }

    public void SetAgreement(string means)
    {
        Debug.Log("<color=orange>Means = " + means + "</color>");
        if (means == "")
        {
            SetColor(Color.red);
            Explanation.enabled = true;
            PhoneNumber.enabled = false;
            MeansAgreement.enabled = false;
        }
        else
        {
            SetColor(Color.green);
            Explanation.enabled = false;
            MeansAgreement.text = "Estáis de acuerdo en comunicaros mediante: <color=brown>:means:</color>".Replace(":means:", means);
            PhoneNumber.enabled = true;
            MeansAgreement.enabled = true;
        }
    }

    public void ReceiveIndex(int index)
    {
        Debug.Log("Received index: " + index);
        API.GetSingleton().SetCommsPreference(PlayerPrefs.GetString("UserId"), OtherUserId, index, (err, res) =>
        {
            Debug.Log("Changed?");
        });
        Activation = index;
    }

    public void SetIndex(int n)
    {
        commsChooser.SetIndex(n);
    }
}
