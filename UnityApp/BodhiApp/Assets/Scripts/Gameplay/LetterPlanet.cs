using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LetterPlanet : Planet
{
    public override void MakePlanetControllerProceedToNextScreen(PlanetGameController controller)
    {
        controller.PlanetsToSceneSequence("Messages", TypeOfContent.Message);
    }
}
