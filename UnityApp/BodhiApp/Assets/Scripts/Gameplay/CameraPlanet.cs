using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraPlanet : Planet
{
    public override void MakePlanetControllerProceedToNextScreen(PlanetGameController controller)
    {
        controller.PlanetsToSequence("Avatar");
    }
}
