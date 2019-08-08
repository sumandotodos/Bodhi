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

    public Coroutine PostComment(string userid, string body, System.Func<string, string, int> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/comment";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().POST(url, body, callback);
    }

    public Coroutine GetFavoritesList(string userid, System.Func<string, string, int> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/favorites";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, callback);
    }

    public void IsFavorite(string userid, string contentid, System.Func<bool, int> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/favorite/" + contentid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().GET(url, (err, text) =>
        {
            RESTResult_Bool IsFav = JsonUtility.FromJson<RESTResult_Bool>(text);
            callback(IsFav.result);
            return 0;
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
                return 0; });
    }

    public void DestroyFavorite(string userid, string contentid)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/favorite/" + contentid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().DELETE(url, (err, text) => {
            Debug.Log("DELETE error: " + err);
            Debug.Log("DELETE response: " + text);
            return 0;
        });
    }

    public void CreateContent(string userid, string payload, ContentType contentType)
    {

    }
}
