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
    public UITextFader TitleFader;
    public BombController bombController;

    public string SpringOutAnimationName = "SpringUpOK";
    public string SpringInAnimationName = "SpringDown";
    public string HiddenAnimationName = "Hidden";

    public int Columns = 6;
    public int Rows = 10;
    public int Bichos = 5;

    string Header;

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
        string Category = PlayerPrefs.GetString("ContentType");
        int nPhrases = ContentsController.GetSingleton().ChooseTopic();
        Bichos = Mathf.Max(6, nPhrases);
    
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

        Header = ContentsController.GetSingleton().GetHeader();
        if (Header != "")
        {
            StartCoroutine(ShowTitleCoroutine());
        }
    }

    IEnumerator ShowTitleCoroutine()
    {
        TitleFader.GetComponent<Text>().text = Header;
        UIScaleFader scaler = TitleFader.GetComponent<UIScaleFader>();
        TitleFader.Start();
        TitleFader.fadeToOpaque();
        scaler.Start();
        float Duration = Mathf.Max(3.0f, 0.10f * Header.Length);
        scaler.SetSpeed(0.2f * (1.0f / (1.2f*Duration)));
        scaler.scaleOutImmediately();
        scaler.scaleIn();
        yield return new WaitForSeconds(Duration);
        TitleFader.fadeToTransparent();
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

    Vector2 BichoScreenCoords;
    public void BichoFound(Vector2 screenCoords)
    {
        EnableAllButtons();
        remainingText.SetText("" + (--Bichos));
        ContentsController.GetSingleton().PrepareNextText();
        BichoScreenCoords = screenCoords;
        StartCoroutine(BichoFoundCoroutine());
    }

    IEnumerator BichoFoundCoroutine()
    {
        mustDismissPanel = true;
        yield return new WaitForSeconds(1.35f);
        FrameScaler.SetSpeed(0.22f);
        FrameScaler.scaleOutImmediately();
        FrameScaler.setEaseType(EaseType.boingOutMore);
        FrameScaler.scaleIn();
        FrameMover.Start();
        FrameMover.PointA = BichoScreenCoords;
        FrameMover.PointB = FGUtils.UICoordinateTransform(new Vector2(0.5f, 0.5f), UICoordinateType.Normalized);
        FrameMover.Go();
        foreach (DelayExec de in delayExec)
        {
            de.StartWork();
        }
       
    }

    public void Like()
    {
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
        if (score >= 5) 
        {
            GameObject newGO = (GameObject)Instantiate(ScorePrefab);
            newGO.GetComponent<FleetingScore>().Init(score);
            newGO.transform.position = atLocalCoords;
        }
        if(score >= 20)
        {
            bombController.AddBomb();
        }
    }

    public void ReportClearedCells(int c)
    { 
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
        Debug.Log("Finish game sequence called with mustDismissPanel: " + mustDismissPanel);
        yield return new WaitUntil(() => (mustDismissPanel == false));
        skyFader.fadeToOpaque();
        yield return new WaitForSeconds(2.0f);
        fader.fadeToOpaque();
        yield return new WaitForSeconds(1.5f);
        yield return SceneManager.LoadSceneAsync("Planets");
    }

}
