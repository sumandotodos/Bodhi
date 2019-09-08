using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContentType { Comment, Video, Echo };

public class API : MonoBehaviour
{
    public static API instance;

    private void Awake()
    {
        instance = this;
    }

    public static API GetSingleton()
    {
        return instance;
    }

    public void DeleteComment(string userid, string contentid)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/comment/" + contentid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().DELETE(url, (err, text) => {
            Debug.Log("DELETE error: " + err);
            Debug.Log("DELETE response: " + text);
        });
    }

    public Coroutine GetProfile(string userid, System.Action<string, UserProfile> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/user/profile";
        Debug.Log("<color=orange>5</color>");
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        Debug.Log("<color=orange>6</color>");
        return REST.GetSingleton().GET(url, (err, response) =>
        {
            Debug.Log("<color=orange>Returned from get user profile</color>");
            UserProfile Result = JsonUtility.FromJson<UserProfile>(response);
            Debug.Log("<color=orange>parsed profile</color>");
            callback(err, Result);
        });
    }

    public Coroutine UpdateProfile(string userid, UserProfile profile, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/user/profile";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().AddHeader("content-type", "application/json");
        string profileString = JsonUtility.ToJson(profile);
        return REST.GetSingleton().PUT(url, profileString, (err, response) =>
        {
            RESTResult_String Result = JsonUtility.FromJson<RESTResult_String>(response);
            callback(err, Result.result);
        });
    }

    public Coroutine GetHandle(string userid, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/login/handle/" + userid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, response) => 
            {
                RESTResult_String Result = JsonUtility.FromJson<RESTResult_String>(response);
                callback(err, Result.result);
            });
    }

    public Coroutine UpdateHandle(string userid, string newHandle, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/login/handle";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().AddHeader("content-type", "text/plain");
        return REST.GetSingleton().PUT(url, newHandle, callback);
    }

    public Coroutine PostComment(string userid, string body, string prefix, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/comment/"+prefix;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().POST(url, body, callback);
    }

    public Coroutine PutAvatar(string userid, byte[] data, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/avatar";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().AddHeader("content-type", "application/octet-stream");
        return REST.GetSingleton().PUT(url, data, callback);
    }

    public Coroutine GetUserProfileAndQuestion(string userid, User u, System.Action<string, User, ProfileAndQuestion> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/user/profileandquestion/" + userid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, text) => {
            ProfileAndQuestion result = JsonUtility.FromJson<ProfileAndQuestion>(text);
            callback(err, u, result);
        });

    }

    public Coroutine GetAvatar(string userid, System.Action<string, bool, Texture2D> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/avatar/" + userid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET_Binary(url, (err, data) =>
        {
            Texture2D newTexture = new Texture2D(2, 2);
            bool success = ImageConversion.LoadImage(newTexture, data);
            callback(err, success, newTexture);
        });
    }

    public Coroutine GetItemContent(string itemid, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/" + itemid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, callback);
    }

    public Coroutine GetFavoritesList(string userid, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/favorites";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, callback);
    }

    public Coroutine PutFavoriteQuestion(string userid, string fav, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/favoritequestion";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().AddHeader("content-type", "text/plain");
        return REST.GetSingleton().PUT(url, fav, callback);
    }

    public Coroutine GetFollows(string userid, System.Action<string, UserListResult> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/follow";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, data) =>
        {
            UserListResult result = JsonUtility.FromJson<UserListResult>(data);
            callback(err, result);
        });
    }

    public Coroutine GetUserIndex(string userid, System.Action<string, int> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/user/index";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, data) =>
        {
            RESTResult_Int result = JsonUtility.FromJson<RESTResult_Int>(data);
            callback(err, result.result);
        });
    }

    public Coroutine GetRandomUsers(string userid, string session,
        int skip, int maxUsers, System.Action<string, UserListResult> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/follow/randomusers/" + session + "/" + skip + "/" + maxUsers;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, data) =>
        {
            UserListResult result = JsonUtility.FromJson<UserListResult>(data);
            callback(err, result);
        });
    }

    public Coroutine GetUnreadMessagesCount(string userid, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/message/unreadcount";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, callback);
    }

    public Coroutine GetContributionsList(string userid, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/comments";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, callback);
    }

    public Coroutine GetMessagesList(string userid, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/message/";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, callback);
    }

    public void IsFavorite(string userid, string contentid, System.Action<bool> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/favorite/" + contentid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().GET(url, (err, text) =>
        {
            RESTResult_Bool IsFav = JsonUtility.FromJson<RESTResult_Bool>(text);
            callback(IsFav.result);
        });
    }

    public void CreateFavorite(string userid, string contentid)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/favorite/" + contentid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().POST(url, contentid, (err,text) => {
            Debug.Log("POST error: " + err);
            Debug.Log("POST response: " + text);
          });
    }

    public void ReorderFavorite(string userid, int fav1, int fav2)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/exchangefavorite/" + fav1 + "/" + fav2;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().POST(url, "", (err, text) =>
        {
            Debug.Log("Exchange response: " + text);
        });
    }

    public void DestroyFavorite(string userid, string contentid)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/favorite/" + contentid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().DELETE(url, (err, text) => {
            Debug.Log("DELETE error: " + err);
            Debug.Log("DELETE response: " + text);
        });
    }

    public void DeleteMessage(string userid, string messageid)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/message/" + messageid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().DELETE(url, (err, text) => {
            Debug.Log("DELETE error: " + err);
            Debug.Log("DELETE response: " + text);
        });
    }

    public void CreateContent(string userid, string payload, ContentType contentType)
    {

    }
}
