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

    // Start is called before the first frame update
    void Start()
    {
        fader.Start();
        fader.fadeToTransparent();
        UpdateAvatarRI(avatarTaker.ApplyMaskTexture(avatarTaker.LoadAvatar()));
        //avatarTaker.GetAvatarFromGallery((tex) => { UpdateAvatarRI(tex); });
    }

    public void TouchOnPhoto()
    {
        avatarTaker.GetAvatarFromCamera((tex) => {
            UpdateAvatarRI(tex);
            avatarTaker.SaveAvatar(tex);
        });
    }

    public void TouchOnGallery()
    {
        avatarTaker.GetAvatarFromGallery((tex) => {
            UpdateAvatarRI(tex);
            avatarTaker.SaveAvatar(tex);
            byte[] jpegData = avatarTaker.Rescale(avatarTaker.CropTextureToSquare(tex), 256, 256).EncodeToJPG();
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



    public void TouchOnGoBackButton()
    {
        StartCoroutine(GoBackCoroutine());
    }

    IEnumerator GoBackCoroutine()
    {
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync("Planets");
    }

}
