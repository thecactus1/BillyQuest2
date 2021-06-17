    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directional
{
    public string name;
    public bool pressed;
    public bool held;
    public bool released;
    public bool reset;

    public Directional(string n)
    {
        pressed = false;
        held = false;
        released = false;
        reset = true;
        name = n;
    }
}

public static class InputManager
{
    private static string[] buttons = { "Grab", "Taunt", "Light", "Heavy", "Jump" };
    private static Directional[] axis = { new Directional("left"), new Directional("right"), new Directional("up"), new Directional("down") };
    public static bool doubletap;

    // Update is called once per frame
    public static void UpdateAllAxis()
    {
        Directional[] newAx = new Directional[4];
        for(int i = 0; i < newAx.Length; ++i)
        {
            newAx[i] = updateAxis(axis[i]);
        }
        axis = newAx;
    }
    private static Directional updateAxis(Directional axis)
    {
        //get the axes to update
        string axisname = "Horizontal";
        Directional newAxis = axis;
        newAxis.released = false;
        newAxis.pressed = false;
        //find if the axis is vertical
        if(newAxis.name == "up" || newAxis.name == "down")
        {
            axisname = "Vertical";
        }
        //negative axes
        if(newAxis.name == "left" || newAxis.name == "down")
        {
            if (Input.GetAxis(axisname) < -0.3f)
            {
                if (newAxis.reset == true)
                {
                    newAxis.pressed = true;
                    newAxis.reset = false;
                }
                newAxis.held = true;
            }
            else 
            {
                newAxis.held = false;
                if (newAxis.reset == false)
                {
                    newAxis.released = true;
                    newAxis.reset = true;
                }
            }
            
        }
        //get the positive axes
        if (newAxis.name == "right" || newAxis.name == "up")
        {
            if (Input.GetAxis(axisname) > 0.3f)
            {
                if (newAxis.reset == true)
                {
                    newAxis.pressed = true;
                    newAxis.reset = false;
                }
                newAxis.held = true;
            }
            else
            {
                newAxis.held = false;
                if (newAxis.reset == false)
                {
                    newAxis.released = true;
                    newAxis.reset = true;
                }
            }

        }
        return newAxis;
    }
    //check if a button is held
    public static bool getButton(string button)
    {
        for (int i = 0; i <axis.Length; ++i)
        {
            if(axis[i].name == button)
            {
                return axis[i].held;
            }
        }
        if (Input.GetButton(button))
            return true;
        return false;
    }
    //Get buttonPressed
    public static bool getButtonPressed(string button)
    {
        for (int i = 0; i < axis.Length; ++i)
        {
            if (axis[i].name == button)
            {
                return axis[i].pressed;
            }
        }
        if (Input.GetButtonDown(button))
            return true;
        return false;
    }
    //get ButtonReleased
    public static bool getButtonReleased(string button)
    {
        for (int i = 0; i < axis.Length; ++i)
        {
            if (axis[i].name == button)
            {
                return axis[i].released;
            }
        }
        if (Input.GetButtonUp(button))
            return true;
        return false;
    }
}

public class InputUpdater : MonoBehaviour
{

    private float timer;

    private void Start()
    {
        timer = 0f;
    }
    private void Update()
    {
        InputManager.UpdateAllAxis();
        if (timer> 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
                timer = 0;
        }
        if (timer < 0)
        {
            timer += Time.deltaTime;
            if (timer > 0)
                timer = 0;
        }
        if (timer == 0)
            InputManager.doubletap = false;
        if (InputManager.getButtonPressed("right") && timer > 0f)
        {
            InputManager.doubletap = true;
        }
        if (InputManager.getButtonPressed("right") && InputManager.doubletap == false)
        {
            timer = 0.5f;
        }
        if (InputManager.getButtonPressed("left") && timer < 0f)
        {
            InputManager.doubletap = true;
        }
        if (InputManager.getButtonPressed("left") && InputManager.doubletap == false)
        {
            timer = -0.5f;
        }
    }
}