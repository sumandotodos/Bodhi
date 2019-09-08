using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonsAvatarController : MonoBehaviour
{
    public Texture2D DefaultUserTexture;
    public Transform CameraYPivot;
    public SpriteRenderer UserAvatarPicture;
    public UIFader UserAvatarPictureFader;
    public PlanetsUIController uiController;
    public AvatarTaker avatarTaker;
    public List<User> listOfUsers = null;
    public float PlanetaryOrbitalSpeed = 2.0f;

    public Texture2D LatestSuccessfulTexture;

    public Texture2D[] textures;

    float PlanetaryRotation = 0.0f;

    Texture2D MaskedDefaultUserTexture;

    float sectorWidth;
    int nSectors;
    int prevSection = 0;

    void Start()
    {
        nSectors = listOfUsers.Count;
        sectorWidth = (360.0f) / (float)nSectors;
        prevSection = (int)(FGUtils.NormalizeAngle(CameraYPivot.rotation.eulerAngles.y) / (float)nSectors);
        MaskedDefaultUserTexture = avatarTaker.ApplyMaskTexture(DefaultUserTexture);
    }

    public void SetListOfUsers(List<User> newList)
    {
        listOfUsers = newList;
        nSectors = listOfUsers.Count;
        sectorWidth = (360.0f) / (float)nSectors;
        SetUpNumberOfTextures(nSectors);
        for(int i = 0; i < nSectors; ++i)
        {
            DownloadTextureForUser(listOfUsers[i]._id, i, (index, tex) =>
            {
                Debug.Log("<color=red>DownloadTextureForUser counterpart</color>");
                Debug.Log("<color=red>i: "+index+", Tex size: " + tex.width + ", " + tex.height + "</color>");
                textures[index] = tex;

            });
        }
    }


    private void SetUpNumberOfTextures(int n)
    {
        textures = new Texture2D[n];
        for(int i = 0; i < n; ++i)
        {
            textures[i] = MaskedDefaultUserTexture;
        }
    }

    public void DownloadTextureForUser(string id, int index, System.Action<int, Texture2D> callback)
    {
        StartCoroutine(DownloadTextureCoroutine(id, index, callback));
    }

    IEnumerator DownloadTextureCoroutine(string id, int index, System.Action<int, Texture2D> callback)
    {
        yield return API.GetSingleton().GetAvatar(id, (err, success, tex) => {
            if (success)
            {
                Debug.Log("<color=green>Tex size: " + tex.width + ", " + tex.height + "</color>");
                LatestSuccessfulTexture = tex;
                callback(index, avatarTaker.ApplyMaskTexture(tex));
            }
            else
            {
                callback(index, MaskedDefaultUserTexture);
            }
        });
    }

    void Update()
    {

        PlanetaryRotation += Time.deltaTime * PlanetaryOrbitalSpeed;

        if (listOfUsers == null)
        {
            return;
        }

        if(listOfUsers.Count == 0)
        {
            return;
        }

        int section = (int)(FGUtils.NormalizeAngle(CameraYPivot.rotation.eulerAngles.y - PlanetaryRotation + sectorWidth / 2.0f) / sectorWidth);
        if (section < 0) section = -section;
        section = section % listOfUsers.Count;
        if (section != prevSection)
        {
            uiController.changeAvatar(textures[section]);
            prevSection = section;
        }
    }

 

}
