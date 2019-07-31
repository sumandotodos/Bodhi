using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class LoginStatusData
{
    public bool loggedIn;
    public string FBid;
    public string IGid;
    public string AppToken;
    public string id;

    public LoginStatusData()
    {
        loggedIn = false;
        FBid = "";
        IGid = "";
        AppToken = "";
        id = "";
    }

}

public class LoginController : MonoBehaviour
{

    public LoginStatus loginStatus;

    public UIFader FBfader;
    public UIFader GIFader;
    public UIFader LogoutFader;
    public UIGeneralFader goForwardFader;
    public UIOpacityWiggle goForwardOpWiggle;

    LoginStatusData loginStatusData;

    public void Initialize()
    {
        loadLoginData();
        loginStatus.Initialize(loginStatusData);
        LoginConfigurations.init();
    }

    IEnumerator _ShowLoginButtonsSequence()
    {
        FBfader.fadeToOpaque();
        yield return new WaitForSeconds(0.25f);
        GIFader.fadeToOpaque();
    }

    IEnumerator _ShowLogoutButtonsSequence()
    {
        goForwardFader.fadeToOpaque();
        yield return new WaitForSeconds(0.5f);
        goForwardOpWiggle.isActive = true;
        yield return new WaitForSeconds(0.25f);
        LogoutFader.fadeToOpaque();
        yield return new WaitForSeconds(0.25f);
    }

    IEnumerator _HideLoginButtonsSequence()
    {
        FBfader.fadeToTransparent();
        yield return new WaitForSeconds(0.25f);
        GIFader.fadeToTransparent();
    }

    IEnumerator _HideLogoutButtonsSequence()
    {
        goForwardOpWiggle.isActive = false;
        goForwardFader.fadeToTransparent();
        yield return new WaitForSeconds(0.25f);
        LogoutFader.fadeToTransparent();
        yield return new WaitForSeconds(0.25f);
    }


    public void ShowLoginInterface()
    {
        StartCoroutine(_ShowLoginButtonsSequence());
    }

    public void ShowLogoutInterface()
    {
        StartCoroutine(_ShowLogoutButtonsSequence());
    }

    public void HideLoginInterface()
    {
        StartCoroutine(_HideLoginButtonsSequence());
    }

    public void HideLogoutInterface()
    {
        StartCoroutine(_HideLogoutButtonsSequence());
    }

    public void ShowInterface()
    {
        if(loginStatusData.loggedIn)
        {
            ShowLogoutInterface();
        }
        else
        {
            ShowLoginInterface();
        }
    }

    public void saveLoginData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save000.dat", FileMode.Create);
        formatter.Serialize(file, loginStatusData);
        file.Close();
    }

    public void loadLoginData()
    {
        if (File.Exists(Application.persistentDataPath + "/save000.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save000.dat", FileMode.Open);
            loginStatusData = (LoginStatusData)formatter.Deserialize(file);
            file.Close();
        }
        else
        {
            loginStatusData = new LoginStatusData();
        }
    }

    public void LogInWithFB(string FBid, string id)
    {
        HideLoginInterface();
        ShowLogoutInterface();
        loginStatusData.loggedIn = true;
        loginStatusData.id = id;
        loginStatusData.FBid = FBid;
        loginStatusData.AppToken = FBid;
        loginStatus.Refresh(loginStatusData);
        saveLoginData();
    }

    public void LogInWithIG(string IGid, string id)
    {
        HideLoginInterface();
        ShowLogoutInterface();
        loginStatusData.loggedIn = true;
        loginStatusData.id = id;
        loginStatusData.IGid = IGid;
        loginStatusData.AppToken = IGid;
        loginStatus.Refresh(loginStatusData);
        saveLoginData();
    }

    public void LogoutButtonTouch()
    {
        HideLogoutInterface();
        ShowLoginInterface();
        loginStatusData.loggedIn = false;
        loginStatus.Refresh(loginStatusData);
        saveLoginData();
    }

}
