using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AvatarController : MonoBehaviour
{
    public UIFader fader;

    public RawImage avatarRI;

    public AvatarTaker avatarTaker;
    public Texture2D DefaultUserAvatar;
    public ProfileController profileController;

    public UIMoverTwoPoints scrollMover;

    public UIGeneralFader forwardArrowFader;

    public float HorizontalDisplacement = 1080f;

    System.Action ForwardArrowAction;
    System.Action BackwardArrowAction;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Debug.Log("<color=orange>Started avatar controller</color>");
        ForwardArrowAction = Screen1TouchForward;
        BackwardArrowAction = Screen1TouchBackward;
        fader.Start();
        Debug.Log("<color=orange>2</color>");
        forwardArrowFader.GetComponentInChildren<UIOpacityWiggle>().isActive = true;
        Debug.Log("<color=orange>3</color>");
        UpdateAvatarRI(avatarTaker.ApplyMaskTexture(avatarTaker.LoadAvatar()));
        Debug.Log("<color=orange>4</color>");
        //avatarTaker.GetAvatarFromGallery((tex) => { UpdateAvatarRI(tex); });
        yield return API.GetSingleton().GetProfile(PlayerPrefs.GetString("UserId"),
            (err, profile) =>
            {
                Debug.Log("<color=orange>About to populate</color>");
                profileController.Populate(profile);
            });
        fader.fadeToTransparent();
    }

    public void TouchOnPhoto()
    {
        avatarTaker.GetAvatarFromCamera((tex) => {
            UpdateAvatarRI(tex);
            avatarTaker.SaveAvatar(tex);
            byte[] jpegData = avatarTaker.Rescale(avatarTaker.CropTextureToSquare(tex), 256, 256).EncodeToJPG(50);
            Debug.Log("<color=purple>Image: " + jpegData.Length + " bytes</color>");
            API.GetSingleton().PutAvatar(PlayerPrefs.GetString("UserId"), jpegData, (err, text) =>
            {
                Debug.Log(err + ", " + text);
            });
        });
    }

    public void TouchOnGallery()
    {
        avatarTaker.GetAvatarFromGallery((tex) => {
            UpdateAvatarRI(tex);
            avatarTaker.SaveAvatar(tex);
            byte[] jpegData = avatarTaker.Rescale(avatarTaker.CropTextureToSquare(tex), 256, 256).EncodeToJPG(50);
            Debug.Log("<color=purple>Image: " + jpegData.Length + " bytes</color>");
            API.GetSingleton().PutAvatar(PlayerPrefs.GetString("UserId"), jpegData, (err, text) =>
            {
                Debug.Log(err + ", " + text);
            });
        });
    }

    public void UpdateAvatarRI(Texture2D tex)
    {
        avatarRI.texture = avatarTaker.ApplyMaskTexture(tex);
    }

    public void Screen1TouchForward()
    {
        scrollMover.PointA = new Vector2(0.0f, 0.0f);
        scrollMover.PointB = new Vector2(-HorizontalDisplacement, 0.0f);
        scrollMover.Go();
        ForwardArrowAction = Screen2TouchForward;
        BackwardArrowAction = Screen2TouchBackward;
        forwardArrowFader.GetComponentInChildren<UIOpacityWiggle>().isActive = false;
        forwardArrowFader.fadeToTransparent();
    }

    public void Screen1TouchBackward()
    {
        profileController.UploadProfileToServer();
        StartCoroutine(GoBackCoroutine());
    }

    public void Screen2TouchForward()
    {
        // nothing here
    }

    public void Screen2TouchBackward()
    {
        scrollMover.PointB = new Vector2(0.0f, 0.0f);
        scrollMover.PointA = new Vector2(-HorizontalDisplacement, 0.0f);
        scrollMover.Go();
        ForwardArrowAction = Screen1TouchForward;
        BackwardArrowAction = Screen1TouchBackward;
        forwardArrowFader.GetComponentInChildren<UIOpacityWiggle>().isActive = true;
        forwardArrowFader.fadeToOpaque();
    }

    public void TouchForwardButton()
    {
        ForwardArrowAction();
    }

    public void TouchOnGoBackButton()
    {
        //StartCoroutine(GoBackCoroutine());
        BackwardArrowAction();
    }

    IEnumerator GoBackCoroutine()
    {
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync("Planets");
    }

}
