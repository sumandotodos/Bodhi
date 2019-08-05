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

    public Coroutine GET(string url, System.Func<string, string, int> callback)
    {
        UnityWebRequest res = UnityWebRequest.Get(url);
        return StartCoroutine(REST_Coroutine(res, url, callback));
    }

    public Coroutine DELETE(string url, System.Func<string, string, int> callback)
    {
        UnityWebRequest res = UnityWebRequest.Delete(url);
        return StartCoroutine(REST_Coroutine(res, url, callback));
    }

    public Coroutine PUT(string url, string body, System.Func<string, string, int> callback)
    {
        UnityWebRequest res = UnityWebRequest.Put(url, body);
        return StartCoroutine(REST_Coroutine(res, url, callback));
    }

    public Coroutine POST(string url, string body, System.Func<string, string, int> callback)
    {
        UnityWebRequest res = UnityWebRequest.Post(url, body);
        return StartCoroutine(REST_Coroutine(res, url, callback));
    }

    IEnumerator REST_Coroutine(UnityWebRequest res, string url, System.Func<string, string, int> callback)
    {
        if (Headers != null)
        {
            foreach (KeyValuePair<string, string> entry in Headers)
            {
                res.SetRequestHeader(entry.Key, entry.Value);
            }
        }
        //bool Succeded = false;
        //do
        //{
            yield return res.SendWebRequest();
        //  if(res.error != null)
        // {
        //    yield return new WaitForSeconds(RetryTimeout);
        // }
        // else
        // {
        //     Succeded = true;
        // }
        //} while (!Succeded);
        if (callback != null)
        {
            callback(res.error, res.downloadHandler.text);
        }
    }





}
