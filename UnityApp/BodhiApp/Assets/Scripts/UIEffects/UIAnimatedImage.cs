using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class AnimSegment
{
    public string name;
    public int start;
    public int end;
    public float speed;
    public bool loop;
}

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

    int PlayheadStart = 0;
    int PlayheadEnd = 0;

    public AnimSegment[] segments;

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
        if(state == 2)
        {
            time -= Time.deltaTime;
            if(time < 0.0f)
            {
                currentFrame = PlayheadStart;
                theImage.sprite = image[currentFrame];
                time = 0.0f;
                state = 1;
            }
        }

    }

	public void setFrame(int f) {
		currentFrame = (f % image.Length);
		theImage.sprite = image [currentFrame];
	}

	public void go() {
        currentFrame = PlayheadStart;
        theImage.sprite = image[currentFrame];
        state = 1;
	}

    public void go(float Delay)
    {
        state = 2;
        time = Delay;
    }

    public void reset() {
		state = 0;
		currentFrame = 0;
		theImage.sprite = image [0];
		time = 0.0f;
	}

    public void PlaySegment(int s)
    {
        currentFrame = PlayheadStart = segments[s].start;
        PlayheadEnd = segments[s].end;
        loop = segments[s].loop;
        if (segments[s].speed > 0.0f)
        {
            animationSpeed = segments[s].speed;
        }
    }

    public void PlaySegment(string name)
    {
        for(int i = 0; i < segments.Length; ++i)
        {
            if(segments[i].name == name)
            {
                PlaySegment(i);
            }
        }
    }
}
