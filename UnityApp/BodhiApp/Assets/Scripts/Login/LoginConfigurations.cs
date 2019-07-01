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

    public static string MakeFBTokenFromCodeURL(string code)
    {
        string BaseURL = "https://graph.facebook.com/v3.3/oauth/access_token?client_id=CLIENT-ID&redirect_uri=REDIRECT-URI&client_secret=CLIENT-SECRET&code=CODE";
        return BaseURL.Replace("CLIENT-ID", FBClient)
            .Replace("REDIRECT-URI", FlygamesRedirectURL)
            .Replace("CLIENT-SECRET", FBSecret)
            .Replace("CODE", code);
    }

    public static string MakeFBAuthURL()
    {
        string BaseURL = FacebookAuthURL + "?client_id=CLIENT-TOKEN&redirect_uri=REDIRECT-URI&state=STATE-PARAM";
        return BaseURL.Replace("CLIENT-TOKEN", FBClient)
         .Replace("REDIRECT-URI", FlygamesRedirectURL)
         .Replace("STATE-PARAM", FBStateParam);
    }

    public static void SaveAccessTokens()
    {

    }

}
