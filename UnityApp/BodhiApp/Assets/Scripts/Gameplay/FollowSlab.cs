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
    public UITextFader ContentFader;

    public float SlabHeight = 240.0f;
    public float SlabMargin = 60.0f;

    PersonProfilePopulator personProfilePopulator;

    public override void SetHeight(float _Height)
    {
        _Height = SlabHeight;
        base.SetHeight(_Height);
    }
    public override float Adjust(float H)
    {
        return SlabHeight + SlabMargin + ExtraHeight; //base.Adjust(H) + 70.0f;
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
        personProfilePopulator = FindObjectOfType<PersonProfilePopulator>();
    }

    void Start()
    {
        Debug.Log("Starting a FollowSlab...");
        if(personProfilePopulator.FollowingThisUser)
        {
            Debug.Log("Set to Following");
            SetFollowState(FollowState.Following);
        }
        else
        {
            Debug.Log("Set to not following");
            SetFollowState(FollowState.NotFollowing);
        }
    }

    public void TouchOnButton()
    {
        Debug.Log("TouchOnButton...");
        if(TouchAction!=null)
        {
            Debug.Log("... not null");
            TouchAction();
        }
    }

    public void SetFollowState(FollowState state)
    {
        Debug.Log("<color=blue>SetFollowState " + state + " called</color>");
        followState = state;
        switch(state)
        {
            case FollowState.Following:
                BackgroundImage.color = FollowingColor;
                ButtonImage.color = NotFollowingColor;
                TextComponent.text = FollowingText;
                ContentFader.fadeToOpaque();
                ButtonText.text = UnfollowText;
                TouchAction = Unfollow;
                break;
            case FollowState.NotFollowing:
                BackgroundImage.color = NotFollowingColor;
                ButtonImage.color = FollowingColor;
                //TextComponent.text = NotFollowingText;
                ContentFader.fadeToTransparent();
                ButtonText.text = FollowText;
                TouchAction = Follow;
                break;
        }
    }

    public void Unfollow()
    {
        // call API to unfollow
        Debug.Log("Called unfollow");
        API.GetSingleton().UnfollowUser(PlayerPrefs.GetString("UserId"), personProfilePopulator.ProfileUserId, (err, result) => {
            Debug.Log("This was the response: " + result);
            if(err==null)
            {
                SetFollowState(FollowState.NotFollowing);
            }
        });
    }

    public void Follow()
    {
        // call API to follow, callback to change color
        Debug.Log("Called follow");
        API.GetSingleton().FollowUser(PlayerPrefs.GetString("UserId"), personProfilePopulator.ProfileUserId, (err, result) => {
            Debug.Log("This was the response: " + result);
            if (err==null)
            {
                SetFollowState(FollowState.Following);
            }
        });
    }
}
