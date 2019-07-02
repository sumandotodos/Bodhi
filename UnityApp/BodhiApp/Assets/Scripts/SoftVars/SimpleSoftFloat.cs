using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSoftFloat : SoftFloat {

	float current;
	float target;
	public float speed = 0.25f;
	public float threshold = 0.01f;

	public override void SetValue(float _target) {
		target = _target;
	}

    public override void SetSpeed(float sp)
    {
        speed = sp;
    }

    public override float GetValue() {
		return current;
	}

	public override void SetValueImmediate(float value) {
		target = current = value;
	}
		
	public void initialize () {
		current = target = 0;
	}

	public override bool Update (float deltatime) {
        if (Mathf.Abs(target - current) > threshold)
        {
            current = Mathf.Lerp(current, target, speed * deltatime);
            return true;
        }
        else return false;
	}
}
