using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoftFloat {

    public abstract float GetValue();
    public abstract void SetValue(float _target);
    public abstract void SetValueImmediate(float value);
    public abstract void SetSpeed(float sp);
    public abstract bool Update(float deltaTime);
}
