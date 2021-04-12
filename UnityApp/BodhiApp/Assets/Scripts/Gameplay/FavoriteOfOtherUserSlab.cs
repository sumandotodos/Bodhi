using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteOfOtherUserSlab : Slab
{
    public RectTransform ReplyButton;

    public override void SetHeight(float _Height)
    {
        //base.SetHeight(_Height);
        Height = _Height;                                               
        Vector2 d = BackgroundImage.rectTransform.sizeDelta;
        d.y = Height * 1.06f + 155.0f;
        BackgroundImage.rectTransform.sizeDelta = d;
        //BackgroundImage.rectTransform.sizeDelta.y = Height;
        d = FrameImage.rectTransform.sizeDelta;
        d.y = Height * 1.06f + 155.0f;
        FrameImage.rectTransform.sizeDelta = d;
        // FrameImage.rectTransform.sizeDelta.y = Height;
        d = TextComponent.rectTransform.sizeDelta;
        d.y = Height * 1.06f + 155.0f;
        TextComponent.rectTransform.sizeDelta = d;
        //TextComponent.preferredHeight = Height;

        Vector2 pos = ReplyButton.anchoredPosition;
        pos.y = -(_Height * 1.06f + 155.0f) + 200.0f;
        ReplyButton.anchoredPosition = pos;
    }

    public override float Adjust(float H)
    {
        return H - 35.0f + H / 6.8f + ExtraHeight + 60.0f;
    }

    public void TouchOnSendVideoResponse()
    {
        Debug.Log("QuestionId: " + id);
        Debug.Log("ToUser: " + extra);
        SendVideoResponseController.TouchOnSendVideoResponse(extra, question, id);
    }
}
