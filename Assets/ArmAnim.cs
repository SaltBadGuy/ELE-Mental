using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAnim : MonoBehaviour
{
    public Animator LAAnimCon;
    public Animator RAAnimCon;
    public PlayerState PCS;



    // Start is called before the first frame update
    void Start()
    {
        //Grabbing the Player State since that'll say whether character is hurt, happy, idle etc.
        PCS = this.gameObject.GetComponentInParent<PlayerState>();

    }

    // Update is called once per frame
    void Update()
    {
        //Grabs all the emotions from PlayerState and updates the animator variables. Means if we want the arms to do something when the character is idle, happy etc. we can do that.
        LAAnimCon.SetFloat("Boredom", PCS.Boredom);
        LAAnimCon.SetFloat("Anger", PCS.Anger);
        LAAnimCon.SetBool("Happy", PCS.Happy);
        LAAnimCon.SetBool("Hurt", PCS.Hurt);

        //Counts down the blink timer.
        LAAnimCon.SetFloat("BlinkCDFloat", LAAnimCon.GetFloat("BlinkCDFloat") - 1);

        if (LAAnimCon.GetFloat("BlinkCDFloat") > 0f)
        {
            LAAnimCon.SetFloat("BlinkCDFloat", LAAnimCon.GetFloat("BlinkCDFloat") - 1);
        }
        else
        {
            LAAnimCon.SetBool("BlinkCDBool", false);
        }

        //Ditto for right arm.
        RAAnimCon.SetFloat("Boredom", PCS.Boredom);
        RAAnimCon.SetFloat("Anger", PCS.Anger);
        RAAnimCon.SetBool("Happy", PCS.Happy);
        RAAnimCon.SetBool("Hurt", PCS.Hurt);

        //Counts down the blink timer.
        RAAnimCon.SetFloat("BlinkCDFloat", RAAnimCon.GetFloat("BlinkCDFloat") - 1);

        if (RAAnimCon.GetFloat("BlinkCDFloat") > 0f)
        {
            RAAnimCon.SetFloat("BlinkCDFloat", RAAnimCon.GetFloat("BlinkCDFloat") - 1);
        }
        else
        {
            RAAnimCon.SetBool("BlinkCDBool", false);
        }

    }

    //Called when the blink animation ends, prevents the character from blinking for a random period of time (Right now this is just here for reference. Likely the arms will be involved in an animation we want to happen semi-randomly.)
    void BlinkEvent()
    {
        LAAnimCon.SetBool("BlinkCDBool", true);
        LAAnimCon.SetFloat("BlinkCDFloat", Random.Range(150.0f, 600.0f));
    }
}
