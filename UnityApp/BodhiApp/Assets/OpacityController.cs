using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OpacityController : MonoBehaviour
{
    SpriteRenderer spr;
    public float op;
    public float targetOp;
    float opSpeed = 2.0f;
    public float delayWindow = 0.05f;
    bool isStarted = false;
    public float remaining = 0.0f;
    public float targetOpAfterDelay;
    // Start is called before the first frame update
    public void Start()
    {
        if (isStarted) return;
        isStarted = true;
        spr = this.GetComponent<SpriteRenderer>();
        op = targetOp = targetOpAfterDelay = 1.0f;
        updateOpacity();
    }

    // Update is called once per frame
    void Update()
    {
        updateValue();
        updateOpacity();
        if(remaining>0.0f)
        {
            remaining -= Time.deltaTime;
            if(remaining <= 0.0f)
            {
                targetOp = targetOpAfterDelay;
            }
        }
    }

    private void updateValue()
    {
        if(op < targetOp)
        {
            op += opSpeed * Time.deltaTime;
            if(op > targetOp)
            {
                op = targetOp;
            }
        }

        if(op > targetOp)
        {
            op -= opSpeed * Time.deltaTime;
            if (op < targetOp)
            {
                op = targetOp;
            }
        }

        updateOpacity();
    }

    private void updateOpacity()
    {
        spr.color = new Color(1, 1, 1, op);
    }

    public void SetOpacityImmediate(float newOp)
    {
        op = targetOp = newOp;
        updateOpacity();
    }

    public void SetOpacity(float newOp)
    {
        targetOp = newOp;
    }

    public void SetOpacity(float newOp, int delay)
    {
        targetOpAfterDelay = newOp;
        remaining = ((float)delay) * delayWindow;
    }
}
