using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalObject : MonoBehaviour
{
    virtual public void SetYAngleImmediate(float angle)
    {

    }

    virtual public void SetXAngleImmediate(float angle)
    {

    }
}

public class OrbitalCamera : OrbitalObject
{
    public GameObject YPivot;
    public GameObject XPivot;
    public GameObject CameraHolder;

    public GameObject Target_N;

    SoftVector3 position;
    TweenableSoftFloat SoftY;
    TweenableSoftFloat SoftX;
    TweenableSoftFloat SoftDist;
    bool isStarted = false;

    public void Start()
    {
        if (isStarted) return;
        isStarted = true;
        SoftX = new TweenableSoftFloat();
        SoftY = new TweenableSoftFloat();
        SoftDist = new TweenableSoftFloat(-CameraHolder.transform.localPosition.z);
        position = new SoftVector3();
        position.setValueImmediate(this.transform.position);
        SoftX.setSpeed(30.0f);
        SoftX.setValueImmediate(18.5f);
        SoftY.setSpeed(30.0f);
        SoftDist.setSpeed(14.0f);
        position.speed = 0.5f;
        SoftX.setEaseType(EaseType.sigmoid);
        SoftY.setEaseType(EaseType.sigmoid);
        SoftDist.setEaseType(EaseType.cubicOut);
    }

    private void Update()
    {
        if(position.update())
        {
            this.transform.position = position.getValue();
        }
        if (SoftX.update())
        {
            XPivot.transform.localRotation = Quaternion.Euler(SoftX.getValue(), 0, 0);
        }
        if(SoftY.update())
        {
            YPivot.transform.localRotation = Quaternion.Euler(0, SoftY.getValue(), 0);
        }
        if(SoftDist.update())
        {
            CameraHolder.transform.localPosition = new Vector3(0, 0, -SoftDist.getValue());
        }
    }

    public void SetPosition(Vector3 pos)
    {
        position.setValue(pos);
    }

    override public void SetYAngleImmediate(float angle)
    {
        SoftY.setValueImmediate(angle);
        YPivot.transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    override public void SetXAngleImmediate(float angle)
    {
        SoftX.setValueImmediate(angle);
        XPivot.transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }

    public void SetZDistanceImmediate(float distance)
    {
        SoftDist.setValueImmediate(distance);
        CameraHolder.transform.localPosition = new Vector3(0, 0, -distance);
    }

    public void SetYAngle(float angle)
    {
        SoftY.setValue(angle);
    }

    public void SetXAngle(float angle)
    {
        SoftX.setValue(angle);
    }

    public void SetZDistance(float distance)
    {
        SoftDist.setValue(distance);
    }

    public void SetZoomInConfiguration()
    {
        SoftDist.setEaseType(EaseType.linear);
        SoftDist.setSpeed(7.0f);
    }


}
