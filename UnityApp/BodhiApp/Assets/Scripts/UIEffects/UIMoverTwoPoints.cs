using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIMoverTwoPoints : MonoBehaviour {

    public Vector2 PointA;
    public Vector2 PointB;
    public float Speed;
    public EaseType easeType;
    public UICoordinateType coordinateType;
    public UISpaceType space = UISpaceType.World;
    public UITransformType transformType = UITransformType.Absolute;


    TweenableSoftFloat TParam;

    public bool AutoStart = true;

    Vector2 Initial;

    bool started = false;

	// Use this for initialization
	public void Start () {
        if (started) return;
        started = true;
        TParam = new TweenableSoftFloat();
        Initial = this.transform.position;
        if(space == UISpaceType.Local)
        {
            Initial = this.transform.localPosition;
        }
        if (AutoStart)
        {
            Go();
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(TParam.update())
        {
            float t = TParam.getValue();
            Vector2 CurrentPosition = PointA + (PointB-PointA)*t;
            if (space == UISpaceType.World)
            {
                if (transformType == UITransformType.Absolute)
                {
                    this.transform.position = FGUtils.UICoordinateTransform(CurrentPosition, coordinateType);
                }
                else
                {
                    this.transform.position = FGUtils.UICoordinateTransform(CurrentPosition+Initial, coordinateType);
                }
            }
            else
            {
                if (transformType == UITransformType.Absolute)
                {
                    this.transform.localPosition = FGUtils.UICoordinateTransform(CurrentPosition, coordinateType);
                }
                else
                {
                    this.transform.localPosition = FGUtils.UICoordinateTransform(CurrentPosition + Initial, coordinateType);
                }
            }
        }
    }

    public void Go()
    {
        TParam.setValueImmediate(0);
        TParam.setSpeed(Speed);
        TParam.setEaseType(easeType);
        TParam.setValue(1.0f);
    }

    public void Return()
    {
        TParam.setValueImmediate(1.0f);
        TParam.setSpeed(Speed);
        TParam.setEaseType(easeType);
        TParam.setValue(0.0f);
    }


}
