using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class BrawlCollider
{
    private BoxCollider box;
    private int id;
    private bool boxtype;

    //boxtype: true = hitbox false = hurtbox
    //declare brawl and boxcolliders
    public BrawlCollider()
    {
        box = null;
        id = 0;
        boxtype = true;

    }
    public BrawlCollider(BoxCollider b, bool type, int i)
    {
        id = i;
        boxtype = type;
        box = b;
    }
    //setters and getters
    public void setBox(BoxCollider b)
    {
        b = box;
    }
    public void setBox(BrawlCollider b)
    {
        box = b.getBox();
    }
    public void setBox(BoxCollider b, bool type)
    {
        b = box;
        boxtype = type;
    }
    public void setBox(BoxCollider b, int i)
    {
        b = box;
        id = i;
    }
    public void setBox(BoxCollider b, bool type, int i)
    {
        b = box;
        boxtype = type;
        id = i;
    }
    public int getId()
    {
        return id;
    }
    public bool getType()
    {
        return boxtype;
    }
    public BoxCollider getBox()
    {
        return box;
    }
    //destroy the box when necessary
    public void destroy()
    {
        Object.Destroy(box);
    }
}

public class BrawlManager : MonoBehaviour
{
    //create array of boxes and put all the brawlcolliders inside
    public BrawlCollider[] boxes;
    public BrawlManager()
    {
        boxes = new BrawlCollider[0];
    }
    public void updateBoxes(BoxCollider[] newboxes)
    {
        boxes = new BrawlCollider[newboxes.Length];
        for(int i = 0; i < newboxes.Length-1; ++i)
        {
            boxes[i].setBox(newboxes[i], i);
        }
    }
    //get properties of a brawlcollider
    public BrawlCollider getBox(int i)
    {
        return boxes[i];
    }
    public BrawlCollider[] getBoxes()
    {
        return boxes;
    }
    public void destroyAll()
    {
        if (boxes.Length != 0) {
            
            for (int i = 0; i < boxes.Length - 1; ++i)
            {
                boxes[i].destroy();
            }
        }
        boxes = new BrawlCollider[0];
    }
    public void destroyAllButDef()
    {
        if (boxes.Length == 0)
            return;
        BrawlCollider b = boxes[0];
        if (boxes.Length > 1)
        {
            for (int i = 1; i < boxes.Length; ++i)
            {
                boxes[i].destroy();
            }
        }
        boxes = new BrawlCollider[1];
        boxes[0] = b;
    }
    public void addBox(BoxCollider b, bool type)
    {
        BrawlCollider[] newbox = new BrawlCollider[boxes.Length + 1];
        for (int i = 0; i < boxes.Length; ++i)
        {
            newbox[i] = boxes[i];
        }
        newbox[boxes.Length] = new BrawlCollider(b, type, newbox.Length);
        boxes = newbox;
    }
}


