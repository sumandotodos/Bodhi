using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController gameController = null;
    public OrbitalCamera orbitalCamera;

    public TweenableSoftFloat CameraZ;

    public UIHighlight[] panelButtons;

    public GameObject ScorePrefab;

    public string SpringOutAnimationName = "SpringUpOK";
    public string SpringInAnimationName = "SpringDown";
    public string HiddenAnimationName = "Hidden";

    private void Awake()
    {
        if(gameController == null)
        {
            gameController = this;
        }
    }

    public Animation UIanimation;
    // Start is called before the first frame update
    void Start()
    {
        ContentsController.GetSingleton().ChooseTopic();
        UIanimation.Play(HiddenAnimationName);
        CameraZ = new TweenableSoftFloat();
        CameraZ.setValueImmediate(10.0f);
        CameraZ.setEaseType(EaseType.cubicOut);
        CameraZ.setSpeed(3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            CameraZ.setValue(CameraZ.getValue() + 10.0f);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CameraZ.setValue(CameraZ.getValue() - 10.0f);
        }
        CameraZ.update();
        orbitalCamera.SetZDistance(CameraZ.getValue());
    }

    public static GameController GetSingleton()
    {
        return gameController;
    }

    public void ShowPanel()
    {

    }

    public void OnBichoFound()
    {

    }

    public void DismissPanel()
    {
        UIanimation.Play(SpringInAnimationName);
        ContentsController.GetSingleton().isShowingText = false;
        Raycaster.GetSingleton().SetActive(true);
    }

    public void BichoFound()
    {
        EnableAllButtons();
        ContentsController.GetSingleton().PrepareNextText();
        UIanimation.Play(SpringOutAnimationName);
    }

    public void Like()
    {
        Debug.Log("<color=green>Like!!!</color>");
        DisableAllButtons();
        DismissPanel();
    }

    public void DontLike()
    {
        DisableAllButtons();
        DismissPanel();
    }

    public void Soso()
    {
        DisableAllButtons();
        DismissPanel();
    }

    public void DisableAllButtons()
    {
        foreach(UIHighlight but in panelButtons)
        {
            but.setEnable(false);
        }
    }

    public void EnableAllButtons()
    {
        foreach (UIHighlight but in panelButtons)
        {
            but.setEnable(true);
        }
    }

    public void TouchScore(int score, Vector3 atLocalCoords)
    {
        if (score > 5) 
        {
            GameObject newGO = (GameObject)Instantiate(ScorePrefab);
            newGO.GetComponent<FleetingScore>().Init(score);
            newGO.transform.position = atLocalCoords;
        }
    }

}
