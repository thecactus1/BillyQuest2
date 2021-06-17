using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    private string[] boolstatuses = { "active", "movable", "animated", "canattack", "player", "collision"};
    private string[] stringstatuses = { "name", "sprite"};
    private string[] floatstatuses = { "mspeed", "rspeed", "decellerate", "threshold", "vspeed", "gravity" , "jspeed"};
    private string[] intstatuses = { };
    private bool[] bools;
    private float[] floats;
    private int[] ints;
    private string[] strings;
    public Status(){
        strings = new string[stringstatuses.Length];
        for (int i = 0; i < strings.Length; ++i)
        {
            strings[i] = "";
        }
        bools = new bool[boolstatuses.Length];
        for(int i = 0; i < bools.Length; ++i)
        {
            bools[i] = false;
        }
        floats = new float[floatstatuses.Length];
        for (int i = 0; i < floats.Length; ++i)
        {
            floats[i] = 0f;
        }
        ints = new int[intstatuses.Length];
        for (int i = 0; i < ints.Length; ++i)
        {
            ints[i] = 0;
        }
        //default values
        setFloat("threshold", 0.2f);
        setBool("animated", true);
        setBool("movable", true);
        setBool("canattack", true);
        setFloat("gravity", 0.2f);
    }
    //SetValue uses overloads to quickly set up values
    public void setValue(string name, int inp)
    {
        setInt(name, inp);
    }
    public void setValue(string name, string inp)
    {
        setString(name, inp);
    }
    public void fileValues(string path)
    {
        bool read = false;
        //read lines from a file and then set the values through the parser
        string[] lines = System.IO.File.ReadAllLines(path);
        for(int i = 0; i < lines.Length; ++i)
        {
            if (lines[i].Contains("<"))
                read = false;
            if (lines[i].Contains("<values>"))
                read = true;
            string[] seperate = lines[i].Split('=');
            bool setvalue = false;
            if (seperate.Length > 1)
                setvalue = true;    
            for(int g = 0; g < seperate.Length; ++g)
            {
                seperate[g] = seperate[g].Trim();
            }
            if (read == true && setvalue == true)
                setValueParse(seperate[0], seperate[1]);
        }
    }
    public void fileValues(string[] lines)
    {
        bool read = false;
        //read lines from a file and then set the values through the parser
        
        for (int i = 0; i < lines.Length; ++i)
        {
            if (lines[i].Contains("<"))
                read = false;
            if (lines[i].Contains("<values>"))
                read = true;
            string[] seperate = lines[i].Split('=');
            bool setvalue = false;
            if (seperate.Length > 1)
                setvalue = true;
            for (int g = 0; g < seperate.Length; ++g)
            {
                seperate[g] = seperate[g].Trim();
            }
            if (read == true && setvalue == true)
                setValueParse(seperate[0], seperate[1]);
        }
    }
    //parse out a value from a string
    public void setValueParse(string name, string inp)
    {
        for (int i = 0; i < floats.Length; ++i)
        {
            if (name == floatstatuses[i])
                floats[i] = float.Parse(inp);
        }
        for (int i = 0; i < ints.Length; ++i)
        {
            if (name == intstatuses[i])
                ints[i] = int.Parse(inp);
        }
        for (int i = 0; i < bools.Length; ++i)
        {
            if (name == boolstatuses[i])
            {
                if (inp.ToLower() == "true")
                    bools[i] = true;
                else
                    bools[i] = false;
            }
                
        }
        setString(name, inp);
    }
    public string getValueParse(string name)
    {
        for (int i = 0; i < floats.Length; ++i)
        {
            if (name == floatstatuses[i])
                return floats[i].ToString();
        }
        for (int i = 0; i < ints.Length; ++i)
        {
            if (name == intstatuses[i])
                return ints[i].ToString();
        }
        for (int i = 0; i < bools.Length; ++i)
        {
            if (name == boolstatuses[i])
                return bools[i].ToString();
        }
        return getString(name);
    }
    public void setValue(string name, float inp)
    {
        setFloat(name, inp);
    }
    public void setValue(string name, bool inp)
    {
        setBool(name, inp);
    }
    public int getInt(string name)
    {
        for (int i = 0; i < ints.Length; ++i)
        {
            if (name == intstatuses[i])
                return ints[i];
        }
        return 0;
    }
    public bool getBool(string name)
    {
        for (int i = 0; i < bools.Length; ++i)
        {
            if (name == boolstatuses[i])
                return bools[i];
        }
        return false;
    }
    public float getFloat(string name)
    {
        for (int i = 0; i < floats.Length; ++i)
        {
            if (name == floatstatuses[i])
                return floats[i];
        }
        return 0f;
    }
    public string getString(string name)
    {
        for(int i = 0; i < strings.Length; ++i)
        {
            if (name == stringstatuses[i])
                return strings[i];
        }
        return "";
    }
    public void setString(string name, string inp)
    {
        for (int i = 0; i < strings.Length; ++i)
        {
            if (name == stringstatuses[i])
                strings[i] = inp;
        }
    }
    public void setInt(string name, int inp)
    {
        for (int i = 0; i < ints.Length; ++i)
        {
            if (name == intstatuses[i])
                ints[i] = inp;
        }
    }
    public void setFloat(string name, float inp)
    {
        for (int i = 0; i < floats.Length; ++i)
        {
            if (name == floatstatuses[i])
                floats[i] = inp;
        }
    }
    public void setBool(string name, bool inp)
    {
        for (int i = 0; i < bools.Length; ++i)
        {
            if (name == boolstatuses[i])
                bools[i] = inp;
        }
    }
}

