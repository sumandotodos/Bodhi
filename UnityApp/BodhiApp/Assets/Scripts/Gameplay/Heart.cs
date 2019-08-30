using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Heart : Planet
{
    public TypeOfContent favType;

    public override void MakePlanetControllerProceedToNextScreen(PlanetGameController controller)
    {
        controller.PlanetsToFavoritesSequence(favType);
    }

    public static string FavTypeToString(TypeOfContent favType)
    {
        switch (favType)
        {
            case TypeOfContent.Idea:
                return "Idea";
            case TypeOfContent.Question:
                return "Question";
        }
        return "";
    }

    public static TypeOfContent FavTypeFromString(string favString)
    {
        switch(favString)
        {
            case "Idea":
                return TypeOfContent.Idea;
            case "Question":
                return TypeOfContent.Question;
        }
        return TypeOfContent.Idea;
    }
}
