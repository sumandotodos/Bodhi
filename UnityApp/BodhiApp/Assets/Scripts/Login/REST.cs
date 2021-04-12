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

[System.Serializable]
public class RequestData
{
    public System.Func<string, string, byte[], UnityWebRequest> Starter;
    public string url;
    public string body;
    public byte[] bodyData;
    public RequestData(string _url, string _body, byte[] _bodyData, System.Func<string, string, byte[], UnityWebRequest> _Starter)
    {
        url = _url;
        body = _body;
        bodyData = _bodyData;
        Starter = _Starter;
    }
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

    private UnityWebRequest GETRequest(string url, string body, byte[] bodyData)
    {
        return UnityWebRequest.Get(url);
    }
    public Coroutine GET(string url, System.Action<string, string> callback)
    {
        RequestData rd = new RequestData(url, "", null, GETRequest);
        return StartCoroutine(REST_Coroutine(rd, url, null, callback));
    }

    private UnityWebRequest GET_BinaryRequest(string url, string body, byte[] bodyData)
    {
        return UnityWebRequest.Get(url);
    }
    public Coroutine GET_Binary(string url, System.Action<uint, float> progressCallback, System.Action<string, byte[]> callback)
    {
        RequestData rd = new RequestData(url, "", null, GET_BinaryRequest);
        return StartCoroutine(REST_Binary_Coroutine(rd, url, progressCallback, callback));
    }

    private UnityWebRequest DELETERequest(string url, string body, byte[] bodyData)
    {
        return UnityWebRequest.Delete(url);
    }
    public Coroutine DELETE(string url, System.Action<string, string> callback)
    {
        RequestData rd = new RequestData(url, "", null, DELETERequest);
        return StartCoroutine(REST_Coroutine(rd, url, null, callback));
    }

    private UnityWebRequest PUTRequestBody(string url, string body, byte[] bodyData)
    {
        return UnityWebRequest.Put(url, body);
    }
    public Coroutine PUT(string url, string body, System.Action<string, string> callback)
    {
        RequestData rd = new RequestData(url, body, null, PUTRequestBody);
        return StartCoroutine(REST_Coroutine(rd, url, null, callback));
    }

    private UnityWebRequest PUTRequestBodyData(string url, string body, byte[] bodyData)
    {
        return UnityWebRequest.Put(url, bodyData);
    }
    public Coroutine PUT(string url, byte[] bodyData, System.Action<string, string> callback)
    {
        RequestData rd = new RequestData(url, "", bodyData, PUTRequestBodyData);
        return StartCoroutine(REST_Coroutine(rd, url, null, callback));
    }
    public Coroutine PUT(string url, byte[] bodyData, System.Action<uint, float> progressCallback, System.Action<string, string> callback)
    {
        RequestData rd = new RequestData(url, "", bodyData, PUTRequestBodyData);
        return StartCoroutine(REST_Coroutine(rd, url, progressCallback, callback));
    }

    private UnityWebRequest POSTRequest(string url, string body, byte[] bodyData)
    {
        WWWForm form = new WWWForm();
        form.AddField("payload", body);
        return UnityWebRequest.Post(url, form);
    }
    public Coroutine POST(string url, string body, System.Action<string, string> callback)
    {
        RequestData rd = new RequestData(url, body, null, POSTRequest);
        return StartCoroutine(REST_Coroutine(rd, url, null, callback));
    }


    IEnumerator REST_Binary_Coroutine(RequestData rd, string url, System.Action<uint, float> progressCallback, System.Action<string, byte[]> callback)
    {
        UnityWebRequest res = null;

        if (loadWaitController_N != null)
        {
            loadWaitController_N.StartNetworkTransfer();
        }

        bool Succeded = false;
        do
        {
            res = rd.Starter(rd.url, rd.body, rd.bodyData);
            if (Headers != null)
            {
                foreach (KeyValuePair<string, string> entry in Headers)
                {
                    res.SetRequestHeader(entry.Key, entry.Value);
                }
            }
            SetupProgressCallback(res, progressCallback);
            yield return res.SendWebRequest();
            if (res.error != null)
            {
                Debug.Log("There was this error: " + res.error + " for this request: " + res.url);
                res.Abort();
                yield return new WaitForSeconds(RetryTimeout);

            }
            else
            {
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
                    Debug.Log("   >>> REST: res.downloadHandler is null");
                }
            }
            else
            {
                Debug.Log("   >>> REST: res is null");
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




    IEnumerator REST_Coroutine(RequestData rd, string url, System.Action<uint, float> progressCallback, System.Action<string, string> callback)
    {

        UnityWebRequest res = null;

        if (loadWaitController_N!=null)
        {
            loadWaitController_N.StartNetworkTransfer();
        }

        bool Succeded = false;
        do
        {
            res = rd.Starter(rd.url, rd.body, rd.bodyData);
            if (Headers != null)
            {
                foreach (KeyValuePair<string, string> entry in Headers)
                {
                    res.SetRequestHeader(entry.Key, entry.Value);
                }
            }
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
                Succeded = true;
            }
        } while (!Succeded);

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
                    callback(res.error, null);
                }
            }
            else
            {
                callback("Unknown error", null);
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
