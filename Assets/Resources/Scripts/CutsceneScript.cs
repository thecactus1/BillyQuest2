using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*How to Script Cutscenes
 regular commands:
 with regular commands, enter the name of the character and then the command, e.g. billy says "a sentence"
 says: Causes a character to say something
 billy says "hey"
 faces: causes a character to face another
 billy faces test
 stopstalking: stops talking
 stops: stops all actions
 animates: causes a character to animate
 billy animates "p1"
 */
public class Timer
{
    private float value;
    private string consequence;

    public Timer()
    {
        value = 1;
        consequence = "";
    }
    public Timer(float val, string con)
    {
        value = val;
        consequence = con;
    }
    public void setValue(float val)
    {
        value = val;
    }
    public float getValue()
    {
        return value;
    }
    public string getConsequence()
    {
        return consequence;
    }
    public void setConsequence(string val)
    {
        consequence = val;
    }

}

public class Collider
{
    private string consequence;
    private Collider2D collider;
    public void setCollider(Collider2D val)
    {
        collider = val;
    }
    public Collider2D getCollider()
    {
        return collider;
    }
    public string getConsequence()
    {
        return consequence;
    }
    public void setConsequence(string val)
    {
        consequence = val;
    }
}

public class Conditional
{
    private string consequence;
    private string conditional;
    public void setConditional(string val)
    {
        conditional = val;
    }
    public string getConditional()
    {
        return conditional;
    }
    public string getConsequence()
    {
        return consequence;
    }
    public void setConsequence(string val)
    {
        consequence = val;
    }
}

public class CutsceneScript : MonoBehaviour
{
    public string script;
    private string[] instructions;
    private bool execute;
    private int layers;
    private string path;
    private int instruction;
    private float[] timers;
    private Timer[] consequencetimers;
    private Conditional[] conditionaltimers;
    private bool waitforinput = false;
    private int choice;

