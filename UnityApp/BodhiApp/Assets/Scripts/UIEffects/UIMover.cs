using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMover : MonoBehaviour {

	public float factor = 1;
	public float xSpeed, ySpeed;

	Vector3 initialPosition;

    // Use this for initialization
    bool started = false;
	public void Start () {
        if (started) return;
        started = true;
		initialPosition = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 t = this.transform.localPosition;
		t.x += xSpeed * factor * Time.deltaTime;
		t.y += ySpeed * factor * Time.deltaTime;
		this.transform.localPosition = t;
	}

	public void Reset()
	{
		this.transform.localPosition = initialPosition;
	}
}
