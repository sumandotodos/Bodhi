using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class FBUserInfo
{
    public string name;
    public string email;
}

[System.Serializable]
public class FBTokenResponse
{
    public string access_token;
    public string token_type;
    public string expires_in;
}

[System.Serializable]
public class FBTokenInspectionResponseData
{
    public string app_id;
    public bool is_valid;
    public string user_id;
}

[System.Serializable]
public class FBTokenInspectionResponse
{
    public FBTokenInspectionResponseData data;
}
/*
{
    "data": {
        "app_id": 138483919580948, 
        "type": "USER",
        "application": "Social Cafe", 
        "expires_at": 1352419328, 
        "is_valid": true, 
        "issued_at": 1347235328, 
        "metadata": {
            "sso": "iphone-safari"
        }, 
        "scopes": [
            "email",
            "publish_actions"
        ], 
        "user_id": "1207059"
    }
}*/

public class FacebookAuthManager : MonoBehaviour
{
    public LoginController loginController;

    private static int NumberOfInstances = 0;
    private void Awake()
    {
        ++NumberOfInstances;
        if (NumberOfInstances > 1)
        {
            Debug.Log("<color=red>Error: Singleton pattern violation</color>");
            DestroyImmediate(this.gameObject);
        }
    }

    public SampleWebView webView;

    private string CodeFromFB;
    private string TokenFromFB;

    public string MessageCallback(string msg)
    {
        Debug.Log("<color=red>Callback msg: " + msg + "</color>");
        string prefix = LoginConfigurations.FlygamesRedirectURL;
        string codeQuery = "?code=";
        string errorQuery = "?error=";
        if (msg.StartsWith(prefix))
        {
            webView.DestroyWebView();
            if (msg.StartsWith(prefix + codeQuery))
            {
                string code = msg.Substring(prefix.Length + codeQuery.Length);
                Debug.Log("Code from facebook: " + code);
                CodeFromFB = code;
                StartCoroutine(ExchangeCodeForToken());
            }
            else if (msg.StartsWith(prefix + errorQuery))
            {

            }

        }
        return msg;
    }


    public void LoginWithFacebookButton()
    {
        StartCoroutine(LoginWithFacebookIfNetworkAvailable());
    }

    public void LogoutWithFacebookButton()
    {
        webView.OpenWebView(LoginConfigurations.InstagramLogoutURL, null);
        StartCoroutine(WaitABitAndClose());
    }

    IEnumerator LoginWithFacebookIfNetworkAvailable()
    {
        bool NetworkAlive = false;
        yield return API.GetSingleton().Healthcheck((alive) =>
        {
            NetworkAlive = alive;
        });
        if(NetworkAlive)
        {
            string URL = LoginConfigurations.MakeFBAuthURL();
            webView.OpenWebView(URL, MessageCallback);
        }
    }

    IEnumerator ExchangeCodeForToken()
    {
        UnityWebRequest res = UnityWebRequest.Get(LoginConfigurations.MakeFBTokenFromCodeURL(CodeFromFB));
        yield return res.SendWebRequest();

        Debug.Log(res.downloadHandler.text);
        FBTokenResponse tokenResponse = JsonUtility.FromJson<FBTokenResponse>(res.downloadHandler.text);
        TokenFromFB = tokenResponse.access_token;
        Debug.Log("<color=blue>" + TokenFromFB + "</color>");
        //loginController.LogInWithFB(TokenFromFB, TokenFromFB);

        res = UnityWebRequest.Get(LoginConfigurations.MakeFBTokenVerifyURL(TokenFromFB));
        yield return res.SendWebRequest();

        Debug.Log("<color=purple>" + res.downloadHandler.text + "</color>");
        FBTokenInspectionResponse response = JsonUtility.FromJson<FBTokenInspectionResponse>(res.downloadHandler.text);
        string userid = response.data.user_id;

        res = UnityWebRequest.Get(LoginConfigurations.MakeFBGetUserInfoURL(userid, TokenFromFB));
        yield return res.SendWebRequest();

        Debug.Log("<color=green>" + res.downloadHandler.text + "</color>");
        FBUserInfo userInfo = JsonUtility.FromJson<FBUserInfo>(res.downloadHandler.text);
        loginController.LogInWithFB(userid, userInfo.name);
    }

   /* IEnumerator VerifyToken()
    {
        UnityWebRequest res = UnityWebRequest.Get(LoginConfigurations.MakeFBTokenVerifyURL(TokenFromFB));
        yield return res.SendWebRequest();

        FBTokenInspectionResponse response = JsonUtility.FromJson<FBTokenInspectionResponse>(res.downloadHandler.text);

    }*/

    IEnumerator WaitABitAndClose()
    {
        yield return new WaitForSeconds(2.5f);
        webView.CloseWebView();
    }
}
