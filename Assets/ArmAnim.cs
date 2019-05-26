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
        PCS = this.gameObject.GetComponentInParent<PlayerState>();

    }

    // Update is called once per frame
    void Update()
    {
        //Grabs all the emotions from PlayerState and updates the animator variables.
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

    }

    //Called when the blink animation ends, prevents the character from blinking for a random period of time
    void BlinkEvent()
    {
        LAAnimCon.SetBool("BlinkCDBool", true);
        LAAnimCon.SetFloat("BlinkCDFloat", Random.Range(150.0f, 600.0f));
    }
}
