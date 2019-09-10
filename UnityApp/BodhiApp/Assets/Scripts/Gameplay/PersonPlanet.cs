using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersonPlanet : Planet
{
    public string handle;
    public string id;

    public override void MakePlanetControllerProceedToNextScreen(PlanetGameController controller)
    {
        PlayerPrefs.SetString("OtherUserHandle", handle);
        PlayerPrefs.SetString("OtherUserId", id);
        controller.PlanetsToSceneSequence("PersonProfile", TypeOfContent.Any);
    }
}
