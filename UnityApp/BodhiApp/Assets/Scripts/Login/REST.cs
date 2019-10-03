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

    const float ProgressUpdateInterval = 0.5f;

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

    public void AddHeader(string key, string value)
    {
        Headers[key] = value;
    }

    public Coroutine GET(string url, System.Action<string, string> callback)
    {
        UnityWebRequest res = UnityWebRequest.Get(url);
        return StartCoroutine(REST_Coroutine(res, url, null, callback));
    }

    public Coroutine GET_Binary(string url, System.Action<uint, float> progressCallback, System.Action<string, byte[]> callback)
    {
        UnityWebRequest res = UnityWebRequest.Get(url);
        return StartCoroutine(REST_Binary_Coroutine(res, url, progressCallback, callback));
    }

    public Coroutine DELETE(string url, System.Action<string, string> callback)
    {
        UnityWebRequest res = UnityWebRequest.Delete(url);
        return StartCoroutine(REST_Coroutine(res, url, null, callback));
    }

    public Coroutine PUT(string url, string body, System.Action<string, string> callback)
    {
        UnityWebRequest res = UnityWebRequest.Put(url, body);
        return StartCoroutine(REST_Coroutine(res, url, null, callback));
    }

    public Coroutine PUT(string url, byte[] bodyData, System.Action<string, string> callback)
    {
        UnityWebRequest res = UnityWebRequest.Put(url, bodyData);
        return StartCoroutine(REST_Coroutine(res, url, null, callback));
    }

    public Coroutine PUT(string url, byte[] bodyData, System.Action<uint, float> progressCallback, System.Action<string, string> callback)
    {
        UnityWebRequest res = UnityWebRequest.Put(url, bodyData);
        return StartCoroutine(REST_Coroutine(res, url, progressCallback, callback));
    }

    public Coroutine POST(string url, string body, System.Action<string, string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("comment", body);
        UnityWebRequest res = UnityWebRequest.Post(url, form);
        return StartCoroutine(REST_Coroutine(res, url, null, callback));
    }


    IEnumerator REST_Binary_Coroutine(UnityWebRequest res, string url, System.Action<uint, float> progressCallback, System.Action<string, byte[]> callback)
    {
        if (loadWaitController_N != null)
        {
            loadWaitController_N.StartNetworkTransfer();
        }

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
            SetupProgressCallback(res, progressCallback);
            yield return res.SendWebRequest();
            if (res.error != null)
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

        if (callback != null)
        {
            if (res != null)
            {
                if (res.downloadHandler != null)
                {
                    callback(res.error, res.downloadHandler.data);
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

    private void SetupProgressCallback(UnityWebRequest wr, System.Action<uint, float> progressCallback)
    {
        if (progressCallback != null)
        {
            if (wr.method == "PUT" || wr.method == "POST")
            {
                StartCoroutine(UploadProgressCoroutine(wr.uploadHandler, progressCallback));
            }
            else
            {
                StartCoroutine(DownloadProgressCoroutine(wr, progressCallback));
            }
        }
    }




    IEnumerator REST_Coroutine(UnityWebRequest res, string url, System.Action<uint, float> progressCallback, System.Action<string, string> callback)
    {
        if(loadWaitController_N!=null)
        {
            loadWaitController_N.StartNetworkTransfer();
        }

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
            SetupProgressCallback(res, progressCallback);
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

    IEnumerator UploadProgressCoroutine(UploadHandler h, System.Action<uint, float> progressCallback)
    {
        while(h.progress < 1.0f)
        {
            yield return new WaitForSeconds(ProgressUpdateInterval);
            progressCallback(0, h.progress);
        }
    }

    IEnumerator DownloadProgressCoroutine(UnityWebRequest wr, System.Action<uint, float> progressCallback)
    {
        while (wr.downloadProgress < 1.0f)
        {
            string ContentLengthStr = wr.GetResponseHeader("Content-Length");
            uint ContentLength;
            uint.TryParse(ContentLengthStr, out ContentLength);
            yield return new WaitForSeconds(ProgressUpdateInterval);
            progressCallback(ContentLength, wr.downloadProgress);
        }
    }


}
