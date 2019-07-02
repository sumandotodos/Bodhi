using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftAngle : SoftFloat {

    float speed;
    float currentAngle;
    float targetAngle;

    public static float normalizeAngle(float _angle) {
        float result = _angle;
        while (result < 0.0f) result += 360.0f;
        while (result > 360.0f) result -= 360.0f;
        return result;
    }

    public override float GetValue()
    {
        return currentAngle;
    }

    public static float angleDiff(float angle1, float angle2, bool crossPole) {
        if (crossPole) return 360.0f - Mathf.Abs(angle2 - angle1);
        else return Mathf.Abs(angle2 - angle1);
    }

    public override void SetValue(float _target)
    {
        targetAngle = _target;
    }

    override public void SetSpeed(float _speed) {
        speed = _speed;
    }

    public override void SetValueImmediate(float value)
    {

    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    override public bool Update(float deltaTime) {
        bool change = false;
        if(currentAngle < targetAngle) {
            if(angleDiff(targetAngle, currentAngle, false) < angleDiff(targetAngle, currentAngle, true)) {
                currentAngle += speed * deltaTime;
                change = true;
                if (currentAngle >= targetAngle) currentAngle = targetAngle;
            }
            else {
                currentAngle -= speed * deltaTime;
                change = true;
                currentAngle = normalizeAngle(currentAngle);
            }
        }
        else if (currentAngle > targetAngle) {
            if (angleDiff(targetAngle, currentAngle, false) < angleDiff(targetAngle, currentAngle, true))
            {
                currentAngle -= speed * deltaTime;
                change = true;
                if (currentAngle <= targetAngle) currentAngle = targetAngle;
            }
            else
            {
                currentAngle += speed * deltaTime;
                change = true;
                currentAngle = normalizeAngle(currentAngle);
            }
        }
        return change;
	}


}
