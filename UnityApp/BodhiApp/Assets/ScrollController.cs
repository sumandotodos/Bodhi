using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    public Transform ScrollTransform;
    public ListController listController;

    float TouchYCoord;

    public float YScroll;

    float MaxYScroll;

    float Factor = 3.8f;

    public float Mass = 1.0f;
    public float Friction = 0.5f;

    public float Speed = 0.0f;

    public float SpeedFactor = 1.0f;

    public float TrespassEgressSpeed = 12.0f;

    public float LastFrameY;
    public float NextToLastFrameY;
    public float ThisFrameY;

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
        MaxYScroll = Mathf.Max(listController.GetTotalHeight() - 1920.0f + 300.0f, 0.0f);
        TouchYCoord = Input.mousePosition.y;
        TouchUpdate = TouchingUpdate;
        Speed = 0.0f;
    }

    public void EndTouch()
    {
        TouchUpdate = NotTouchingUpdate;
        float CurrentY = Input.mousePosition.y;
        float Diff = Factor * (CurrentY - TouchYCoord);
        float EndFrameY = YScroll + Diff;
        ThisFrameY = EndFrameY;
        Speed = SpeedFactor * (EndFrameY - LastFrameY) / Time.deltaTime;
        YScroll += Factor * (Input.mousePosition.y - TouchYCoord);
        //YScroll = Mathf.Clamp(YScroll, 0.0f, MaxYScroll);
        ScrollTransform.localPosition = new Vector2(0, YScroll);
       
    }

    private void Update()
    {
        TouchUpdate();
        ScrollUpdate();
    }

    private void ScrollUpdate()
    {
        YScroll += Time.deltaTime * Speed;
        Speed = AbsMinusWithZeroClamp(Speed, Friction * Time.deltaTime);
        if(YScroll < 0.0f)
        {
            Speed = -YScroll * TrespassEgressSpeed;
        }
        if(YScroll > MaxYScroll)
        {
            Speed = -(YScroll - MaxYScroll) * TrespassEgressSpeed;
        }
    }

    private float SignOf(float value)
    {
        return value > 0.0f ? 1.0f : -1.0f;
    }

    private float AbsMinusWithZeroClamp(float a, float b)
    {
        if (Mathf.Abs(a) < 0.01f) return 0.0f;
        float SignBefore = SignOf(a);
        a = a + b * (-1.0f * SignBefore);
        float SignAfter = SignOf(a);
        if(SignAfter * SignBefore < 0.0f)
        {
            a = 0.0f;
        }
        return a;
    }

    private float SoftClamp(float y)
    {
        if (y < 0.0f)
        {
            y = MaxTrespass * (Mathf.Exp(y / MaxTrespass) - 1.0f);
        }
        if (y > MaxYScroll)
        {
            y = MaxYScroll - MaxTrespass * (Mathf.Exp((MaxYScroll - y) / MaxTrespass) - 1.0f);
        }
        return y;
    }

    public void TouchingUpdate()
    {
        float CurrentY = Input.mousePosition.y;
        float Diff = Factor * (CurrentY - TouchYCoord);
        float CandidateY = YScroll + Diff;
        ScrollTransform.localPosition = new Vector2(0, SoftClamp(CandidateY));
        LastFrameY = NextToLastFrameY;
        NextToLastFrameY = CandidateY;
    }

    public void NotTouchingUpdate()
    {
        ScrollTransform.localPosition = new Vector2(0, SoftClamp(YScroll));
    }

}
