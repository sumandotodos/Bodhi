using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionController : MonoBehaviour
{
    public Transform CameraYPivot;
    public PlanetsUIController uiController;
    public List<User> listOfUsers = null;
    public float checkYRot;

    float sectorWidth;
    int nSectors;
    int prevSection = 0;

    void Start()
    {
        nSectors = listOfUsers.Count;
        sectorWidth = (360.0f) / (float)nSectors;
        prevSection = (int)(FGUtils.NormalizeAngle(CameraYPivot.rotation.eulerAngles.y) / (float)nSectors);
    }

    public void SetListOfUsers(List<User> newList)
    {
        listOfUsers = newList;
        nSectors = listOfUsers.Count;
        sectorWidth = (360.0f) / (float)nSectors;
    }

    // Update is called once per frame
    void Update()
    {
        if(listOfUsers == null)
        {
            return;
        }

        if(listOfUsers.Count == 0)
        {
            return;
        }

        checkYRot = CameraYPivot.rotation.eulerAngles.y;
        int section = (int)(FGUtils.NormalizeAngle(CameraYPivot.rotation.eulerAngles.y+sectorWidth/2.0f) / sectorWidth);
        if (section < 0) section = -section;
        section = section % listOfUsers.Count;
        if(section != prevSection)
        {
            uiController.changeQuestion(listOfUsers[section].favquestion);
            prevSection = section;
            Debug.Log("<color=blue>New section: " + section + "</color>");
        }
    }
}
