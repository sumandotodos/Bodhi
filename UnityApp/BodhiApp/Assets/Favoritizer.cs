using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Favoritizer : MonoBehaviour
{
    bool isFavorite = false;

    public string ContentId;

    public UIAnimatedImage animatedImage;

    public void ToggleFavorization()
    {
        isFavorite = !isFavorite;
        if (isFavorite)
        {
            animatedImage.PlaySegment("Favoritize");
            string id = ContentsController.GetSingleton().RetrieveId();
            Debug.Log("<color=red>"+id+" became favorite</color>");
            API.GetSingleton().CreateFavorite(PlayerPrefs.GetString("UserId"), ContentId);
        }
        else
        {
            animatedImage.PlaySegment("Disfavoritize");
            API.GetSingleton().DestroyFavorite(PlayerPrefs.GetString("UserId"), ContentId);
        }

    }

    public void ResetIcon()
    {
        animatedImage.reset();
        isFavorite = false;
    }

    public void SetIsFavorite()
    {   
        animatedImage.PlaySegment("AlreadyFavorite");
        isFavorite = true;
    }
}
