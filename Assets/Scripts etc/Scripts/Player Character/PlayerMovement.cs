using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(1, 1);
    public Vector2 LastMovementDir;
    public Vector2 movement;
    public Vector2 TestPos;
    public Vector2 NewPos;

    public float inputX;
    public float inputY;

    public LayerMask MoveLMask;

    //Referring to the 2 different trees, VisRot is stuff affected by rotation and so on such as the character body, while StaHit is things like the character's hitbox where we don't want it to rotate necessarily.
    public GameObject VisRot;
    public GameObject StaHit;
    //And we'll grab the movement hitbox too.
    public GameObject HBox;

    //Either uses the standard 360 controller set up if true, will use keyboard and mouse if false
    public bool Controller;


    void Update()
    {
        //Get Axis Information
        if (Controller)
        {
            inputX = Input.GetAxis("L_XAxis_1");
            inputY = Input.GetAxis("L_YAxis_1");
        }
        else
        {
            inputX = Input.GetAxis("Move_XAxis");
            inputY = Input.GetAxis("Move_YAxis");
        }

        //Input is -1 to 1, so multiply it by some number
        movement = new Vector2(
          speed.x * inputX,
          speed.y * inputY);

        //If the position checks out, we can see this so that the game moves the character to its new pos.
        NewPos = new Vector2(transform.position.x + movement.x, transform.position.y + movement.y);

    }

    void FixedUpdate()
    {
        //Moving the game object...
        //Defaults to no movement. 
        Vector3 TestedNewPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        float dis = Vector2.Distance(transform.position, NewPos);

        //Grabbing the box collider of the game object
        BoxCollider2D box = HBox.GetComponent<BoxCollider2D>();

        // Casts a ray in the direction the character wants to move to, checks if there'll be any collision at the end point
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(movement.x, movement.y, transform.position.z), Color.green, 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, NewPos, dis, MoveLMask);
        

        //Checks if the ray actually hits anything at all
        if (hit.collider != null)
        {
            // If it hits a wall, then we need to adjust the movement so that the character won't go inside the wall.
            if (hit.collider.tag == "Wall")
            {
                Debug.Log(gameObject.name + "attempted to move into a wall, correcting. Hit point is " + hit.point.x + ", " + hit.point.y);

                //For the x value...

                //If we're moving left into the wall...
                if (NewPos.x < hit.point.x)
                {
                    //We'll shove the character away enough from the wall that the boxcollider won't hit it.

                    TestedNewPos.x = (hit.point.x + (box.size.x));
                }
                else if (NewPos.x > hit.point.x)
                {
                    TestedNewPos.x = (hit.point.x - (box.size.x));
                }

                //For the y value...

                //If the character is moving up into the wall...
                if (NewPos.y < hit.point.y)
                {
                    //We'll shove the character away enough from the wall that the boxcollider won't hit it.
                    TestedNewPos.y = (hit.point.y + (box.size.y));
                }
                //Or if the character is moving down into a wall...
                else if (NewPos.y > hit.point.y)
                {
                    TestedNewPos.y = (hit.point.y - (box.size.y));
                }
                //If neither of those were true, there's no change (ie. TestedNewPos.x == hit.point.X) and it doesn't matter.
                




                //TestedNewPos = new Vector3(TestedNewPos.x - Vector2.Distance(TestedNewPos, hit.point), TestedNewPos.y, TestedNewPos.z);
                //TestedNewPos =  new Vector3(TestedNewPos.x - hit.distance, TestedNewPos.y - hit.distance, TestedNewPos.z);
                //Vector2 Adj = new Vector2(hit.collider.ClosestPoint, transform.position.y);
            }
        }
        //If it hasn't hit anything, then the movement is probably fine.
        else
        {
            TestedNewPos = new Vector3(NewPos.x, NewPos.y, transform.position.z);
        }

        //Finally, set the character's position.
        transform.position = new Vector3(TestedNewPos.x, TestedNewPos.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Colliding with " + col.gameObject.name);
        
    }

}

//transform.position = new Vector3(NewPos.x, NewPos.y, transform.position.z);