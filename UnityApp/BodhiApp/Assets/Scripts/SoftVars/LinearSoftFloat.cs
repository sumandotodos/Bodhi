using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearSoftFloat : SoftFloat {

    float curValue;
    float targetValue;
    float speed;

    public override float GetValue()
    {
        return curValue;
    }

    public override void SetValue(float _target)
    {
        targetValue = _target;
    }

    public override void SetValueImmediate(float value)
    {
        curValue = targetValue = value;
    }

    public override void SetSpeed(float sp)
    {
        speed = sp;
    }

    public override bool Update(float deltaTime)
    {
        bool change = false;
        if(curValue < targetValue) {
            curValue += speed * deltaTime;
            change = true;
            if (curValue > targetValue) curValue = targetValue;
        }
        if(curValue > targetValue) {
            curValue -= speed * deltaTime;
            change = true;
            if (curValue < targetValue) curValue = targetValue;
        }
        return change;
    }

}
