using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCAnim : MonoBehaviour
{

    public Animator Anim;
    public PlayerState PCS;
    public PlayerShoot PCSH;

    // Start is called before the first frame update
    void Start()
    {
        //Grabbing the Player State since that'll say whether character is hurt, happy, idle etc.
        PCS = this.gameObject.GetComponentInParent<PlayerState>();
        PCSH = this.gameObject.GetComponentInParent<PlayerShoot>();
    }


    void Update()
    {
        //Grabs all the emotions from PlayerState and updates the animator variables.
        Anim.SetFloat("Boredom", PCS.Boredom);
        Anim.SetFloat("Anger", PCS.Anger);
        Anim.SetBool("Happy", PCS.Happy);
        Anim.SetBool("Hurt", PCS.Hurt);

        //Counts down the blink timer.
        Anim.SetFloat("BlinkF", Anim.GetFloat("BlinkF") - 1);

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
