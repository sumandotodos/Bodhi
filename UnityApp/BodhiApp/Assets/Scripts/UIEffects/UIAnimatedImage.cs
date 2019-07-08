using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAnimatedImage : MonoBehaviour {

	public Sprite[] image;
	SpriteRenderer theImage;
	int currentFrame;
	public bool autostart = true;
	public bool loop = true;
	public int offset = 0;
	int state = 0;
	float time;
	public float animationSpeed;

	bool started = false;

    public bool randomStartFrame = false;

    public int PlayheadStart = 0;
    public int PlayheadEnd = 0;
    int playhead;

	// Use this for initialization
	public void Start () {

		if (started)
			return;
		started = true;
	
		currentFrame = offset % image.Length;
		theImage = this.GetComponent<SpriteRenderer> ();
		theImage.sprite = image [0];
		time = 0.0f;
		state = 0;

        if (randomStartFrame)
        {
            currentFrame = Random.RandomRange(PlayheadStart, PlayheadEnd);
        }
        else
        {
            currentFrame = PlayheadStart;
        }

        if (autostart)
			state = 1;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (state == 0) {

		}
		if (state == 1) {
			time += Time.deltaTime;
			if (time > (1.0f / animationSpeed)) {
				time = 0.0f;

               currentFrame = (currentFrame + 1);// % image.Length;
               if (loop)
               {
                   if (currentFrame == image.Length)
                   {
                            currentFrame = 0;
                   }
                   else if (currentFrame == PlayheadEnd)
                   {
                        currentFrame = PlayheadStart;
                   }
                }
                else
                {
                    if (currentFrame == image.Length)
                    {
                        currentFrame = image.Length - 1;
                        state = 0;
                    }
                    else if (currentFrame == PlayheadEnd+1)
                    {
                        currentFrame = PlayheadEnd;
                        state = 0;
                    }


                }
                theImage.sprite = image[currentFrame];

			}
		}

	}

	public void setFrame(int f) {
		currentFrame = (f % image.Length);
		theImage.sprite = image [currentFrame];
	}

	public void go() {
		state = 1;
	}

	public void reset() {
		state = 0;
		currentFrame = 0;
		theImage.sprite = image [0];
		time = 0.0f;
	}
}
