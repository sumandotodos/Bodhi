using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Style { bottomLeft, bottomCenter, bottomRight, topLeft, topCenter, topRight };
public enum Orientation { Vertical, Horizontal, Neutral };

public class UITip : MonoBehaviour {

    public bool dismissableByTouch = true;
    public bool justOnce = true;

    public Text theText;
	public UIScaleFader scaler;

	public AudioSource ausource;

	public Orientation orientation;

	public UIFader notoquesFader;

	public Sprite[] styleImage;
	public Vector2[] stylePivot;
	Image theImage;

	float relPos = 0;
	float phase;
	const float phaseSpeed = 10.0f;
	const float amplitude = 3.0f;
	Vector3 originalPos;
	public float initialAmplitudeSpeed = 0.2f;

    public static bool TipMutex;

    public static void CheckTipIn()
    {
        TipMutex = true;
    }

    public static void CheckTipOut()
    {
        TipMutex = false;
    }

    // Use this for initialization
    void Start () {
		phase = 0;
		relPos = 0;
		originalPos = this.transform.position;
		theImage = this.GetComponent<Image> ();
	}

	public void setPosition(Vector2 pos) {
		originalPos = pos;
		this.transform.position = pos;
	}

	bool going = false;

	public void reset() {
		going = false;
		phase = 0;
		relPos = 0;
		grow = true;
		initialAmplitude = 0;
	}

	public void go() {
        if ((justOnce==false) || TipSaveController.MustDoTip(this.name))
        {
            if(justOnce == true)
            {
                ServerMessagesController.SetDelay(6.0f);
            }
            going = true;
            UITip.CheckTipIn();
            scaler.setEaseType(EaseType.boingOutMore);
            notoquesFader.fadeToOpaque();
            scaler.scaleIn();
            ausource.Play();
        }
        else notoquesFader.fadeToTransparent();
	}

	public void setStyle(Style s) {
		int styleIndex = 0;
		switch (s) {
		case Style.bottomLeft:
			styleIndex = 0;
			break;
		case Style.bottomCenter:
			styleIndex = 1;
			break;
		case Style.bottomRight:
			styleIndex = 2;
			break;
		case Style.topLeft:
			styleIndex = 3;
			break;
		case Style.topCenter:
			styleIndex = 4;
			break;
		case Style.topRight:
			styleIndex = 5;
			break;
		}
	
		theImage.sprite = styleImage [styleIndex];
		theImage.GetComponent<RectTransform> ().pivot = stylePivot [styleIndex];
	}

	public float initialAmplitude = 0;
	bool grow = true;

//	public void moveTo(Vector2 pos) {
//		this.transform.position = pos;
//	}

	// Update is called once per frame
	void Update () {
		if (!going)
			return;
		phase += phaseSpeed * Time.deltaTime;
		relPos = amplitude * Mathf.Sin (phase) * initialAmplitude;
		if(grow) initialAmplitude += initialAmplitudeSpeed * Time.deltaTime;
		if (initialAmplitude > 1.0f)
			initialAmplitude = 1.0f;
		if (!grow)
			initialAmplitude -= initialAmplitudeSpeed * 3 * Time.deltaTime;
		if (initialAmplitude < 0.0f)
			initialAmplitude = 0.0f;
		if (orientation == Orientation.Vertical) {
			this.transform.position = new Vector3(originalPos.x, originalPos.y + relPos, originalPos.z);
		}
		else if (orientation == Orientation.Horizontal)
        {
			this.transform.position = new Vector3 (originalPos.x + relPos, originalPos.y, originalPos.z);
		}

		if (dismissableByTouch && scaler.scaleValue >= 0.5f) {
			if (Input.GetMouseButtonDown (0)) {
                dismiss();
			}
		}
	}

    public void dismiss()
    {
        scaler.setEaseType(EaseType.cubicIn);
        scaler.SetSpeed(2.0f);
        grow = false;
        scaler.scaleOut();
        UITip.CheckTipOut();
        if(justOnce==true)
        {
            ServerMessagesController.SetDelay(6.0f);
        }
        notoquesFader.fadeToTransparent();
    }

    public bool hasBeenDispatched() {
		return scaler.scaleValue == 0;
	}

    public void SetMessage(string msg)
    {
        theText.text = msg;
    }


}
