using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ContactOption
{
    public string description;
    public bool allow;
    public ContactOption(string _description, bool _allow)
    {
        description = _description;
        allow = _allow;
    }
}

[System.Serializable]
public class UserProfile
{
    public string about;
    public string phone;
    public string email;
    public List<ContactOption> contactoptions;
    public UserProfile()
    {
        about = "";
        phone = "";
        email = "";
        contactoptions = new List<ContactOption>();
    }
}

[System.Serializable]
public class ToggleDescriptor
{
    public string description;
    public Toggle toggle;
}

public class ProfileController : MonoBehaviour
{
    public InputField profileInputField;
    public Text remainingCharactersText;
    public InputField emailInputField;
    public InputField phoneInputField;
    public ToggleDescriptor[] toggles;
    public Color remainOKColor;
    public Color remainFewColor;

    public int MaxChars = 250;

    public float UpdateInterval = 5.0f;

    bool Dirty = false;
    float Remaining;

    UserProfile profile = new UserProfile();

    void Start()
    {
        Remaining = UpdateInterval;
    }

    void Update()
    {
        if (Dirty && Remaining > 0.0f)
        {
            Remaining -= Time.deltaTime;
            if (Remaining <= 0.0f)
            {
                UploadProfileToServer();
            }
        }
    }

    private void UpdateRemainingChars(int remain)
    {
        remainingCharactersText.text = "(" + remain + " caracteres restantes)";
        remainingCharactersText.color = remain > 30 ? remainOKColor : remainFewColor;
    }

    public void OnToggleChange(int index)
    {
        int profileOptionIndex = FGUtils.findInList<ContactOption>(profile.contactoptions,
            (option) => { return option.description == toggles[index].description; });

        if(profileOptionIndex == -1)
        {
            profile.contactoptions.Add(
                new ContactOption(
                    toggles[index].description,
                    toggles[index].toggle.isOn
                )
            );
        }
        else
        {
            profile.contactoptions[profileOptionIndex].allow =
                toggles[index].toggle.isOn;
        }
        Dirty = true;
        Remaining = UpdateInterval;
        Spy();


    }

    public void OnAboutChanged()
    {
        int nChars = Mathf.Min(profileInputField.text.Length, 250);
        profile.about = profileInputField.text.Substring(0, nChars);
        profileInputField.text = profile.about;
        UpdateRemainingChars(MaxChars - nChars);
        Dirty = true;
        Remaining = UpdateInterval;
        Spy();
    }

    public void OnPhoneChanged()
    {
        profile.phone = phoneInputField.text;
        Dirty = true;
        Remaining = UpdateInterval;
        Spy();
    }

    public void OnEmailChanged()
    {
        profile.email = emailInputField.text;
        Dirty = true;
        Remaining = UpdateInterval;
        Spy();
    }

    public void Spy()
    {
        string representation = JsonUtility.ToJson(profile);
        Debug.Log(representation);
    }

    public void Populate(UserProfile _profile)
    {
        profile = _profile;
        phoneInputField.text = profile.phone;
        emailInputField.text = profile.email;
        profileInputField.text = profile.about;
        foreach(ToggleDescriptor t in toggles)
        {
            int index = FGUtils.findInList<ContactOption>(profile.contactoptions,
             (o) => { return o.description == t.description; });
            t.toggle.isOn = (index != -1) && (profile.contactoptions[index].allow);
        }
    }

    public void UploadProfileToServer()
    {
        if (Dirty)
        {
            API.GetSingleton().UpdateProfile(PlayerPrefs.GetString("UserId"), profile,
              (err, text) =>
              {
                  if (text == "success")
                  {
                      Dirty = false;
                  }
                  else
                  {
                      Remaining = UpdateInterval;
                  }
              });
        }
    }

}
