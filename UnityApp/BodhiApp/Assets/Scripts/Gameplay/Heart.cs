using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Heart : Planet
{
    public override void MakePlanetControllerProceedToNextScreen(PlanetGameController controller)
    {
        controller.PlanetsToFavoritesSequence();
    }
}
