using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisiontest : MonoBehaviour
{
    private BoxCollider2D tester;
    private List<BrawlCollider> colliderlist;

    // Start is called before the first frame update
    void Start()
    {
        tester = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.getButtonPressed("right"))
        {
            Debug.Log("R");
        }
        
    }
    /*private void OnTriggerEnter2D(Collider collision)
    {

        BrawlCollider[] boxes = collision.gameObject.GetComponent<BrawlManager>().getBoxes();
        for (int i = 0; i < boxes.Length; ++i)
        {
            
            if (boxes[i].getBox().IsTouching(tester))
            {
                
            }
        }
    }*/
}
