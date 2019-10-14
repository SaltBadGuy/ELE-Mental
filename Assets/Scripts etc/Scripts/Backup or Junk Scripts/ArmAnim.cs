using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAnim : MonoBehaviour
{
    //public Animator Anim;
    //public Animator Anim;
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

    // Update is called once per frame
    void Update()
    {
        //Grabs all the emotions from PlayerState and updates the animator variables. Means if we want the arms to do something when the character is idle, happy etc. we can do that.
        Anim.SetFloat("Boredom", PCS.Boredom);
        Anim.SetFloat("Anger", PCS.Anger);
        Anim.SetBool("Happy", PCS.Happy);
        Anim.SetBool("Hurt", PCS.Hurt);

        if (PCSH.StartFiring)
        {

        }

    }

    //Called when the blink animation ends, prevents the character from blinking for a random period of time (Right now this is just here for reference. Likely the arms will be involved in an animation we want to happen semi-randomly.)
    void BlinkEvent()
    {
        Anim.SetBool("BlinkCDBool", true);
        Anim.SetFloat("BlinkCDFloat", Random.Range(150.0f, 600.0f));
    }
}
