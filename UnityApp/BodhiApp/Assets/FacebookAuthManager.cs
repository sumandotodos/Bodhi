using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class FBTokenResponse
{
    public string access_token;
    public string token_type;
    public string expires_in;
}

public class FacebookAuthManager : MonoBehaviour
{
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

    public Text codeLabel;

    public SampleWebView webView;

    private string CodeFromFB;
    private string TokenFromFB;

    public string MessageCallback(string msg)
    {
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
                codeLabel.text = code;
                CodeFromFB = code;
                StartCoroutine(ExchangeCodeForToken());
            }
            else if (msg.StartsWith(prefix + errorQuery))
            {
                Debug.Log("Hubo un erroraco");
            }

        }
        return msg;
    }


    public void LoginWithFacebookButton()
    {
        string URL = LoginConfigurations.MakeFBAuthURL();
        webView.OpenWebView(URL, MessageCallback);
    }

    public void LogoutWithFacebookButton()
    {
        webView.OpenWebView(LoginConfigurations.InstagramLogoutURL, null);
        StartCoroutine(WaitABitAndClose());
    }

    IEnumerator ExchangeCodeForToken()
    {
        UnityWebRequest res = UnityWebRequest.Get(LoginConfigurations.MakeFBTokenFromCodeURL(CodeFromFB));
        yield return res.SendWebRequest();

        Debug.Log(res.downloadHandler.text);
        FBTokenResponse tokenResponse = JsonUtility.FromJson<FBTokenResponse>(res.downloadHandler.text);
        TokenFromFB = tokenResponse.access_token;
        Debug.Log("<color=blue>" + TokenFromFB + "</color>");

    }

    IEnumerator WaitABitAndClose()
    {
        yield return new WaitForSeconds(2.5f);
        webView.CloseWebView();
    }
}
