using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TokensInfo {
    public string IGToken;
    public string FBToken;
}

public class LoginConfigurations : MonoBehaviour
{
    public static string APIVersion = "v1";

    public static string PSK = "vQb9BpkcLGQWlmAild4B";
    public static Dictionary<string, string> Headers = new Dictionary<string, string>();
    public static string Version = "V 1.0.0";
    public static void init()
    {
        if (!Headers.ContainsKey("psk"))
        {
            Headers.Add("psk", PSK);
        }
    }

    /* Instagram app code & secret */
    public static string IGClient = "66ad8286ae5446f0ace3ef0fb4b695cc";
    public static string IGSecret = "1f22db5493244b38a5ab94def0773b87";

    /* Instagram auth/login API */
    public static string InstagramAuthURL = "https://api.instagram.com/oauth/authorize";
    public static string InstagramLogoutURL = "https://instagram.com/accounts/logout";
    public static string InstagramTokenURL = "https://api.instagram.com/oauth/access_token";
    public static string InstagramCheckTokenURL = "https://api.instagram.com/v1/users/self/?access_token=ACCESS-TOKEN";

    /* Facebook app code & secret */
    public static string FBClient = "502179840150782";
    public static string FBSecret = "8173e2f6ad176da295576a2955823077";

    /* Facebook auth / login API */
    public static string FacebookAuthURL = "https://www.facebook.com/v3.3/dialog/oauth";
    public static string FacebookLogoutURL = "https://www.facebook.com/";
    public static string FBStateParam = "tumadrecomeca31416";

    public static string FlygamesRedirectURL = "https://apps.flygames.org/auth-redirect/";


    public static string MakeIGAuthURL()
    {
        string BaseURL = InstagramAuthURL + "/?client_id=CLIENT-ID&redirect_uri=REDIRECT-URI&response_type=code";
        return BaseURL.Replace("CLIENT-ID", IGClient).Replace("REDIRECT-URI", FlygamesRedirectURL);
    }

    public static string MakeIGTokenCheckURL(string token)
    {
        return InstagramCheckTokenURL.Replace("ACCESS-TOKEN", token);
    }

    public static string MakeUserIdLoginRequest(string token)
    {
        string uuid = SystemInfo.deviceUniqueIdentifier;
        return MakeServerBaseURL() + "/"+APIVersion+"/login/user/" + uuid + "/" + token;
    }

    public static string MakeServerBaseURL()
    {
        return "https://apps.flygames.org/volare";
    }

    public static string MakeFBTokenFromCodeURL(string code)
    {
        string BaseURL = "https://graph.facebook.com/v3.3/oauth/access_token?client_id=CLIENT-ID&redirect_uri=REDIRECT-URI&client_secret=CLIENT-SECRET&code=CODE";
        return BaseURL.Replace("CLIENT-ID", FBClient)
            .Replace("REDIRECT-URI", FlygamesRedirectURL)
            .Replace("CLIENT-SECRET", FBSecret)
            .Replace("CODE", code);
    }

    public static string MakeFBTokenVerifyURL(string token)
    {
        string BaseURL = "https://graph.facebook.com/v3.3/debug_token?input_token=TOKEN&access_token=CLIENT-ID";
        return BaseURL.Replace("CLIENT-ID", FBClient + "|" + FBSecret)
            .Replace("TOKEN", token);
    }

    public static string MakeFBAuthURL()
    {
        string BaseURL = FacebookAuthURL + "?client_id=CLIENT-TOKEN&redirect_uri=REDIRECT-URI&state=STATE-PARAM";
        return BaseURL.Replace("CLIENT-TOKEN", FBClient)
         .Replace("REDIRECT-URI", FlygamesRedirectURL)
         .Replace("STATE-PARAM", FBStateParam);
    }

    public static string MakeFBGetUserInfoURL(string id, string accessToken)
    {
        string BaseURL = "https://graph.facebook.com/v3.3/ID?access_token=ACCESS-TOKEN";
        return BaseURL.Replace("ACCESS-TOKEN", accessToken).Replace("ID", id);
    }

    public static void SaveAccessTokens()
    {

    }

}
