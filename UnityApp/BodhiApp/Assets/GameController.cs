using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static GameController gameController = null;
    public UIScaleFader FrameScaler;
    public UIMoverTwoPoints FrameMover;
    public OrbitalCamera orbitalCamera;
    public TweenableSoftFloat CameraZ;
    public UIHighlight[] panelButtons;
    public DelayExec[] delayExec;
    public GameObject ScorePrefab;
    public UIFader fader;
    public UIFader skyFader;
    public CrossfadeText remainingText;

    public string SpringOutAnimationName = "SpringUpOK";
    public string SpringInAnimationName = "SpringDown";
    public string HiddenAnimationName = "Hidden";

    public int Columns = 6;
    public int Rows = 10;
    public int Bichos = 5;

   

    bool mustDismissPanel = false;

    int TotalCells;

    public GridSpawner gridSpawner_A = null;

    private void Awake()
    {
        if(gameController == null)
        {
            gameController = this;
            if (gridSpawner_A == null)
            {
                gridSpawner_A = FindObjectOfType<GridSpawner>();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int nPhrases = ContentsController.GetSingleton().ChooseTopic();
        Bichos = Mathf.Max(6, nPhrases);
        //UIanimation.Play(HiddenAnimationName);
        FrameScaler.setEaseType(EaseType.cubicIn);
        FrameScaler.SetSpeed(3.0f);
        FrameScaler.scaleOutImmediately();
        CameraZ = new TweenableSoftFloat();
        CameraZ.setValueImmediate(10.0f);
        CameraZ.setEaseType(EaseType.cubicOut);
        CameraZ.setSpeed(3.0f);
        gridSpawner_A.BuildGrid(Columns, Rows, Bichos);
        remainingText.Start();
        remainingText.SetText("" + Bichos);
        TotalCells = Columns * Rows;
        fader.Start();
        fader.fadeToTransparent();
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
        FrameScaler.setEaseType(EaseType.cubicIn);
        FrameScaler.SetSpeed(3.0f);
        FrameScaler.scaleOut();
        foreach (DelayExec de in delayExec)
        {
            de.StopWork();
        }
        ContentsController.GetSingleton().isShowingText = false;
        Raycaster.GetSingleton().SetActive(true);
        mustDismissPanel = false;
    }

    public void BichoFound(Vector2 screenCoords)
    {
        EnableAllButtons();
        remainingText.SetText("" + (--Bichos));
        ContentsController.GetSingleton().PrepareNextText();
        FrameScaler.SetSpeed(0.08f);
        FrameScaler.setEaseType(EaseType.boingOutMore);
        FrameScaler.scaleIn();
        FrameMover.Start();
        FrameMover.PointA = screenCoords;
        FrameMover.PointB = FGUtils.UICoordinateTransform(new Vector2(0.5f, 0.5f), UICoordinateType.Normalized);
        FrameMover.Go();
        foreach(DelayExec de in delayExec)
        {
            de.StartWork();
        }
        mustDismissPanel = true;
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

    public void ReportClearedCells(int c)
    {
        //Debug.Log("<color=purple>Faded: " + c + "</color>");
        //if (c == TotalCells)
        if(Bichos == 0)
        {
            FinishGameSequence();
        }
    }

    public void FinishGameSequence()
    {
        StartCoroutine(_FinishGameSequence());
    }

    IEnumerator _FinishGameSequence()
    {
        yield return new WaitUntil(() => (mustDismissPanel == false));
        skyFader.fadeToOpaque();
        yield return new WaitForSeconds(2.0f);
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.5f);
        yield return SceneManager.LoadSceneAsync("Planets");
    }

}
