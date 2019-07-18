using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIMoverTwoPoints : MonoBehaviour {

    public Vector2 PointA;
    public Vector2 PointB;
    public float Speed;
    public EaseType easeType;
    public UICoordinateType coordinateType;

    TweenableSoftFloat TParam;

    public bool AutoStart = true;

	// Use this for initialization
	void Start () {
        TParam = new TweenableSoftFloat();
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
            this.transform.position = FGUtils.UICoordinateTransform(CurrentPosition, coordinateType);
        }
    }

    public void Go()
    {
        TParam.setValueImmediate(0);
        TParam.setSpeed(Speed);
        TParam.setEaseType(easeType);
        TParam.setValue(1.0f);
    }


}
