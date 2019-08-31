using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnreadMessagesUpdater : MonoBehaviour
{
    public TMPro.TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        int UnreadMessages = PlayerPrefs.GetInt("UnreadMessages");
        textMesh.text = "" + UnreadMessages;
        textMesh.enabled = false;
        StartCoroutine(GetUnreadMessagesCoroutine());
    }

    IEnumerator GetUnreadMessagesCoroutine()
    {
        string currentUser = PlayerPrefs.GetString("UserId");
        int UnreadMessages = 0;
        yield return API.GetSingleton().GetUnreadMessagesCount(currentUser,
        (err, text) =>
        {
            IntResult result = JsonUtility.FromJson<IntResult>(text);
            UnreadMessages = result.result;
        });
        textMesh.text = "" + UnreadMessages;
        PlayerPrefs.SetInt("UnreadMessages", UnreadMessages);
        textMesh.enabled = true;
    }
}
