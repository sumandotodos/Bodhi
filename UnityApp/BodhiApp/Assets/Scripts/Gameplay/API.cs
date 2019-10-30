﻿using System.Collections;
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

    public Coroutine Healthcheck(System.Action<bool> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/healthcheck";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, response) =>
        {
            callback(err == null);
        });
    }

    public Coroutine GetUploadUrl(string userid, System.Action<string, UploadURL> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/uploadurl";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, response) => {
            UploadURL urlObject = JsonUtility.FromJson<UploadURL>(response);
            callback(err, urlObject);
        });
    }

    public Coroutine GetDownloadUrl(string userid, string remoteFilePath, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/downloadurl/" + Utils.SlashSeparatedToColonSeparated(remoteFilePath);
        Debug.Log("The API url: " + url);
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, response) => {
            UploadURL urlObject = JsonUtility.FromJson<UploadURL>(response);
            callback(urlObject.error, urlObject.url);
        });
    }

    public Coroutine GetProfile(string userid, System.Action<string, UserProfile> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/user/profile/" + userid;
       
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);

        return REST.GetSingleton().GET(url, (err, response) =>
        {
        
            UserProfile Result = JsonUtility.FromJson<UserProfile>(response);
        
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
            "/item/comment";
        PostCommentData data = new PostCommentData();
        data.prefix = prefix;
        data.content = body;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        //REST.GetSingleton().AddHeader("content-type", "application/json");
        return REST.GetSingleton().POST(url, JsonUtility.ToJson(data), callback);
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

    public Coroutine PutAvatar(string userid, byte[] data, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/avatar";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().AddHeader("content-type", "application/octet-stream");
        return REST.GetSingleton().PUT(url, data, callback);
    }

    public Coroutine UploadVideoResponse(
        string userid, 
        string touserid, 
        string questionid,
        byte[] allBytes,
        System.Action<uint, float> updateCallback, 
        System.Action<string, string> responseCallback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/video/" + touserid + "/" + questionid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().AddHeader("content-type", "application/octet-stream");
        return REST.GetSingleton().PUT(url, allBytes, updateCallback, responseCallback);
    }


    public Coroutine GetUserProfileAndQuestion(
        string userid, 
        User u, 
        System.Action<string, User, ProfileAndQuestion> callback)
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
        return REST.GetSingleton().GET_Binary(url, null, (err, data) =>
        {
            if (data.Length > 10)
            {
                Texture2D newTexture = new Texture2D(2, 2);
                bool success = ImageConversion.LoadImage(newTexture, data);
                callback(err, success, newTexture);
            }
            else
            {
                callback(err, false, null);
            }
        });
    }

    public Coroutine GetItemContent(string itemid, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/" + itemid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, callback);
    }

    public Coroutine GetFavoritesList(string userid, System.Action<string, FavoritesListResult> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/favorites/" + userid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, data) =>
        {
            FavoritesListResult favs = JsonUtility.FromJson<FavoritesListResult>(data);
            callback(err, favs);
        });
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

    public Coroutine GetContributionsList(string userid, System.Action<string, ItemListResult> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
          "/item/comments/" + userid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, data) =>
        {
            ItemListResult result = JsonUtility.FromJson<ItemListResult>(data);
            callback(err, result);
        });
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

    public Coroutine DeleteVideo(string fullpath, System.Action<string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/item/video/" + fullpath.Replace('/', ':');
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().AddHeader("content-type", "application/json");
        Fullpath newFullpath = new Fullpath();
        newFullpath.fullpath = fullpath;
        return REST.GetSingleton().DELETE(url, (err, text) =>
        {
            callback(text);
        });
    }

    public void CreateMessage(string fromUserId, string toUserId, string type, string content, string extra)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/message/";
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().AddHeader("content-type", "application/json");
        Message newMessage = new Message();
        newMessage._fromuserid = fromUserId;
        newMessage._touserid = toUserId;
        newMessage.content = content;
        newMessage.extra = extra;
        REST.GetSingleton().POST(url, JsonUtility.ToJson(newMessage), (err, response) =>
          {
              Debug.Log("Post response: " + response);
          });
    }

    public Coroutine GetFollowedUsers(string userid, int skip, int maxUsers, System.Action<string, UserListResult> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/follow/" + skip + "/" + maxUsers;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, response) =>
        {
            UserListResult listOfUsers = JsonUtility.FromJson<UserListResult>(response);
            callback(err, listOfUsers);
        });
    }

    public Coroutine IsFollowing(string userid, string followuserid, System.Action<string, bool> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/follow/" + followuserid;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, response) =>
        {
            if (err != null)
            {
                callback(err, false);
            }
            else
            {
                RESTResult_Bool result = JsonUtility.FromJson<RESTResult_Bool>(response);
                callback(err, result.result);
            }
        });
    }

    public void FollowUser(string userid, string followuserid, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/follow/" + followuserid;
        Debug.Log("POST url: " + url);
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().POST(url, "", (err, response) =>
        {
            callback(err, response);
        });
    }

    public void UnfollowUser(string userid, string followuserid, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/follow/" + followuserid;
        Debug.Log("Unfollow DELETE url: " + url);
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        REST.GetSingleton().DELETE(url, (err, response) =>
        {
            Debug.Log("Unfollow DELETE response: " + response);
            if (err != null)
            {
                Debug.Log("Unfollow DELETE err: " + err);
            }
            callback(err, response);
        });
    }

    public Coroutine GetCommsPreference(string fromuser, string touser, System.Action<string, int> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/follow/commprefs/" + fromuser + "/" + touser;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().GET(url, (err, response) =>
        {
            RESTResult_Int value = JsonUtility.FromJson<RESTResult_Int>(response);
            callback(err, value.result);        
        });
    }

    public Coroutine SetCommsPreference(string fromuser, string touser, int index, System.Action<string, string> callback)
    {
        string url = LoginConfigurations.MakeServerBaseURL() + "/" + LoginConfigurations.APIVersion +
            "/follow/commprefs/" + touser + "/" + index;
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        return REST.GetSingleton().PUT(url, "body", (err, response) =>
        {
            callback(err, response);
        });
    }

}
