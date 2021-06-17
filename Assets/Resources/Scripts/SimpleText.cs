using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleText : MonoBehaviour
{
    
    private string text;
    private Color textcolor;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get the wait
        GameObject.Find("textinput").GetComponent<SpriteRenderer>().enabled = GameObject.FindGameObjectWithTag("GameController").GetComponent<CutsceneScript>().getWait();
    }
    public void updateText()
    {
        //enable the new text to happen
        if (text != "new text")
        gameObject.GetComponent<Canvas>().enabled = true;
        TextMeshProUGUI[] children = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < children.Length; ++i)
        {
            children[i].text = text;
            gameObject.transform.position = pos;
            children[i].rectTransform.sizeDelta = new Vector2(200f, 100f);
            if (children[i].name == "Text")
                children[i].color = textcolor;
        }   
    }
    public void changeText(string change)
    {
        text = change;
    }
    public void changeText(string change, Color color)
    {
        text = change;
        textcolor = color;
    }
    public void changeColor(Color color)
    {
        textcolor = color;
    }
    public void changePosition(Vector2 position)
    {
        pos = new Vector3(position.x, position.y, -5f);
    }
}
