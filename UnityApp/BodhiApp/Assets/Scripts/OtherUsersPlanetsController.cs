using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.S3;

public class OtherUsersPlanetsController : MonoBehaviour
{
    public Texture2D DefaultUserTexture;
    public Transform CameraYPivot;
    public SpriteRenderer UserAvatarPicture;
    public UIFader UserAvatarPictureFader;
    public PlanetsUIController uiController;
    public AvatarTaker avatarTaker;
    public List<User> listOfUsers = null;
    public float PlanetaryOrbitalSpeed = 2.0f;
    public Material[] PlanetMaterials;
   

    float sectorWidth;
    int nSectors;
    int prevSection = 0;

    float PlanetaryRotation = 0.0f;
    int MaxUsersPerPage = 0;

    Texture2D[] textures;

    ScaleFader[] planetScaleFaders;

    Texture2D MaskedDefaultUserTexture;

    void Start()
    {
        nSectors = listOfUsers.Count;
        sectorWidth = (360.0f) / (float)nSectors;
        prevSection = (int)(FGUtils.NormalizeAngle(CameraYPivot.rotation.eulerAngles.y) / (float)nSectors);
        MaskedDefaultUserTexture = avatarTaker.ApplyMaskTexture(DefaultUserTexture);
    }


    public void SetPlanetsScaleFaders(List<ScaleFader> Faders)
    {
        planetScaleFaders = Faders.ToArray();
    }

    public void SetListOfUsers(int UsersPerPage, List<User> newList)
    {
        MaxUsersPerPage = UsersPerPage;
        listOfUsers = newList;
        nSectors = listOfUsers.Count;
        sectorWidth = (360.0f) / (float)MaxUsersPerPage;
        SetUpNumberOfTextures(nSectors);
        for (int i = 0; i < nSectors; ++i)
        {
            DownloadTextureForUser(listOfUsers[i]._id, i, (index, tex, origTex) =>
            {
                textures[index] = tex;
            });
        }
    }


    private void SetUpNumberOfTextures(int n)
    {
        textures = new Texture2D[n];
        for (int i = 0; i < n; ++i)
        {
            textures[i] = MaskedDefaultUserTexture;
        }
    }


    public void DownloadTextureForUser(string id, int index, System.Action<int, Texture2D, Texture2D> callback)
    {
        StartCoroutine(DownloadTextureCoroutine(id, index, callback));
    }


    IEnumerator DownloadTextureCoroutine(string id, int index, System.Action<int, Texture2D, Texture2D> callback)
    {
        yield return API.GetSingleton().GetAvatar(id, (err, success, tex) => {
            if (success)
            {
                callback(index, avatarTaker.ApplyMaskTexture(tex), tex);
            }
            else
            {
                callback(index, MaskedDefaultUserTexture, MaskedDefaultUserTexture);
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

        if (listOfUsers.Count == 0)
        {
            return;
        }

        int section = (int)(FGUtils.NormalizeAngle((MaxUsersPerPage-1) * sectorWidth + CameraYPivot.rotation.eulerAngles.y - PlanetaryRotation + sectorWidth / 2.0f) / sectorWidth);
        if (section < 0) section = -section;
        section = section % MaxUsersPerPage;
        if ((section != prevSection))
        {
            if(section < listOfUsers.Count)
            {
                uiController.changeAvatar(textures[section]);
                uiController.changeQuestion(listOfUsers[section].favquestion);
            }
            else
            {
                uiController.changeAvatar(null);
                uiController.changeQuestion(null);
            }
            ScaleHighlightPlanet(section);
            prevSection = section;
        }
    }


    private void ScaleHighlightPlanet(int n)
    {
        for(int i = 0; i < planetScaleFaders.Length; ++i)
        {
            if(i==n)
            {
                planetScaleFaders[i].scaleIn();
            }
            else
            {
                planetScaleFaders[i].scaleOut();
            }
        }
    }

    public string GetUserId()
    {
        return listOfUsers[prevSection]._id;
    }

    public string GetQuestion()
    {
        return listOfUsers[prevSection].favquestion;
    }

    public string GetQuestionId()
    {
        return listOfUsers[prevSection].favquestionid;
    }

    public void TouchOnAnswerQuestion()
    {


    }



}
