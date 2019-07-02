using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//

public class UIScaleFader : FGProgram, UITwoPointEffect {

	public float prevValue;
	public float value;

	public float scaleValue;
	public float scaleTarget;

	public TweenableSoftFloat scale;
	bool started = false;

	public float maxScale = 1.0f;
	public float minScale = 0.0f;
	public float speed = 1.0f;

	public float linSpaceTarget;
	public float linSpaceOrigin;
	public float linSpaceValue;

	public EaseType easeType;

	public bool startScaledOut = true;

	int state = 0; 	// 0: idle;
					// 1: fading

	private void updateScale() {
		this.transform.localScale = new Vector3 (scaleValue, scaleValue, scaleValue);
	}

	public void reset() {
		if (startScaledOut) {
			scale.setValueImmediate (minScale);
			scaleValue = scaleTarget = minScale;
            if (scaleValue < 0.01f) disableRender();
		} else {
			scale.setValueImmediate (maxScale);
			scaleValue = scaleTarget = maxScale;
		}
		state = 0;
		updateScale ();
	}

    public void Start() {
		if (started == true)
			return;
		started = true;
		scale = new TweenableSoftFloat ();
		switch (easeType) {
		case EaseType.linear:
			break;
		case EaseType.boingOut:
			scale.setTransformation (TweenTransforms.boingOut);
			break;
            case EaseType.boingOutMore:
                scale.setTransformation(TweenTransforms.boingOutMore);
                break;
            case EaseType.cubicOut:
			scale.setTransformation (TweenTransforms.cubicOut);
			break;
		case EaseType.tanh:
			scale.setTransformation (TweenTransforms.tanh);
			break;
		}


		scale.setSpeed (speed);

		reset ();

	}

    public void SetSpeed(float sp)
    {
        scale.setSpeed(sp);
    }

    public void setEaseType(EaseType t) {
		scale.setEaseType (t);
	}

	public void scaleIn() {
		state = 1;
        enableRender();
		scale.setValue (maxScale);
	}

	public void scaleOut() {
		state = 1;
		scale.setValue (minScale);
	}

	public void scaleOutImmediately() {
		state = 1;
		scale.setValueImmediate (minScale);
	}

	public void scaleInImmediately() {
		state = 1;
        enableRender();
		scale.setValueImmediate (maxScale);
	}

	public void scaleInTask(FGProgram waiter) {
		registerWaiter (waiter);
		scaleIn ();
	}

	public void scaleOutTask(FGProgram waiter) {
		registerWaiter (waiter);
		scaleOut ();
	}

    void disableRender()
    {
        this.gameObject.SetActive(false);
        /*Image imComponent = this.GetComponent<Image>();
        RawImage rawImComponent = this.GetComponent<RawImage>();
        Text textComponent = this.GetComponent<Text>();
        if (imComponent != null) imComponent.enabled = false;
        if (rawImComponent != null) rawImComponent.enabled = false;
        if (textComponent != null) textComponent.enabled = false;
        */
    }

    void enableRender()
    {
        this.gameObject.SetActive(true);
        /*
        Image imComponent = this.GetComponent<Image>();
        RawImage rawImComponent = this.GetComponent<RawImage>();
        Text textComponent = this.GetComponent<Text>();
        if (imComponent != null) imComponent.enabled = true;
        if (rawImComponent != null) rawImComponent.enabled = true;
        if (textComponent != null) textComponent.enabled = true;
        */       
    }

    void Update() {

		linSpaceValue = scale.linSpaceValue;
		linSpaceOrigin = scale.linSpaceOrigin;
		linSpaceTarget = scale.linSpaceTarget;

		scaleValue = scale.getValue ();
		//scaleTarget = scale.linSpaceTarget;

		//update (); // update program
		prevValue = scale.prevValue;
		value = scale.getValue ();//.value;
		if(state == 1) {
			if (!scale.update ()) {
                if(scaleValue <= 0.01f)
                {
                    disableRender();
                }
                notifyFinish ();
				state = 0;
			} 
			scaleValue = scale.getValue ();
			updateScale ();

		}

	}

//	public void notifyFinishExternal() {
//		for (int i = 0; i < waiters.Count; ++i) {
//			waiters [i].waitFinish ();
//		}
//		waiters = new List<FGProgram>();
//	}

	public float getNormalizedParameter() {
		return (scale.getValue () - minScale) / (maxScale - minScale);
	}
		
}
