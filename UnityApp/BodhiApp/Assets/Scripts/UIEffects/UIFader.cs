using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIFader : FGProgram, UITwoPointEffect {

	public float prevValue;
	public float value;

	public float opacityValue;
	public float opacityTarget;

	public SoftFloat opacity;
	bool started = false;
	Image imageComponent;
    RawImage rawImageComponent;
	public float maxOpacity = 1.0f;
	public float minOpacity = 0.0f;
	public float speed = 1.0f;
	public Color opaqueColor = Color.black;
	public bool startOpaque = true;

    Button button_N;

	int state = 0; 	// 0: idle;
					// 1: fading

	private void updateColor() {
		Color newColor = opaqueColor;
		newColor.a = opacity.GetValue ();
        if(imageComponent != null)
		    imageComponent.color = newColor;
        if (rawImageComponent != null)
            rawImageComponent.color = newColor;
	}

	public void Start() {
		if (started == true)
			return;
		started = true;
		opacity = new LinearSoftFloat ();
		//opacity.setTransformation (TweenTransforms.tanh);
		imageComponent = this.GetComponent<Image> ();
        rawImageComponent = this.GetComponent<RawImage>();
        button_N = GetComponent<Button>();

        if (startOpaque) {
			setOpacity (maxOpacity);
		} else {
			setOpacity (minOpacity);
		}

		opacity.SetSpeed (speed);
		state = 0;
		updateColor ();
	}

    public void SetSpeed(float _sp)
    {
        opacity.SetSpeed(_sp);
    }

    public void setOpacity(float op) {
        if (op > 0.0f)
        {
            if (imageComponent != null)
                imageComponent.enabled = true;
            if (rawImageComponent != null)
                rawImageComponent.enabled = true;
            if (button_N != null)
                button_N.interactable = true;
        }
        else
        {
            if(imageComponent != null)
            imageComponent.enabled = false;
            if (rawImageComponent != null)
                rawImageComponent.enabled = false;
            if (button_N != null)
                button_N.interactable = false;
        }
		opacity.SetValueImmediate (op);
		opacityValue = op;
		updateColor ();
	}

	public void fadeToOpaque() {
		state = 1;
        if (imageComponent != null)
            imageComponent.enabled = true;
        if (rawImageComponent != null)
            rawImageComponent.enabled = true;
        if (button_N != null)
            button_N.interactable = true;
        opacity.SetValue (maxOpacity);
	}

	public void fadeToTransparent() {
		state = 1;
        if(imageComponent != null)
		imageComponent.enabled = true;
        if (rawImageComponent != null)
            rawImageComponent.enabled = true;
		opacity.SetValue (minOpacity);
	}

    public void fadeToOpaqueImmediately()
    {
        state = 0;
        if(imageComponent != null)
        imageComponent.enabled = (maxOpacity > 0.0f);
        if (rawImageComponent != null)
            rawImageComponent.enabled = (maxOpacity > 0.0f);
        opacity.SetValueImmediate(maxOpacity);
        updateColor();
    }

    public void fadeToTransparentImmediately()
    {
        state = 0;
        if(imageComponent != null)
        imageComponent.enabled = (minOpacity > 0.0f);
        if (rawImageComponent != null)
            rawImageComponent.enabled = (minOpacity > 0.0f);
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

		opacityValue = opacity.GetValue ();//value;

		update (); // update program
		
		value = opacity.GetValue ();
		if(state == 1) {
            if (!opacity.Update (Time.deltaTime)) {
				if (opacity.GetValue () <= 0.0f) {
                    if(imageComponent!=null)
					    imageComponent.enabled = false;
                    if (rawImageComponent != null)
                        rawImageComponent.enabled = false;
                    if (button_N != null)
                        button_N.interactable = false;
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


    public float getNormalizedParameter()
    {
        return (opacity.GetValue() - minOpacity) / (maxOpacity - minOpacity);
    }

}