public class HitBoxLoader3D : MonoBehaviour
{
    public string box;
    private int frame;
    public string path;
    AnimationScript anim;
    BrawlManager boxes;
    private bool defactive;
  

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<AnimationScript>();
        boxes = gameObject.AddComponent<BrawlManager>();
        Default();
    }
    // Update is called once per frame
    void Update()
    {
        if (frame != anim.getFrame() || !anim.getSprite().Equals(box) || !anim.getPath().Equals(path))
        {
            path = anim.getPath();
            box = anim.getSprite();
            frame = anim.getFrame();
            UpdateBoxes();
        }
    }
    public void defActive(bool tf)
    {
        defactive = tf;
    }
    //set up singular default hitbox for a character
    void Default()
    {
        boxes.destroyAll();
        if (!System.IO.File.Exists("Assets/Resources/Sprites/" + path + "/Collision/" + "Default.json"))
            return;
        //select frame data from json file(edit to become sprite based)
        frame = 0;
        StreamReader s = new StreamReader("Assets/Resources/Sprites/" + path + "/Collision/" + "Default.json");
        if (s == null)
        {
            return;
        }
        string data = s.ReadToEnd();
        //parse out collision data
        JSONNode collision = SimpleJSON.JSON.Parse(data);
        //create all hitboxes on the player
        float x, y, w, h;
        //dividing the hbox output by 100 gets unity pixels, but the offsets are screwy
        x = (collision["hitbox"]["frame " + frame.ToString()][0]["x"] / 100f);
        w = collision["hitbox"]["frame " + frame.ToString()][0]["w"] / 100f;
        h = collision["hitbox"]["frame " + frame.ToString()][0]["h"] / 100f;
        //fix the offset of x by adjusting it to the right 1 pixel for every length unit
        x += 0.005f * (w * 100f);
        //fix the offset of y by making the offset (-1*offset)-0.005*height
        y = -1 * (collision["hitbox"]["frame " + frame.ToString()][0]["y"] / 100f) - (0.005f * (h * 100f));
        //add the collider and start a new box
        var boxcollider = gameObject.AddComponent<BoxCollider>();
        //boxcollider.offset = new Vector2(x, y);
        boxcollider.center = new Vector3(x, y, 0f);
        boxcollider.size = new Vector3(w, h, 0.4f);
        boxcollider.isTrigger = true;
        boxes.addBox(boxcollider, false);
        UpdateBoxes();
       
    }
    public void newdef(string b)
    {
        boxes.destroyAll();
        //select frame data from json file(edit to become sprite based)
        frame = 0;
        if (!System.IO.File.Exists("Assets/Resources/Sprites/" + path + "/Collision/" + b + ".json"))
            return;
        StreamReader s = new StreamReader("Assets/Resources/Sprites/" + path + "/Collision/" + b + ".json");
        if (s == null)
        {
            return;
        }
        string data = s.ReadToEnd();
        //parse out collision data
        JSONNode collision = SimpleJSON.JSON.Parse(data);
        //create all hitboxes on the player
        float x, y, w, h;
        //dividing the hbox output by 100 gets unity pixels, but the offsets are screwy
        x = (collision["hitbox"]["frame " + frame.ToString()][0]["x"] / 100f);
        w = collision["hitbox"]["frame " + frame.ToString()][0]["w"] / 100f;
        h = collision["hitbox"]["frame " + frame.ToString()][0]["h"] / 100f;
        //fix the offset of x by adjusting it to the right 1 pixel for every length unit
        x += 0.005f * (w * 100f);
        //fix the offset of y by making the offset (-1*offset)-0.005*height
        y = -1 * (collision["hitbox"]["frame " + frame.ToString()][0]["y"] / 100f) - (0.005f * (h * 100f));
        //add the collider and start a new box
        var boxcollider = gameObject.AddComponent<BoxCollider>();
        //boxcollider.offset = new Vector2(x, y);
        boxcollider.center = new Vector3(x, y, 0f);
        boxcollider.size = new Vector3(w, h, 0.4f);
        boxcollider.isTrigger = true;
        boxes.addBox(boxcollider, false);

        UpdateBoxes();
    }
    public void UpdateBoxes()
    {
        {
            boxes.destroyAllButDef();
            if (!System.IO.File.Exists("Assets/Resources/Sprites/" + path + "/Collision/" + box + ".json"))
                return;
            StreamReader s = new StreamReader("Assets/Resources/Sprites/" + path + "/Collision/" + box + ".json");
            string data = s.ReadToEnd();
            //parse out collision data
            JSONNode collision = SimpleJSON.JSON.Parse(data);
            int boxcounts = 0;
            //count the boxes
            while (collision["hitbox"]["frame " + frame.ToString()][boxcounts]["w"] != null)
            {
                ++boxcounts;
            }
            //create all hitboxes on the player
            while (boxcounts >= 1)
            {
                float x, y, w, h;
                //dividing the hbox output by 100 gets unity pixels, but the offsets are screwy
                x = (collision["hitbox"]["frame " + frame.ToString()][boxcounts - 1]["x"] / 100f);
                w = collision["hitbox"]["frame " + frame.ToString()][boxcounts - 1]["w"] / 100f;
                h = collision["hitbox"]["frame " + frame.ToString()][boxcounts - 1]["h"] / 100f;
                //fix the offset of x by adjusting it to the right 1 pixel for every length unit
                x += 0.005f * (w * 100f);
                //fix the offset of y by making the offset (-1*offset)-0.005*height
                y = -1 * (collision["hitbox"]["frame " + frame.ToString()][boxcounts - 1]["y"] / 100f) - (0.005f * (h * 100f));
                if (anim.getFlip() == true)
                {
                    x = -x;
                    y = -y;
                }
                //add the collider and start a new box
                var boxcollider = gameObject.AddComponent<BoxCollider>();
                boxcollider.center += new Vector3(x, y, 0);
                //boxcollider.offset = new Vector2(x, y);
                boxcollider.size = new Vector3(w, h, 0.4f);
                boxcollider.isTrigger = true;
                boxes.addBox(boxcollider, true);
                --boxcounts;
            }
            while (collision["hurtbox"]["frame " + frame.ToString()][boxcounts]["w"] != null)
            {
                ++boxcounts;
            }
            //create all hurtboxes on the player
            while (boxcounts >= 1)
            {
                float x, y, w, h;
                //dividing the hbox output by 100 gets unity pixels, but the offsets are screwy
                x = (collision["hurtbox"]["frame " + frame.ToString()][boxcounts - 1]["x"] / 100f);
                w = collision["hurtbox"]["frame " + frame.ToString()][boxcounts - 1]["w"] / 100f;
                h = collision["hurtbox"]["frame " + frame.ToString()][boxcounts - 1]["h"] / 100f;
                //fix the offset of x by adjusting it to the right 1 pixel for every length unit
                x += 0.005f * (w * 100f);
                //fix the offset of y by making the offset (-1*offset)-0.005*height
                y = -1 * (collision["hurtbox"]["frame " + frame.ToString()][boxcounts - 1]["y"] / 100f) - (0.005f * (h * 100f));
                //add the collider and start a new box
                var boxcollider = gameObject.AddComponent<BoxCollider>();
                //boxcollider.offset = new Vector2(x, y);
                boxcollider.center = new Vector3(x, y, 0f);
                boxcollider.size = new Vector3(w, h, 0.4f);
                boxcollider.isTrigger = true;
                boxes.addBox(boxcollider, false);
                --boxcounts;
            }
        }
    }

}