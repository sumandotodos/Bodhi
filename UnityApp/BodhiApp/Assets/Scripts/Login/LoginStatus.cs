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
                Debug.Log("<color=purple> Bodhi user id: " + res + "</color>");
                LightImage.texture = loginStatusData.loggedIn ? LoggedInTex : LoggedOutTex;
                LabelText.text = loginStatusData.loggedIn ? LoginString + loginStatusData.id : LogoutString;
                return 0;
            });
    }

}