    // Start is called before the first frame update
    void Start()
    {
        layers = 0;
        consequencetimers = new Timer[0];
        timers = new float[0];
        instruction = 0;
        path = "Assets/Resources/Cutscene/";
        instructions = System.IO.File.ReadAllLines(path + script + ".txt");
        execute = true;
    }
    public bool getWait()
    {
        return waitforinput;
    }
    private void Update()
    {
        //get playing input
        if (InputManager.getButtonPressed("Jump"))
        {
            waitforinput = false;
        }   
        //execute cutscene instructions
        if (checkTimer() == true && execute == true && waitforinput == false)
        {
            ExecuteInstructions();
        }
        //remove timers
        for(int i = 0; i < timers.Length; ++i)
        {
            timers[i] -= Time.deltaTime;
            if (timers[i] <= 0)
            {
                timers = ArrayTools.RemoveAt<float>(timers, i);
            }
        }
        //timers with consequences
        for (int i = 0; i<consequencetimers.Length; ++i)
        {
            //execute all timers with consequences
            consequencetimers[i].setValue(consequencetimers[i].getValue() - Time.deltaTime);
            if (consequencetimers[i].getValue() <= 0)
            {
                
                //create a new array of consequencetimers without the one that was triggered
                Execute(consequencetimers[i].getConsequence());
                Timer[] newcon = new Timer[0];
                for (int j = 0; i < consequencetimers.Length; ++i)
                {
                    if (consequencetimers[i] != consequencetimers[j])
                    {
                        newcon = ArrayTools.Push(newcon, consequencetimers[j]);
                    }
                }
                consequencetimers = newcon;
            }

        }
        //timers with consequences
        for (int i = 0; i < consequencetimers.Length; ++i)
        {
            //execute all timers with consequences
            consequencetimers[i].setValue(consequencetimers[i].getValue() - Time.deltaTime);
            if (Ifchecker(conditionaltimers[i].getConditional()))
            {

                //create a new array of consequencetimers without the one that was triggered
                Execute(conditionaltimers[i].getConsequence());
                Conditional[] newcon = new Conditional[0];
                for (int j = 0; i < conditionaltimers.Length; ++i)
                {
                    if (conditionaltimers[i] != conditionaltimers[j])
                    {
                        newcon = ArrayTools.Push(newcon, conditionaltimers[j]);
                    }
                }
                conditionaltimers = newcon;
            }

        }
    }
    //add a timer
    public void addTimer(float timer)
    {
        timers = ArrayTools.Push<float>(timers, timer);
    }
    public void addTimer(float timer, string consequence)
    {
        Timer newcon = new Timer(timer, consequence);
        consequencetimers = ArrayTools.Push<Timer>(consequencetimers, newcon);
    }
    //check if no timers remain
    public bool checkTimer()
    {
        if(timers.Length == 0)
        {
            return true;
        }
        return false;
    }
    public void setScript(string sc)
    {
        script = sc;
        instructions = System.IO.File.ReadAllLines(path + script + ".txt");
    }
    public void setScript(string sc, string p)
    {
        script = sc;
        path = p;
        instructions = System.IO.File.ReadAllLines(path + script + ".txt");
    }
    public void setPath(string sc)
    {
        path = sc;
        instructions = System.IO.File.ReadAllLines(path + script + ".txt");
    }
    public void ExecuteInstructions()
    {
        if(instructions.Length>instruction)
        Execute(instructions[instruction]);
        ++instruction;
    }
    public void ExecuteInstructions(int startpoint)
    {
        instruction = startpoint;
        ExecuteInstructions();
    }
    //set a new script and execute
    public void ExecuteInstructions(string script)
    {
        setScript(script);
        ExecuteInstructions();
    }
    //remove negatives from a script for basically no reason (idiot proofing)
    public string removeNegatives(string original)
    {
        string ret = original.Replace("dont", "not");
        ret = ret.Replace("doesnt", "not");
        return ret;
    }
    //core of the string parser
    bool Execute(string instructions)
    {
        if (instructions.Contains("}"))
        {
            if (layers > 0)
                layers -= 1;
            return true;
        }
        //if the string is empty, proceed to next step
        if (instructions.Length == 0 || layers != 0)
            return true;
        //look for a timer command
        if (instructions.StartsWith("in"))
        {
            string[] fors = instructions.Split(' ');
            float timerval = float.Parse(fors[1]);
            instructions = instructions.Replace("in " + fors[1] + " ", "");
            addTimer(timerval, instructions);
            return true;
        }
        if (instructions.StartsWith("goto"))
        {
            string[] fors = instructions.Split(' ');
            string flag = (fors[1]);
            if (int.TryParse(flag, out instruction))
                instruction = int.Parse(flag)-2;
            else
            {
                string[] scanner = this.instructions;
                for(int i = 0; i < scanner.Length; ++i)
                {
                     if(scanner[i].Equals("<" + flag.Trim() + ">"))
                    {
                        instruction = i;
                        break;
                    }
                }
            }
            return true;
        }
        //allow for code commenting
        if (instructions.StartsWith("//") || instructions.StartsWith("<"))
            return true;
        //replace negatives with "not" 
        instructions = removeNegatives(instructions);
        //qualify if statements
        if (instructions.ToLower().StartsWith("if"))
        {
            instructions = instructions.Replace('{', ' ');
            if(!Ifchecker(instructions.Substring(3)))
                layers += 1;
            return true;
        }
        if (instructions.Contains("{"))
        {
            return true;
        }
        //qualify a wait statement
        if (instructions.ToLower().StartsWith("wait"))
        {
            string val = instructions.Substring(5);
            if(instructions.Contains("input"))
            {
                waitforinput = true;
                return true;
            }
            addTimer(float.Parse(val));
            return true;
        }
        //restart the instructions
        if (instructions.ToLower().StartsWith("restart"))
        {
            ExecuteInstructions(0);
        }
        //qualify destroy statements
        if (instructions.ToLower().StartsWith("destroy"))
        {
            GameObject guy = GameObject.Find(instructions.Substring(7).Trim().ToLower());
            Destroy(guy);
            return true;
        }
        string[] instructionsexec = instructions.Split('"');
        //targets and command
        string[] targetandexec = instructionsexec[0].Split(' ');
        //breaking up the script executions so that even if there are no "" there can be storage of the extra data
        if (instructionsexec.Length == 1) {
            if (targetandexec.Length <= 3)
            {
                string og = instructionsexec[0];
                instructionsexec = new string[2];
                instructionsexec[0] = og;
                if(targetandexec.Length == 3)
                instructionsexec[1] = targetandexec[2];
                if (targetandexec.Length == 2)
                    instructionsexec[1] = targetandexec[1];
            }
            else
            {
                string og = instructionsexec[0];
                instructionsexec = new string[3];
                instructionsexec[0] = og;
                if (instructionsexec.Length > 2)
                    instructionsexec[1] = targetandexec[2];
                else
                    instructionsexec[1] = targetandexec[1];
                instructionsexec[2] = instructionsexec[0].Substring(instructionsexec[0].IndexOf(targetandexec[2], 0) + targetandexec[2].Length);
            }
            
        }
            
        string target = targetandexec[0];
        string exec = targetandexec[1];
        string extra = "";
        //getting extra data from end of string to parse
        if (instructionsexec.Length > 2)
        {
            extra = instructionsexec[2].ToLower();
        }
        target = target.ToLower();
        switch (exec.ToLower())
        {
            case "says":
                if (extra.Contains("not animated"))
                    Say(target, instructionsexec[1]);
                else
                {
                    Say(target, instructionsexec[1]);
                    SpriteChanger(target, "talk", 10);
                }
                if (extra.Contains("for"))
                {
                    string time = extra.Substring(extra.IndexOf("for")+4);
                    float timer = float.Parse(time);
                    addTimer(timer, target + " stopstalking");
                }
                break;
            case "animates":
                if (extra == "")
                    SpriteChanger(target, instructionsexec[1]);
                else
                {
                    string[] ex = extra.Split(' ');
                    float speed = float.Parse(ex[2]);
                    SpriteChanger(target, instructionsexec[1], speed);
                }
                break;
            case "faces":
                Face(target, instructionsexec[1]);
                break;
            case "walks":
                bool turn = true;
                if (instructions.ToLower().Contains("not turn"))
                    turn = false;
                
                if (!instructions.ToLower().Contains("not animated"))
                    SpriteChanger(target, "walk", 5);
                if (extra == "")
                {
                    Walks(target, instructionsexec[1], 1f ,turn);
                }
                else
                {
                    string[] ex = extra.Split(' ');
                    float speed = 1f;
                    speed = float.Parse(ex[2]);
                    Walks(target, instructionsexec[1], speed, turn);
                    if (!instructions.ToLower().Contains("not animated"))
                        SpriteChanger(target, "walk", 5*speed);
                }
                break;
            case "stopstalking":
                Destroy(GameObject.Find(target + "talk"));
                SpriteChanger(target, "def");
                break;
            case "stops":
                Walks(target, "left", 0f, false);
                Destroy(GameObject.Find(target + "talk"));
                SpriteChanger(target, "def");
                break;
            default:
                Character targ = GameObject.Find(target).GetComponent<Character>();
                targ.status.setValueParse(exec.ToLower(), instructionsexec[1]);
                break;
        }
        return true;

    }
    bool Ifchecker(string check)
    {
        bool result = false;
        bool opposite = false;
        string[] checks = check.Split(' ');
        string target = checks[0];
        string var = checks[1];
        string val = "";
        if (check.Contains("is not"))
        {
            val = checks[4];
            opposite = true;
        }
        else
            val = checks[3];
        GameObject guy = GameObject.Find(target);
        switch (var)
        {
            case "choice":
                if (choice == int.Parse(val))
                    result = true;
                break;       
            case "speed":
                AnimationScript anim = guy.GetComponent<AnimationScript>();
                float value = float.Parse(val);
                if (anim.getSpeed() == value)
                {
                    result = true;
                }
                break;
            default:
                //get value in the valueparser
                Character charsheet = guy.GetComponent<Character>();
                result = (charsheet.status.getValueParse(var).Trim().CompareTo(val.Trim()) == 0);
                break;
        }
        if (opposite == false)
            return result;
        else
            return !result;
    }
    void SpriteChanger(string target, string sprite)
    {
        GameObject guy = GameObject.Find(target);
        AnimationScript anim = guy.GetComponent<AnimationScript>();
        anim.changeSprite(sprite);
    }
    void SpriteChanger(string target, string sprite, float speed)
    {
        GameObject guy = GameObject.Find(target);
        AnimationScript anim = guy.GetComponent<AnimationScript>();
        anim.changeSprite(sprite);
        anim.setSpeed(speed);
    }
    void Walks(string target, string direction, float speed, bool turn)
    {
        
        GameObject guy = GameObject.Find(target);
        Character character = guy.GetComponent<Character>();
        if (GameObject.Find(direction) != null && turn)
        {
            character.setDirection(getDirection(target, direction));
        }
        if (direction == "left")
        {
            if (turn)
                character.setDirection(Character.dir.left);
            character.setSpeedX(-speed);


        }
        else if (direction == "down")
        {
            if (turn)
                character.setDirection(Character.dir.down);
            character.setSpeedY(-speed);
        }
        else if (direction == "up")
        {
            if (turn)
                character.setDirection(Character.dir.up);
            character.setSpeedY(speed);
        }
        else if (direction == "right")
        {
            if (turn)
                character.setDirection(Character.dir.right);

        }
        else
        {
            if (turn)
                character.setDirection(getDirection(target, direction));

            switch (getDirection(target, direction))
            {
                case Character.dir.right:
                    character.setSpeedX(speed);
                    break;
                case Character.dir.left:

                    character.setSpeedX(-speed);
                    break;
                case Character.dir.up:
                    character.setSpeedY(speed);
                    break;
                case Character.dir.down:
                    character.setSpeedY(-speed);
                    break;
                default:
                    break;
            }
        }

    }
    void setVariable(string target, string variable, string inp)
    {
        //parse the variable and target
        GameObject guy = GameObject.Find(target);
        Character character = guy.GetComponent<Character>();
        character.status.setValueParse(variable, inp);
    }
    void Face(string target, string target2)
    {
        GameObject guy = GameObject.Find(target);
        Character character = guy.GetComponent<Character>();
        if(target2 == "left")
        {
            character.setDirection(Character.dir.left);
        }
        else if (target2 == "down")
        {
            character.setDirection(Character.dir.down);
        }
        else if (target2 == "up")
        {
            character.setDirection(Character.dir.up);
        }
        else if (target2 == "right")
        {
            character.setDirection(Character.dir.right);
        }
        else
        {
            character.setDirection(getDirection(target, target2));
        }
    }
    Character.dir getDirection(string target, string target2)
    {
        GameObject guy = GameObject.Find(target);
        GameObject guy2 = GameObject.Find(target2);
        Vector2 guypos = guy.transform.position;
        Vector2 guy2pos = guy2.transform.position;
        bool lr = false, ud = false;
        float xdist = 0f, ydist = 0f;
        Character.dir Direction = Character.dir.left;
        if (guypos.x > guy2pos.x)
        {
            lr = false;
            xdist = Mathf.Abs(guypos.x - guy2pos.x);
        }
        if (guypos.x < guy2pos.x)
        {
            lr = true;
            xdist = Mathf.Abs(guy2pos.x - guypos.x);
        }
        if (guypos.y > guy2pos.y)
        {
            ud = true;
            ydist = Mathf.Abs(guypos.y - guy2pos.y);
        }
        if (guypos.y < guy2pos.y)
        {
            ud = false;
            ydist = Mathf.Abs(guy2pos.y - guypos.y);
        }
        if (xdist > ydist)
        {
            if (lr == true)
                Direction = Character.dir.right;
            else
                Direction = Character.dir.left;
        }
        else
        {
            if (ud == true)
                Direction = Character.dir.down;
            else
                Direction = Character.dir.up;
        }
        return Direction;
    }
    //gets target to say text
    void Say(string target, string line)
    {
        GameObject sayer = GameObject.Find(target);
        Character character = sayer.GetComponent<Character>();
        Color changer = character.textcolor;
        if (GameObject.Find(target + "talk") != null)
            Destroy(GameObject.Find(target + "talk"));
        //load up prefab
        GameObject texter = GameObject.Instantiate(Resources.Load<GameObject>("prefabs/say"));
        //set up name and text
        texter.name = target + "talk";
        SimpleText txt = texter.GetComponent<SimpleText>();
        txt.changeText(line, changer);
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector3 pos = new Vector3(sayer.transform.position.x+0.5f, sayer.transform.position.y+2.2f, -5f);
        txt.changePosition(pos);
        txt.updateText();
    }
}