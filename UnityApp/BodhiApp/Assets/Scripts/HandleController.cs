using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleController : MonoBehaviour
{
    public TMPro.TMP_InputField handleInputField;

    float Remaining = 0.0f;

    public float Interval = 1.0f;
    string HandleToUpload;

    public GameObject Warning;
    public Text FinalId;

    private void Start()
    {
        FinalId.enabled = false;
        Warning.SetActive(true);
        LoginConfigurations.init();
        API.GetSingleton().GetHandle(PlayerPrefs.GetString("UserId"), (err, handle) =>
        { 
            handleInputField.text = handle;
            OnContentChanged();
        });
    }

    public void OnContentChanged()
    {
        if(ValidateHandle(handleInputField.text))
        {
            handleInputField.textComponent.color = Color.black;
            Warning.SetActive(false);
            FinalId.enabled = true;
            FinalId.text = "Tu id público será: " + handleInputField.text + "#" + PlayerPrefs.GetString("UserId");
            Remaining = Interval;
            HandleToUpload = handleInputField.text;
        }
        else
        {
            handleInputField.textComponent.color = Color.red;
            Warning.SetActive(true);
            FinalId.enabled = false;
            Remaining = 0.0f;
        }

    }

    void Update()
    {
        if (Remaining > 0.0f) {
            Remaining -= Time.deltaTime;
            if (Remaining <= 0.0f)
            {
                API.GetSingleton().UpdateHandle(PlayerPrefs.GetString("UserId"), HandleToUpload, (err, text) => { });
            }
        }
    }

    private bool ValidateHandle(string handle)
    {
        if (handle.Length < 4) return false;
        if (handle.Length > 32) return false;
        if (handle.Contains(" ")) return false;
        if (handle.Contains(":")) return false;
        if (handle.Contains("/")) return false;
        if (handle.Contains("#")) return false;
        return true; 
    }

}
