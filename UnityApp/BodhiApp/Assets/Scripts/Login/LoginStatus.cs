using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginStatus : MonoBehaviour
{
    public RawImage LightImage;
    public Text LabelText;

    public string LogoutString = "No estás identificado";
    public string LoginString = "Estás identificado como ";

    public Texture LoggedOutTex;
    public Texture LoggedInTex;

    public void Initialize(LoginStatusData loginStatusData, LoginController lc)
    {
        Refresh(loginStatusData, lc);
    }

    public void Refresh(LoginStatusData loginStatusData, LoginController lc)
    {
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().GET(LoginConfigurations.MakeUserIdLoginRequest(loginStatusData.AppToken),
            (err, res) =>
            {
                if (err == null)
                {
                    //@TODO handle error 
                    RESTResult_String result = JsonUtility.FromJson<RESTResult_String>(res);
                    PlayerPrefs.SetString("UserId", result.result);
                    if (LoginConfigurations.Headers.ContainsKey("userid"))
                    {
                        LoginConfigurations.Headers["userid"] = result.result;
                    }
                    else
                    {
                        LoginConfigurations.Headers.Add("userid", result.result);
                    }
                    Debug.Log("<color=purple>UserId from server: " + result.result + "</color>");
                    if (lc != null)
                    {
                        lc.CheckId = result.result;
                    }
                    LightImage.texture = loginStatusData.loggedIn ? LoggedInTex : LoggedOutTex;
                    LabelText.text = loginStatusData.loggedIn ? LoginString + loginStatusData.id : LogoutString;
                }
                else
                {
                    LightImage.texture = LoggedOutTex;
                    LabelText.text = LogoutString;
                }
            });
    }

    public void ForceLoggedIn(string UserId)
    {
        LightImage.texture = LoggedInTex;
        LabelText.text = "Forced";
    }

}
