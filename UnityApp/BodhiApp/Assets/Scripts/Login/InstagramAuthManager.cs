using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class IGUser
{
    public string id;
    public string username;
    public string full_name;
    public string profile_picture;
}

[System.Serializable]
public class IGTokenResponse
{
    public string access_token;
    public IGUser user;
}

public class InstagramAuthManager : MonoBehaviour
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

    private string CodeFromIG;
    private string TokenFromIG;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginWithInstagramButton()
    {
        string URL = LoginConfigurations.MakeIGAuthURL();
        webView.OpenWebView(URL, MessageCallback);
    }

    public void LogoutWithInstagramButton()
    {
        webView.OpenWebView(LoginConfigurations.InstagramLogoutURL, null);
        StartCoroutine(WaitABitAndClose());
    }

    IEnumerator WaitABitAndClose()
    {
        yield return new WaitForSeconds(2.5f);
        webView.CloseWebView();
    }

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
                string code = msg.Substring(prefix.Length+codeQuery.Length);
                Debug.Log("Code from instagram: " + code);
                codeLabel.text = code;
                CodeFromIG = code;
                StartCoroutine(ExchangeCodeForToken());
            }
            else if (msg.StartsWith(prefix+errorQuery))
            {
                Debug.Log("Hubo un erroraco");
            }

        }
        return msg;
    }

    IEnumerator ExchangeCodeForToken()
    {
        WWWForm form = new WWWForm();
        form.AddField("client_secret", LoginConfigurations.IGSecret);
        form.AddField("client_id", LoginConfigurations.IGClient);
        form.AddField("grant_type", "authorization_code");
        form.AddField("redirect_uri", LoginConfigurations.FlygamesRedirectURL);
        form.AddField("code", CodeFromIG);


        UnityWebRequest res = UnityWebRequest.Post(LoginConfigurations.InstagramTokenURL, form);
        yield return res.SendWebRequest();

        Debug.Log(res.downloadHandler.text);
        IGTokenResponse tokenResponse = JsonUtility.FromJson<IGTokenResponse>(res.downloadHandler.text);
        TokenFromIG = tokenResponse.access_token;
        Debug.Log("<color=blue>" + TokenFromIG + "</color>");

    }

    IEnumerator CheckIfTokenValid()
    {
        UnityWebRequest res = UnityWebRequest.Get(LoginConfigurations.MakeIGTokenCheckURL(TokenFromIG));
        yield return res.SendWebRequest();

        if(res.downloadHandler.text.Contains("OAuthAccessTokenException"))
        {
            Debug.Log("<color=red>Token invalid!</color>");
        }
        else
        {
            Debug.Log("<color=green>Token valid</color>");
        }
    }
}
