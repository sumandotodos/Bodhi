using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Favoritizer : MonoBehaviour
{
    bool isFavorite = false;

    public UIAnimatedImage animatedImage;

    public void ToggleFavorization()
    {
        isFavorite = !isFavorite;
        if (isFavorite)
        {
            animatedImage.PlaySegment("Favoritize");
            string id = ContentsController.GetSingleton().RetrieveId();
            Debug.Log("<color=red>"+id+" became favorite</color>");
        }
        else
        {
            animatedImage.PlaySegment("Disfavoritize");
        }

    }

    public void SetIsFavorite()
    {
            animatedImage.PlaySegment("AlreadyFavorite");
    }
}
