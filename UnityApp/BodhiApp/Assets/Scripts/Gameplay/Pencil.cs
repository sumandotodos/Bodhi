using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pencil : Planet
{
    public override void MakePlanetControllerProceedToNextScreen(PlanetGameController controller)
    {
        controller.PlanetsToComposeSequence();
    }
}
