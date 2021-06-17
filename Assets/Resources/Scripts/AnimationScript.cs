using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private SpriteRenderer spr;
    public string path;
    public string csprite;
    private bool loop;
    private float frametime;
    private bool flip;
    private bool animate;
    private int frames;
    private bool end;
    public float speed;
    private Sprite[] anim;
    HitBoxPreLoader hb;
    //hitboxloader 3d should not be attached for optimization

    // Start is called before the first frame update
    void Start()
    {
        hb = gameObject.GetComponent<HitBoxPreLoader>();
        flip = false;
        updateSprite();
        spr = gameObject.GetComponentInChildren<SpriteRenderer>();
        frametime = 0;
        loop = true;
    }
    private void updateSprite()
    {
        //update sprite when necessary
        //if(System.IO.File.Exists("Assets/Resources/Sprites/" + path + "/" + csprite))
        if(path.Equals(""))
            anim = Resources.LoadAll<Sprite>("Sprites/" + csprite);
        else
            anim = Resources.LoadAll<Sprite>("Sprites/" + path + "/" + csprite);


    }
    public void setFlip(bool f)
    {
        flip = f;
    }
    public bool getFlip()
    {
        return flip;
    }
    public void changeSprite(string sprite)
    {
        csprite = sprite;
        frametime = 0f;
        updateSprite();
    }
    public void changeSprite(string sprite, bool reset)
    {
        csprite = sprite;
        if (reset == true)
        frametime = 0f;
        updateSprite();
    }
    public void setPath(string p)
    {
        path = p;
        frametime = 0f;
        updateSprite();
    }
    public void setPath(string p, bool reset)
    {
        path = p;
        if(reset == true)
        frametime = 0f;
        updateSprite();
    }
    public void setLoop(bool set)
    {
        loop = set;
    }
    public bool getLoop()
    {
        return loop;
    }
    public void changeSprite(string sprite, string p)
    {
        csprite = sprite;
        path = p;
        frametime = 0f;
        updateSprite();
    }
    public void changeSprite(string sprite, string p, bool reset)
    {
        csprite = sprite;
        path = p;
        if(reset == true)
        frametime = 0f;
        updateSprite();
    }
    public void setFrame(int f)
    {
        frametime = (float)f;
    }
    public int getFrame()
    {
        return Mathf.FloorToInt(frametime);
    }
    public float getSpeed()
    {
        return speed;
    }
    public void setSpeed(float s)
    {
        speed = s;
    }
    public string getSprite()
    {
        return csprite;
    }
    public string getPath()
    {
        return path;
    }
    public string getSpritePath()
    {
        return "Sprites/" + path + "/" + csprite;
    }
    public bool animEnd()
    {
        return end;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (end == true)
            end = false;
        spr.flipX = flip;
        frames = anim.Length;
        //calc frame from frametimings
        int frame = Mathf.FloorToInt(frametime);
        //add to frametimes
        frametime += speed * Time.deltaTime;
        if(frametime > frames)
        {
            end = true;
            if (loop == true)
            {
                frametime = 0f;
            }
            else
            {
                frametime -= 1f;
                if(frametime < 0f)
                {
                    frametime = 0f;
                }
            }
        }
        if (hb != null)
        {
            hb.anim = csprite;
            hb.frame = frame;
            hb.flipped = flip;
        }
        Sprite change = null;   
        if (anim != null && anim[frame])
            change = anim[frame];
        if (change != null)
            spr.sprite = change;
        else
            Debug.Log("NULL");
    }
}
