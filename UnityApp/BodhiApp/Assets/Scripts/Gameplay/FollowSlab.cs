using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FollowState { Following, NotFollowing, Pending };

[System.Serializable]
public class FollowSlab : Slab
{
    public Color FollowingColor;
    public Color NotFollowingColor;
    public Image ButtonImage;
    public Text ButtonText;

    public float SlabHeight = 240.0f;
    public float SlabMargin = 60.0f;

    public override void SetHeight(float _Height)
    {
        _Height = SlabHeight;
        base.SetHeight(_Height);
    }
    public override float Adjust(float H)
    {
        return SlabHeight + SlabMargin; //base.Adjust(H) + 70.0f;
    }

    string FollowingText = "Estás siguiendo a este usuario";
    string NotFollowingText = "";
    string UnfollowText = "Dejar de seguir";
    string FollowText = "Seguir a este usuario";

    FollowState followState = FollowState.Pending;

    ItemsController itemsController;

    System.Action TouchAction;

    private void Awake()
    {
        itemsController = FindObjectOfType<ItemsController>();

    }

    public void TouchOnButton()
    {
        if(TouchAction!=null)
        {
            TouchAction();
        }
    }

    public void SetFollowState(FollowState state)
    {
        followState = state;
        switch(state)
        {
            case FollowState.Following:
                BackgroundImage.color = FollowingColor;
                ButtonImage.color = NotFollowingColor;
                TextComponent.text = FollowingText;
                ButtonText.text = UnfollowText;
                TouchAction = Unfollow;
                break;
            case FollowState.NotFollowing:
                BackgroundImage.color = NotFollowingColor;
                ButtonImage.color = FollowingColor;
                TextComponent.text = NotFollowingText;
                ButtonText.text = FollowText;
                TouchAction = Follow;
                break;
        }
    }

    public void Unfollow()
    {
        // call API to unfollow
        //itemsController.Dislike(id, Index); // rename to dismiss
        SetFollowState(FollowState.NotFollowing);
    }

    public void Follow()
    {
        // call API to follow, callback to change color
        SetFollowState(FollowState.Following);
    }
}
