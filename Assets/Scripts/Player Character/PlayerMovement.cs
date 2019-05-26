using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(1, 1);
    public Vector2 LastMovementDir;
    public Vector2 movement;
    public Vector2 NewPos;

    public float inputX;
    public float inputY;


    void Update()
    {
        //Get Axis Information
        inputX = Input.GetAxis("L_XAxis_1");
        inputY = Input.GetAxis("L_YAxis_1");

        // 4 - Movement per direction
        movement = new Vector2(
          speed.x * inputX,
          speed.y * inputY);

        NewPos = new Vector2(transform.position.x + movement.x, transform.position.y + movement.y);

    }

    void FixedUpdate()
    {
        //Move the game object
        //if (GetComponent<PCState>().Alive)
        transform.position = new Vector3(NewPos.x, NewPos.y, transform.position.z);
        
    }
}