public class Attack
{
    public string name;
    public float speed;
    public string combo;
    public string anim;
    public float xsp;
    public float ysp;
    public float speedtime;
    public string chain;
    public bool cancellable;

    public Attack()
    {
        name = "";
        speed = 10f;
        combo = "";
        anim = "";
        chain = "";
    }
}

public class Attacks
{
    private Attack[] attacks;
    private string attackstring;
    public string altstring;
    public string lastatk;
    public string anim;
    public bool attacking;
    public float attacktimer;
    public Attacks()
    {
        anim = "";
        attackstring = "";
        altstring = "";
        attacks = new Attack[0];
        attacking = false;
    }
    private void addAttack(Attack add)
    {
        Attack[] newattack = new Attack[attacks.Length+1];
        for(int i = 0; i < attacks.Length; ++i)
        {
            newattack[i] = attacks[i];
        }
        newattack[attacks.Length] = add;
        attacks = newattack;
        
    }
    public Attack getAttack(string combo)
    {
        for(int i = 0; i < attacks.Length; ++i)
        {
            if(attacks[i].combo.Equals(combo))
                return attacks[i];
        }
        return new Attack();
    }
    public Attack getAttackName(string name)
    {
        for (int i = 0; i < attacks.Length; ++i)
        {
            if (attacks[i].name.Equals(name))
                return attacks[i];
        }
        return new Attack();
    }
    public void reset()
    {
        attackstring = "";
        altstring = "";
        lastatk = "";
    }
    public string getString()
    {
        return attackstring;
    }
    public string getAltString()
    {
        return altstring;
    }
    public void setString(string inp)
    {
        attackstring = inp;
    }
    public void addInput(string input)
    {
    if (attackstring.Length == 0)
        attackstring = input;
    else
        attackstring += "-" + input;
    }
    public bool scanAttacks(string combo)
    {
        //scan for an attack value based on the combo given
        for(int i = 0; i < attacks.Length; ++i)
        {
            if (attacks[i].combo == combo)
                return true;
        }
        return false;
    }
    public void attacksFromFile(string path)
    {
        int start = 0;
        int end = 0;
        string[] lines = System.IO.File.ReadAllLines(path);
        //count the attacks
        for (int i = 0; i < lines.Length; ++i)
        {
            if (lines[i].Contains("<") && start != 0)
            {
                end = i;
                break;
            }
            if (lines[i].Contains("<attacks>"))
            {
                start = i;
            }
        }
        int[] startpoints = new int[0];
        if(end == 0)
        {
            end = lines.Length;
        }
        //get the points at which attack data starts and ends
        for (int i = start; i < end; ++i)
        {
            if(lines[i].Contains("("))
            startpoints = ArrayTools.Push<int>(startpoints, i);
        }
        for(int i = 0; i < startpoints.Length; ++i)
        {
            for(int j = 0; j < lines.Length; ++j)
            {
                if(j == startpoints[i])
                {
                    Attack newatk = new Attack();
                    string name = lines[j].Replace('(', ' ');
                    name = name.Replace(')', ' ');
                    name = name.Trim();
                    newatk.name = name;
                    for(int g = j+1; g < lines.Length; ++g)
                    {
                        if (lines[g].Contains("("))
                        {
                            break;
                        }
                        //get the info and trim
                        string[] content = lines[g].Split('=');
                        content[0] = content[0].Trim().ToLower();
                        content[1] = content[1].Trim().ToLower();
                        switch (content[0])
                        {
                            case "combo":
                                newatk.combo = content[1];
                                break;
                            case "speed":
                                newatk.speed = float.Parse(content[1]);
                                break;
                            case "animation":
                                newatk.anim = content[1];
                                break;
                            case "xsp":
                                newatk.xsp = float.Parse(content[1]);
                                break;
                            case "ysp":
                                newatk.ysp = float.Parse(content[1]);
                                break;
                            case "speedtime":
                                newatk.speedtime = float.Parse(content[1]);
                                break;
                            case "chain":
                                newatk.chain = content[1];
                                break;
                            case "cancel":
                                newatk.cancellable = bool.Parse(content[1]);
                                break;
                            default:
                                break;
                        }
                    }
                    addAttack(newatk);
                }
            }
        }
    }
}

