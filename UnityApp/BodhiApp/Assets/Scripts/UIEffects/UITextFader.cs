using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UITextFader : FGProgram {

	public float prevValue;
	public float value;

	public float opacityValue;
	public float opacityTarget;

	public SoftFloat opacity;
	bool started = false;
	Text textComponent;
	public float maxOpacity = 1.0f;
	public float minOpacity = 0.0f;
	public float speed = 1.0f;
	public Color opaqueColor = Color.black;
	public bool startOpaque = true;

	int state = 0; 	// 0: idle;
					// 1: fading

	private void updateColor() {
		Color newColor = opaqueColor;
		newColor.a = opacity.GetValue ();
		textComponent.color = newColor;
	}

	public void Start() {
		if (started == true)
			return;
		started = true;
		opacity = new LinearSoftFloat ();
		//opacity.setTransformation (TweenTransforms.tanh);
		textComponent = this.GetComponent<Text> ();
		if (startOpaque) {
			opacity.SetValueImmediate (maxOpacity);
		} else {
			opacity.SetValueImmediate (minOpacity);
		}
		opacity.SetSpeed (speed);
		state = 0;
		updateColor ();
	}

	public void setOpacity(float op) {
		if (op > 0.0f) {
			textComponent.enabled = true;
		} else
			textComponent.enabled = false;
		opacity.SetValueImmediate (op);
		opacityValue = op;
		updateColor ();
	}

    public void setSpeed(float s)
    {
        opacity.SetSpeed(s);
    }

    public void fadeToOpaque() {
		state = 1;
		textComponent.enabled = true;
		opacity.SetValue (maxOpacity);
	}

	public void fadeToTransparent() {
		state = 1;
		textComponent.enabled = true;
		opacity.SetValue (minOpacity);
	}

    public void fadeToOpaqueImmediately()
    {
        state = 0;
        textComponent.enabled = (maxOpacity > 0.0f);
        opacity.SetValueImmediate(maxOpacity);
        updateColor();
    }

    public void fadeToTransparentImmediately()
    {
        state = 0;
        textComponent.enabled = (minOpacity > 0.0f);
        opacity.SetValueImmediate(minOpacity);
        updateColor();
    }

    public void fadeToOpaqueTask(FGProgram waiter) {
		registerWaiter (waiter);
		fadeToOpaque ();
	}

	public void fadeToTransparentTask(FGProgram waiter) {
		registerWaiter (waiter);
		fadeToTransparent ();
	}

	void Update() {

		opacityValue = opacity.GetValue ();//.value;
		

		update (); // update program
		
		value = opacity.GetValue ();//.value;
		if(state == 1) {
            if (!opacity.Update (Time.deltaTime)) {
				if (opacity.GetValue () <= 0.0f) {
					textComponent.enabled = false;
				}
				notifyFinishExternal ();
				state = 0;
			} 
			updateColor ();

		}

	}

	public void notifyFinishExternal() {
		for (int i = 0; i < waiters.Count; ++i) {
			waiters [i].waitFinish ();
		}
		waiters = new List<FGProgram>();
	}
		
}
