using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pencil : Planet
{
    public TypeOfContent favType;

    public override void MakePlanetControllerProceedToNextScreen(PlanetGameController controller)
    {
        if (favType == TypeOfContent.Idea || favType == TypeOfContent.Question)
        {
            controller.PlanetsToSceneSequence("Contributions", favType);
        }
        else
        {
            controller.PlanetsToSceneSequence("Compose", favType);
        }
    }
}
