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

    // Start is called before the first frame update
    public void Initialize(LoginStatusData loginStatusData)
    {
        Refresh(loginStatusData);
    }

    public void Refresh(LoginStatusData loginStatusData)
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
                    LightImage.texture = loginStatusData.loggedIn ? LoggedInTex : LoggedOutTex;
                    LabelText.text = loginStatusData.loggedIn ? LoginString + loginStatusData.id : LogoutString;
                    return 0;
                }
                else
                {
                    LightImage.texture = LoggedOutTex;
                    LabelText.text = LogoutString;
                    return 0;
                }
            });
    }

    public void ForceLoggedIn(string UserId)
    {
        LightImage.texture = LoggedInTex;
        LabelText.text = "Forced";
    }

}