public class Character : MonoBehaviour
{
    private Directional[] axis = { new Directional("left"), new Directional("right"), new Directional("up"), new Directional("down") };
    public string defvalues;
    private GameObject shadow;
    private bool newinput;
    public Color textcolor;
    private int speedtime;
    private AnimationScript anim;
    private float xspeed;
    private float yspeed;
    private float zspeed;
    public string spritefolder; 
    public Status status;
    public Attacks attacks;
    private int walls;
    private bool isGrounded;
    public enum dir
    {
        up,
        down,
        left,
        right
    }
    private dir direction;

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true;
        walls = 0;
        newinput = true;
        string[] lines = System.IO.File.ReadAllLines(defvalues);
        anim = gameObject.GetComponent<AnimationScript>();
        status = new Status();
        attacks = new Attacks();
        status.fileValues(defvalues);
        attacks.attacksFromFile(defvalues);
        xspeed = 0f;
        yspeed = 0f;
        anim.setPath(spritefolder);
        direction = dir.right;
        gameObject.name = name.ToLower();
    }

    public Vector2 getSpeed()
    {
        return new Vector2(xspeed, yspeed);
    }
    public Vector3 getSpeedConvert()
    {
        return new Vector3(xspeed, yspeed, 0f);
    }
    //set speed
    public void setSpeed(Vector2 speed)
    {
        xspeed = speed.x; yspeed = speed.y;
    }
    public void setSpeed(float x, float y)
    {
        xspeed = x; yspeed = y;
    }
    public void setSpeedX(float x)
    {
        xspeed = x;
    }
    public void setSpeedY(float y)
    {
        yspeed = y;
    }
    public float getSpeedY()
    {
        return yspeed;
    }
    public float getSpeedX()
    {
        return xspeed;
    }
    public dir getDirection()
    {
        return direction;
    }
    public void setDirection(dir direct)
    {
        direction = direct;
    }
    public void setFolder(string fold)
    {
        spritefolder = fold;
    }
    public string getFolder()
    {
        return spritefolder;
    }
    public Color getColor()
    {
        return textcolor;
    }
    private void Gravity()
    {
        if (isGrounded == false)
            return;
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().velocity = getSpeedConvert();  
        if (direction == dir.left)
        {
            anim.setFlip(true);
        }
        if(direction == dir.right)
        {
            anim.setFlip(false);
        }
        //look for actives
        if (status.getBool("active") && status.getBool("player"))
        {
            if (status.getBool("movable") && !attacks.attacking)
                Move();
            if (status.getBool("canattack"))
                Attack();
            if (status.getBool("animated"))
                Animate();
        }
        Gravity();
        status.setString("sprite", anim.getSprite());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("wall"))
        {
            walls += 1;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("wall"))
        {
            walls -= 1;
        }
    }

    private void Attack()
    {
       Attack current = attacks.getAttack(attacks.getString());
       //implement attack cancelling
       if(attacks.getString() != current.combo && current.cancellable == true)
        {
            attacks.attacking = false;
            if (attacks.scanAttacks(attacks.getString()))
            executeAttack(attacks.getString());
        }
        if (attacks.attacktimer > 0 && (attacks.attacking == false || attacks.getString().EndsWith("d")))
        {
            attacks.attacktimer -= Time.deltaTime;
            if (attacks.attacktimer < 0)
                attacks.attacktimer = 0f;
        }
        //add in delay combos
        if(attacks.attacktimer == 0)
        {
            //check if a delay combo exists and if so start a delay combo
            if (!attacks.getString().Equals("") && !attacks.getString().Contains("d") && (attacks.scanAttacks(attacks.getString()+"-d-l") || attacks.scanAttacks(attacks.getString() + "-d-h")))
            {
                attacks.addInput("d");
                attacks.attacktimer = 0.3f;
            }
            else {
                    attacks.reset();
                    newinput = true;
            }
        }
        if (InputManager.getButtonPressed("Light") && newinput == true)
        {
            attacks.addInput("l");
            newinput = false;
        }
        //scan for combos and cancel if none
        if (!attacks.getString().Equals("") && attacks.attacking == false)
        {
            if (!current.chain.Equals(""))
            {
                attacks.setString(attacks.getAttackName(current.chain).combo);
            }
            if (!attacks.lastatk.Equals(attacks.getString()))
            {
                if (InputManager.doubletap == true && (attacks.scanAttacks(attacks.getString().Insert(attacks.getString().Length-1, ">->-")) || attacks.getString().Equals("")))
                {
                    attacks.setString(attacks.getString().Insert(attacks.getString().Length - 1, ">->-"));
                    executeAttack(attacks.getString());
                }
                else
                {
                    executeAttack(attacks.getString());
                }
            }
            if (!attacks.scanAttacks(attacks.getString()))
            {
                attacks.attacking = false;
                if(!attacks.getString().EndsWith("d"))
                attacks.reset();
                attacks.lastatk = "";
                return;
            }
        }
        //moving based on attackmovement
        if (attacks.attacking)
        {
            float divisor = 1;
            float speedtime = attacks.getAttack(attacks.getString()).speedtime - anim.getFrame();
            if (speedtime > 1)
                divisor = speedtime;
            if (direction == dir.right)
                xspeed = attacks.getAttack(attacks.getString()).xsp/divisor;
            if (direction == dir.left)
                xspeed = -attacks.getAttack(attacks.getString()).xsp/divisor;
            yspeed = attacks.getAttack(attacks.getString()).ysp;
        }
        //at the end of anim, attack ends. If a move is cancellable, check if it is autochaining but otherwise cancel out
        if (attacks.attacking && anim.animEnd() || (current.cancellable==true && attacks.getString()!= current.combo && attacks.getString()!=current.chain))
        {
            attacks.attacking = false;
            attacks.anim = "";
            xspeed = 0f;
            yspeed = 0f;
        }
    }

    private void executeAttack(string combo)
    {

        attacks.attacking = true;
        attacks.anim = attacks.getAttack(combo).anim;
        attacks.lastatk = attacks.getAttack(combo).combo;
        if (!attacks.getString().Contains("d"))
        {
            attacks.attacktimer = 0.15f;
        }
        newinput = true;
    }

    private void Animate()
    {
        if (getSpeedX() > 0)
            direction = dir.right;
        if (getSpeedX() < 0)
            direction = dir.left;
        //get attack animations
        if (!attacks.anim.Equals(""))
        {
            if (!status.getString("sprite").Equals(attacks.anim))
            {
                anim.changeSprite(attacks.anim);
                anim.setSpeed(attacks.getAttack(attacks.getString()).speed);
                return;
            }
            return;
        }
        //walking animations
        if ((getSpeedX() != 0 || getSpeedY() != 0) && zspeed == 0)
        {
            if (!status.getString("sprite").Equals("walk"))
            {
                anim.changeSprite("walk");
                anim.setSpeed(10f);
            }
        }
        else
        {
            if (zspeed != 0f)
            {
                anim.changeSprite("jump");
            }
            else
            {
                anim.changeSprite("def");
            }
        }
    }

    public void Move()
    {
        if (InputManager.getButton("left"))
        {
            setSpeedX((-status.getFloat("rspeed")*30f)*Time.deltaTime);
        }
        if (InputManager.getButton("right"))
        {
            setSpeedX((status.getFloat("rspeed")*30f) * Time.deltaTime);
        }
        if(!InputManager.getButton("right"))
        {
            if (getSpeedX() > 0)
            {
                setSpeedX(getSpeedX() - (status.getFloat("decellerate")*10f*Time.deltaTime));
                if (getSpeedX() < 0f)
                {
                    setSpeedX(0f);
                }
            }

        }
        if (!InputManager.getButton("left"))
        {
            if (getSpeedX() < 0)
            {
                setSpeedX(getSpeedX() + (status.getFloat("decellerate") * 10f * Time.deltaTime));
                if (getSpeedX() > 0)
                {
                    setSpeedX(0f);
                }
            }

        }
    }
}
