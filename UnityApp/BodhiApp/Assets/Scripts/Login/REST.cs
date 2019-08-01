using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class REST : MonoBehaviour
{
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

    public void GET(string url, System.Func<string, string, int> callback)
    {
        StartCoroutine(GET_Coroutine(url, callback));
    }

    public void DELETE(string url, System.Func<string, string, int> callback)
    {

    }

    public void PUT(string url, string body, System.Func<string, string, int> callback)
    {

    }

    public void POST(string url, string body, System.Func<string, string, int> callback)
    {

    }

    IEnumerator GET_Coroutine(string url, System.Func<string, string, int> callback)
    {
        UnityWebRequest res = UnityWebRequest.Get(url);
        if (Headers != null)
        {
            foreach (KeyValuePair<string, string> entry in Headers)
            {
                res.SetRequestHeader(entry.Key, entry.Value);
            }
        }
        yield return res.SendWebRequest();
        callback(res.error, res.downloadHandler.text);
    }

}
