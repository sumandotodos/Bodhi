using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MessagesResult
{
    public List<string> result;
}

public class ServerMessagesController : MonoBehaviour
{

    public static ServerMessagesController instance;

    MessagesResult mr;

    public UITip messageTip;

    int MostRecentMessage = 0;

    bool TipMutex = false;
    float remaining = 0.0f;

    IEnumerator GetMessagesFromServerCoRo()
    {
        while (1 < 2)
        {
            yield return new WaitForSeconds(6.0f);

            WWW www;
            yield return www = new WWW(LoginConfigurations.MakeServerBaseURL() + "/messages?from=" + MostRecentMessage, null, LoginConfigurations.Headers);

            if (www.error == null)
            {
                mr = JsonUtility.FromJson<MessagesResult>(www.text);
                if (mr.result.Count > 0)
                {
                    for (int i = 0; i < mr.result.Count; ++i)
                    {
                        while(UITip.TipMutex || TipMutex)
                        {
                            yield return new WaitForSeconds(2.0f);
                        }
                        messageTip.SetMessage(mr.result[i]);
                        messageTip.go();
                        yield return new WaitForSeconds(0.10f);
                        yield return new WaitUntil(messageTip.hasBeenDispatched);
                        yield return new WaitForSeconds(0.05f);
                    }
                    MostRecentMessage += mr.result.Count;
                    LoadSaveController.SaveLatestMessageId(MostRecentMessage);
                }
            }

            yield return new WaitForSeconds(60.0f); // Check every minute

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        MostRecentMessage = LoadSaveController.LoadLatestMessageId();
        StartCoroutine(GetMessagesFromServerCoRo());
    }

    public static void SetDelay(float del)
    {
        instance.TipMutex = true;
        instance.remaining = del;
    }

    private void Update()
    {
        if(remaining > 0.0f)
        {
            remaining -= Time.deltaTime;
            if(remaining <= 0.0f)
            {
                TipMutex = false;
            }
        }
    }


}
