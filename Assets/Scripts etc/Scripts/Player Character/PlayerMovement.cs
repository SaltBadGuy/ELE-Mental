using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 speed;
    public float speedMulti;
    public Vector2 LastMovementDir;
    public Vector2 movement;
    public Vector2 TestPos;
    public Vector2 NewPos;

    public float InputX;
    public float InputY;

    //Controls the amount of after images
    public int AfterImageNum;

    public LayerMask MoveLMask;

    public PlayerAnim PCA;
    public PlayerShoot PSH;
    public PlayerState PS;

    //Referring to the 2 different trees, VisRot is stuff affected by rotation and so on such as the character body, while StaHit is things like the character's hitbox where we don't want it to rotate necessarily.
    public GameObject VisRot;
    public GameObject StaHit;
    //Grab the leg object so we can rotate it for movement later
    public GameObject Legs;
    //And we'll grab the movement hitbox too.
    public GameObject HBox;

    //Accepts a string for moving about. Currently supports keyboard and joysticks.
    public string MoveControl;
    
    void Update()
    {
        ChargeMoveBuff();

        GetMoveInput();
    }

    void FixedUpdate()
    {
        Movement();

        Rotation();
    }

    void ChargeMoveBuff()
    {
        //Figure out how much faster the player character should be moving based on how much Charge it has. Also controls how many after images of the character appears.
        if (PS.Charge > 99)
        {
            speedMulti = 2f;
            AfterImageNum = 4;
        }
        else if (PS.Charge > 74)
        {
            speedMulti = 1.75f;
            AfterImageNum = 4;
        }
        else if (PS.Charge > 49)
        {
            speedMulti = 1.5f;
            AfterImageNum = 2;
        }
        else if (PS.Charge > 24)
        {
            speedMulti = 1.25f;
            AfterImageNum = 1;
        }
    }

    void GetMoveInput()
    {
        //Get Axis Information
        if (MoveControl == "Controller")
        {
            InputX = Input.GetAxis("L_XAxis_1");
            InputY = Input.GetAxis("L_YAxis_1");
        }
        else if (MoveControl == "Keyboard")
        {
            InputX = Input.GetAxis("Move_XAxis");
            InputY = Input.GetAxis("Move_YAxis");
        }
        else
        {
            Debug.Log("something weird is being used for moving");
        }

        //Input is -1 to 1, so multiply it by our base speed value plus our speed multiplier (currently only effected by charge)
        movement = new Vector2(
          speed.x * InputX * speedMulti,
          speed.y * InputY * speedMulti);

        //we set a new potential position (ie. where the character would move to if there was no obstacles) The actual movement won't happen just yet, it gets checked in Movement().
        NewPos = new Vector2(transform.position.x + movement.x, transform.position.y + movement.y);
    }

    void Movement()
    {
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

                    //TestedNewPos.x = (hit.point.x + (box.size.x));

                    TestedNewPos.x = (hit.point.x);
                }
                else if (NewPos.x > hit.point.x)
                {
                    TestedNewPos.x = (hit.point.x);
                }

                //For the y value...

                //If the character is moving up into the wall...
                if (NewPos.y < hit.point.y)
                {
                    //We'll shove the character away enough from the wall that the boxcollider won't hit it.
                    TestedNewPos.y = (hit.point.y);
                }
                //Or if the character is moving down into a wall...
                else if (NewPos.y > hit.point.y)
                {
                    TestedNewPos.y = (hit.point.y);
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

    void Rotation()
    {
        //We figure out which direction the legs should be going based on how our character attempts to move.
        Vector2 look = new Vector2(InputX, InputY);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;

        //We finally rotate the legs. If there's no input at all ie InputX and InputY equals 0, we just don't bother changing the rotation.
        if (InputX != 0 || InputY != 0)
        {
            Legs.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Colliding with " + col.gameObject.name);
        
    }

}

//transform.position = new Vector3(NewPos.x, NewPos.y, transform.position.z);