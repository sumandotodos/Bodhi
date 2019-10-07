﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*TODO: subclass MessageSlab into it's different types or this is going to get very dirty*/
public class MessageSlab : Slab
{
    public RectTransform PlayButton;
    public string videoFullPathId;

    public override void SetHeight(float _Height)
    {
        //base.SetHeight(_Height);
        Height = _Height;
        Vector2 d = BackgroundImage.rectTransform.sizeDelta;
        d.y = Height + 130.0f;
        BackgroundImage.rectTransform.sizeDelta = d;
        //BackgroundImage.rectTransform.sizeDelta.y = Height;
        d = FrameImage.rectTransform.sizeDelta;
        d.y = Height + 130.0f;
        FrameImage.rectTransform.sizeDelta = d;
        // FrameImage.rectTransform.sizeDelta.y = Height;
        d = TextComponent.rectTransform.sizeDelta;
        d.y = Height + 130.0f;
        TextComponent.rectTransform.sizeDelta = d;
        //TextComponent.preferredHeight = Height;

        Vector2 pos = PlayButton.anchoredPosition;
        pos.y = -(_Height + 130.0f) + 350.0f;
        PlayButton.anchoredPosition = pos;
    }

    public void TouchOnDownloadAndPlayVideo()
    {
        Debug.Log("Downloading: " + extra);
        ReceiveVideoResponseController.GetSingleton().SetOriginalQuestion(question);
        ReceiveVideoResponseController.GetSingleton().DownloadAndPlayVideoResponse(extra);
    }

    public void TouchOnGoToUserProfile()
    {
        PlayerPrefs.SetString("FavoriteType", Heart.FavTypeToString(TypeOfContent.Any));
        StartCoroutine(_PlanetsToFavoritesSequence("PersonProfile"));
    }

    IEnumerator _PlanetsToFavoritesSequence(string scene)
    {
        UIFader fader = GameObject.Find("Fader").GetComponent<UIFader>();
        PlayerPrefs.SetString("OtherUserId", fromuserid);
        yield return new WaitForSeconds(0.3f);
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync(scene);
    }
}