using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class RESTResult_String
{
    public string result;
}

[System.Serializable]
public class RESTResult_Int
{
    public int result;
}

[System.Serializable]
public class RESTResult_Bool
{
    public bool result;
}

public class REST : MonoBehaviour
{

    public LoadWaitController loadWaitController_N;

    const float RetryTimeout = 5.0f;

    public static REST instance;

    private void Awake()
    {
        instance = this;
    }

    public static REST GetSingleton()
    {
        return instance;
    }

    Dictionary<string, string> Headers = null;

    public void SetHeaders(Dictionary<string, string> _Headers)
    {
        Headers = _Headers;
    }

    public Coroutine GET(string url, System.Action<string, string> callback)
    {
        UnityWebRequest res = UnityWebRequest.Get(url);
        return StartCoroutine(REST_Coroutine(res, url, callback));
    }

    public Coroutine DELETE(string url, System.Action<string, string> callback)
    {
        UnityWebRequest res = UnityWebRequest.Delete(url);
        return StartCoroutine(REST_Coroutine(res, url, callback));
    }

    public Coroutine PUT(string url, string body, System.Action<string, string> callback)
    {
        UnityWebRequest res = UnityWebRequest.Put(url, body);
        return StartCoroutine(REST_Coroutine(res, url, callback));
    }

    public Coroutine PUT(string url, byte[] bodyData, System.Action<string, string> callback)
    {
        UnityWebRequest res = UnityWebRequest.Put(url, bodyData);
        return StartCoroutine(REST_Coroutine(res, url, callback));
    }

    public Coroutine POST(string url, string body, System.Action<string, string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("comment", body);
        UnityWebRequest res = UnityWebRequest.Post(url, form);
        return StartCoroutine(REST_Coroutine(res, url, callback));
    }

    IEnumerator REST_Coroutine(UnityWebRequest res, string url, System.Action<string, string> callback)
    {
        if(loadWaitController_N!=null)
        {
            loadWaitController_N.StartNetworkTransfer();
        }

        Debug.Log("Doing business @ url: " + url);

        if (Headers != null)
        {
            foreach (KeyValuePair<string, string> entry in Headers)
            {
                res.SetRequestHeader(entry.Key, entry.Value);
            }
        }
        bool Succeded = false;
        do
        {
            yield return res.SendWebRequest();
          if(res.error != null)
         {
                Debug.Log("There was this error: " + res.error);
                res.Abort();
                yield return new WaitForSeconds(RetryTimeout);

         }
         else
         {
                Debug.Log("No error");
                Succeded = true;
         }
        } while (!Succeded);
        Debug.Log("Break out of do while");
        if (callback != null)
        {
            if (res != null)
            {
                if (res.downloadHandler != null)
                {
                    callback(res.error, res.downloadHandler.text);
                }
                else
                {
                    Debug.Log("   >>> REST: res.downloadHandler is shit");
                }
            }
            else
            {
                Debug.Log("   >>> REST: res is shit");
            }
        }

        if (loadWaitController_N != null)
        {
            loadWaitController_N.CompleteNetworkTransfer();
        }

    }





}
