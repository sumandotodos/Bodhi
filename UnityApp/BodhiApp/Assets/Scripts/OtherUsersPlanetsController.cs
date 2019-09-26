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

    public void SetListOfUsers(List<User> newList)
    {
        listOfUsers = newList;
        nSectors = listOfUsers.Count;
        sectorWidth = (360.0f) / (float)nSectors;
        SetUpNumberOfTextures(nSectors);
        for (int i = 0; i < nSectors; ++i)
        {
            DownloadTextureForUser(listOfUsers[i]._id, i, (index, tex, origTex) =>
            {
                //if(tex != MaskedDefaultUserTexture)
                //{
                  //  PlanetMaterials[index].mainTexture = avatarTaker.Sphericalize(origTex);
                //}
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

        int section = (int)(FGUtils.NormalizeAngle((listOfUsers.Count-1) * sectorWidth + CameraYPivot.rotation.eulerAngles.y - PlanetaryRotation + sectorWidth / 2.0f) / sectorWidth);
        if (section < 0) section = -section;
        section = section % listOfUsers.Count;
        if (section != prevSection)
        {
            uiController.changeAvatar(textures[section]);
            uiController.changeQuestion(listOfUsers[section].favquestion);
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

    public void TouchOnAnswerQuestion()
    {


    }



}
