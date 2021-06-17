using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using BQ.CollisionInfo;

public class HitBoxPreLoader : MonoBehaviour
{
    private Dictionary<string, List<HitboxInfo>> hitboxdict = new Dictionary<string, List<HitboxInfo>>();
    private Dictionary<string, List<HitboxInfo>> hurtboxdict = new Dictionary<string, List<HitboxInfo>>();
    private List<HitboxInfo> currentHurtboxes = new List<HitboxInfo>();
    private List<HitboxInfo> currentHitboxes = new List<HitboxInfo>();
    public string anim = null;
    private string sprite = null;
    public int frame = 0;
    private int renderedframe = 0;
    private bool ready = false;
    public bool flipped = false;

    // Start is called before the first frame update
    void Start()
    {
        AddBoxes("billy");
    }

    // Update is called once per frame
    void Update()
    {
        if(sprite != anim)
        {
            sprite = anim;
            UpdateCollision();
        }
        if (renderedframe != frame)
        {
            renderedframe = frame;
            UpdateCollision();
        }
    }

    private void UpdateCollision()
    {
        if (ready == false)
            return;
        DeleteCollision();
        if (hitboxdict.ContainsKey(sprite)) {
            List<HitboxInfo> hitboxes = hitboxdict[sprite];
            foreach (HitboxInfo i in hitboxes)
            {
                if (renderedframe == i.frame)
                {
                    CreateHitBox(i, flipped, "hitbox");
                    currentHitboxes.Add(i);
                }
            } 
        }
        if (hurtboxdict.ContainsKey(sprite))
        {
            List<HitboxInfo> hitboxes = hurtboxdict[sprite];
            foreach (HitboxInfo i in hitboxes)
            {
                if (renderedframe == i.frame)
                {
                    CreateHitBox(i, flipped, "hurtbox");
                    currentHurtboxes.Add(i);
                }
            }
        }
    }

    private void DeleteCollision()
    {
        foreach(HitboxInfo i in currentHitboxes)
        {
            foreach(BoxCollider j in gameObject.GetComponents<BoxCollider>())
            {
                if (i.CheckAgainstBoxCollider(j) && j.tag == "hitbox")
                {
                    Destroy(j);
                    continue;
                }
            }
            
        }
        foreach (HitboxInfo i in currentHurtboxes)
        {
            foreach (BoxCollider j in gameObject.GetComponents<BoxCollider>())
            {
                if (i.CheckAgainstBoxCollider(j) && j.tag == "hurtbox")
                {
                    Destroy(j);
                    continue;
                }
            }

        }
        currentHitboxes.Clear();
    }

    private void AddBoxes(string character)
    {
        string[] files = Directory.GetFiles("Assets/Resources/Sprites/" + character + "/Collision/", "*JSON", SearchOption.AllDirectories);
        foreach (string box in files)
        {
            ProcessBoxes(box, "hitbox", ref hitboxdict);
            ProcessBoxes(box, "hurtbox", ref hurtboxdict);
        }
        ready = true;
    }

    private void ProcessBoxes(string box, string hbkey, ref Dictionary<string, List<HitboxInfo>> dict)
    {
        List<HitboxInfo> hitboxes = new List<HitboxInfo>();
        HitboxInfo boxes = null;
        string savestring = box.Substring(box.LastIndexOf('/') + 1, box.LastIndexOf('.') - box.LastIndexOf('/') - 1);
        Debug.Log(savestring);
        StreamReader s = new StreamReader(box);
        string data = s.ReadToEnd();
        //parse out collision data
        JSONNode collision = SimpleJSON.JSON.Parse(data);
        float x, y, w, h;

        foreach (KeyValuePair<string, JSONNode> kvp in (JSONObject)collision[hbkey])
        {
            string key = kvp.Key;
            int framenum = int.Parse(key.Split(' ')[1]);
            if (collision[hbkey][key] == null)
                continue;
            int boxcounts = 0;
            //count the boxes
            while (collision[hbkey][key][boxcounts]["w"] != null)
            {
                ++boxcounts;
            }
            while (boxcounts >= 1)
            {
                //dividing the hbox output by 100 gets unity pixels, but the offsets are screwy
                x = (collision[hbkey][key][boxcounts - 1]["x"] / 100f);
                w = collision[hbkey][key][boxcounts - 1]["w"] / 100f;
                h = collision[hbkey][key][boxcounts - 1]["h"] / 100f;
                //fix the offset of x by adjusting it to the right 1 pixel for every length unit
                x += 0.005f * (w * 100f);
                //fix the offset of y by making the offset (-1*offset)-0.005*height
                y = -1 * (collision[hbkey][key][boxcounts - 1]["y"] / 100f) - (0.005f * (h * 100f));
                /*if (anim.getFlip() == true)
                {
                    x = -x;
                    y = -y;
                }*/
                boxes = new HitboxInfo(x, y, w, h, framenum);
                --boxcounts;
            }
            if (boxes != null)
                hitboxes.Add(boxes);
        }
        if (hitboxes.Count != 0)
            dict[savestring] = hitboxes;

    }

    public void CreateHitBox(HitboxInfo hb, bool flipped, string tag)
    {
        Debug.Log("CREATE");
        //add the collider and start a new box
        var boxcollider = gameObject.AddComponent<BoxCollider>();
        if (!flipped)
        {
            boxcollider.center += new Vector3(hb.x, hb.y, 0);
            boxcollider.size = new Vector3(hb.w, hb.h, 0.4f);
        }
        else
        {
            boxcollider.center += new Vector3(-hb.x, -hb.y, 0);
            boxcollider.size = new Vector3(hb.w,hb.h, 0.4f);
        }
        boxcollider.tag = tag;
        boxcollider.isTrigger = true;
        boxcollider.enabled = true;
    }

    public void DeleteHitbox(BoxCollider todest)
    {
        Destroy(todest);
    }
}
