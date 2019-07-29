using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    public Transform ScrollTransform;
    public ListController listController;

    float TouchYCoord;

    float YScroll;

    float MaxYScroll;

    float Factor = 3.8f;

    public float MaxTrespass = 50.0f;

    System.Action TouchUpdate;

    private void Awake()
    {
        TouchUpdate = NotTouchingUpdate;
        YScroll = ScrollTransform.localPosition.y;
        Factor = (1920.0f / ((float)Screen.height));
    }

    public void BeginTouch()
    {
        MaxYScroll = listController.GetTotalHeight() / Factor;
        TouchYCoord = Input.mousePosition.y;
        TouchUpdate = TouchingUpdate;
    }

    public void EndTouch()
    {
        TouchUpdate = NotTouchingUpdate;
        YScroll += Factor * (Input.mousePosition.y - TouchYCoord);
        YScroll = Mathf.Clamp(YScroll, 0.0f, MaxYScroll);
        ScrollTransform.localPosition = new Vector2(0, YScroll);
    }

    private void Update()
    {
        TouchUpdate();
    }

    public void TouchingUpdate()
    {
        float CurrentY = Input.mousePosition.y;
        float Diff = Factor * (CurrentY - TouchYCoord);
        float CandidateY = YScroll + Diff;
        if(CandidateY < 0.0f) 
        {
            CandidateY = MaxTrespass * (Mathf.Exp(CandidateY/MaxTrespass)-1.0f);
        }
        if(CandidateY > MaxYScroll)
        {
            CandidateY = MaxYScroll - MaxTrespass * (Mathf.Exp((MaxYScroll - CandidateY) / MaxTrespass) - 1.0f);
        }
        ScrollTransform.localPosition = new Vector2(0, CandidateY);

    }

    public void NotTouchingUpdate()
    {
        // nothing here for now...
    }

}
