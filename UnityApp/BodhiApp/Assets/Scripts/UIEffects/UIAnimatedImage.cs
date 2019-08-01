using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class AnimSegment
{
    public string name;
    public int start;
    public int resetStart;
    public int end;
    public float speed;
    public bool loop;
}

public class UIAnimatedImage : MonoBehaviour {

	public Sprite[] image;
	public SpriteRenderer theSpriteRenderer;
    public Image theImage;
	public int currentFrame;
	public bool autostart = true;
	public bool loop = true;
	public int offset = 0;
	public int state = 0;
	public float time;
	public float animationSpeed;

	bool started = false;

    public bool randomStartFrame = false;

    int PlayheadStart = 0;
    int PlayheadEnd = 0;
    int PlayheadLoop = 0;

    public AnimSegment[] segments;

    int playhead;

	// Use this for initialization
	public void Start () {

		if (started)
			return;
		started = true;
	
		currentFrame = offset % image.Length;
		theImage = this.GetComponent<Image> ();
        theSpriteRenderer = this.GetComponent<SpriteRenderer>();
        if (theImage != null)
        {
            theImage.sprite = image[0];
        }
        if (theSpriteRenderer != null)
        {
            theSpriteRenderer.sprite = image[0];
        }
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
                        currentFrame = PlayheadLoop;
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
                if (theImage != null)
                {
                    theImage.sprite = image[currentFrame];
                }
                if (theSpriteRenderer != null)
                {
                    theSpriteRenderer.sprite = image[currentFrame];
                }

            }
		}
        if(state == 2)
        {
            time -= Time.deltaTime;
            if(time < 0.0f)
            {
                currentFrame = PlayheadStart;
                if (theImage != null)
                {
                    theImage.sprite = image[currentFrame];
                }
                if (theSpriteRenderer != null)
                {
                    theSpriteRenderer.sprite = image[currentFrame];
                }
                time = 0.0f;
                state = 1;
            }
        }

    }

	public void setFrame(int f) {
		currentFrame = (f % image.Length);
        if (theImage != null)
        {
            theImage.sprite = image[currentFrame];
        }
        if (theSpriteRenderer != null)
        {
            theSpriteRenderer.sprite = image[currentFrame];
        }
    }

	public void go() {
        currentFrame = PlayheadStart;
        if (theImage != null)
        {
            theImage.sprite = image[currentFrame];
        }
        if (theSpriteRenderer != null)
        {
            theSpriteRenderer.sprite = image[currentFrame];
        }
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
        if (theImage != null)
        {
            theImage.sprite = image[0];
        }
        if (theSpriteRenderer != null)
        {
            theSpriteRenderer.sprite = image[0];
        }
        time = 0.0f;
	}

    public void PlaySegment(int s)
    {
        currentFrame = PlayheadStart = segments[s].start;
        PlayheadLoop = segments[s].resetStart;
        PlayheadEnd = segments[s].end;
        loop = segments[s].loop;
        if (segments[s].speed > 0.0f)
        {
            animationSpeed = segments[s].speed;
        }
        go();
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
