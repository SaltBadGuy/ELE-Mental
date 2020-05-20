using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{

    public Animator Anim;
    public PlayerState PCS;
    public PlayerMovement PCM;
    public PlayerShoot PCSH;

    // Start is called before the first frame update
    void Start()
    {
        //Grabbing the Player State since that'll say whether character is hurt, happy, idle etc.
        PCS = this.gameObject.GetComponentInParent<PlayerState>();
        PCM = this.gameObject.GetComponentInParent<PlayerMovement>();
        PCSH = this.gameObject.GetComponentInParent<PlayerShoot>();

        //THIS WHOLE BIT WILL BE FOR CHANGING STUFF THAT'S A PAIN/NOT POSSIBLE TO DO IN THE INSPECTOR

        //Setting the wrapmode of the WalkCycle to Ping Pong, so it loops cleanly both forwards and backwards.

        //Anim.WalkCycle.wrapMode = WrapMode.PingPong;

    }


    void Update()
    {
        //Grabs all the emotions from PlayerState and updates the animator variables.
        Anim.SetFloat("Boredom", PCS.AnBoredom);
        Anim.SetFloat("Anger", PCS.AnAnger);
        Anim.SetBool("Happy", PCS.AnHappy);
        Anim.SetBool("Hurt", PCS.AnHurt);

        //Counts down the blink timer.
        Anim.SetFloat("BlinkF", Anim.GetFloat("BlinkF") - 1);


        //Sets if the player character is walking and what speed they're moving at.
        Anim.SetFloat("WalkSpeedX", PCM.InputX);
        Anim.SetFloat("WalkSpeedY", PCM.InputY);

        //We take the highest value here as our effective speed between moving in the x and y position (Mathf.abs means that eg. -10 equals 10) 

        if (Mathf.Abs(PCM.InputX) > Mathf.Abs(PCM.InputY))
        {
            Anim.SetFloat("WalkSpeed", Mathf.Abs(PCM.InputX));
        }
        else if (Mathf.Abs(PCM.InputY) > Mathf.Abs(PCM.InputX))
        {
            Anim.SetFloat("WalkSpeed", Mathf.Abs(PCM.InputY));
        }
        //If both speeds are the same, we just set it to WalkSpeedX.
        else
        {
            Anim.SetFloat("WalkSpeed", Mathf.Abs(PCM.InputX));
        }

        //Now we set a bool for movement
        if (PCM.InputX == 0 && PCM.InputY == 0) {
            Anim.SetBool("Moving", false);              
        }
        else
        {
            Anim.SetBool("Moving", true);
        }

        //Simple check to see if player is attacking and if so play the relevant firing animations.
        if (PCSH.StartFiring)
        {
            Anim.SetBool("Attacking", true);
        }
        else
        {
            Anim.SetBool("Attacking", false);
        }

        //Simple Decrement Counter for Blink to come off cooldown. F means float, B means bool.
        if (Anim.GetFloat("BlinkF") > 0f)
        {
            Anim.SetFloat("BlinkF", Anim.GetFloat("BlinkF") - 1);
        }
        else
        {
            Anim.SetBool("BlinkB", false);
        }

        //Simple Decrement Counter for Look to come off cooldown. F means float, B means bool.
        if (Anim.GetFloat("LookF") > 0f)
        {
            Anim.SetFloat("LookF", Anim.GetFloat("LookF") - 1);
        }
        else
        {
            Anim.SetBool("LookB", false);
        }

        //Figuring out where the character is facing, and flipping the walking animation to match that if necessary (ie. changing it to a walking back animation if moving and facing opposite directions.

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(PCM.InputX, PCM.InputY, transform.position.z).normalized, Color.blue, 0);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(PCSH.InputX, PCSH.InputY, transform.position.z).normalized, Color.red, 0);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(-PCSH.InputX, -PCSH.InputY, transform.position.z).normalized, Color.yellow, 0);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(-PCSH.InputX, -PCSH.InputY, transform.position.z).normalized, Color.cyan, 0);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(-PCSH.InputX, -PCSH.InputY, transform.position.z).normalized, Color.magenta, 0);
        //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(-PCSH.InputX - 0.5f, -PCSH.InputY - 0.5f, transform.position.z).normalized, Color.cyan, 0);
        //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(-PCSH.InputX + 0.5f, -PCSH.InputY + 0.5f, transform.position.z).normalized, Color.magenta, 0);

        //Debug.Log(Mathf.Atan2(-PCSH.InputY - PCM.InputY, -PCSH.InputX - PCM.InputX) * 180 / Mathf.PI);
        //Debug.Log((new Vector2(PCM.InputX, PCM.InputY), new Vector2(PCSH.InputX, PCSH.InputY)));
                       
    }

    //Called when the blink animation ends, prevents the character from blinking for a random period of time
    void BlinkEvent()
    {
        Anim.SetBool("BlinkB", true);
        Anim.SetFloat("BlinkF", Random.Range(150.0f, 600.0f));
    }

    //Ditto for looking around.
    void LookEvent()
    {
        Anim.SetBool("LookB", true);
        Anim.SetFloat("LookF", Random.Range(350.0f, 1000.0f));
    }
}
