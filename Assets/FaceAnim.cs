using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnim : MonoBehaviour
{

   public Animator FaceAnimCon;
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
        FaceAnimCon.SetFloat("Boredom", PCS.Boredom);
        FaceAnimCon.SetFloat("Anger", PCS.Anger);
        FaceAnimCon.SetBool("Happy", PCS.Happy);
        FaceAnimCon.SetBool("Hurt", PCS.Hurt);
        
        //Counts down the blink timer.
        FaceAnimCon.SetFloat("BlinkCDFloat", FaceAnimCon.GetFloat("BlinkCDFloat") - 1);

        if (FaceAnimCon.GetFloat("BlinkCDFloat") > 0f)
        {
            FaceAnimCon.SetFloat("BlinkCDFloat", FaceAnimCon.GetFloat("BlinkCDFloat") - 1);
        }
        else
        {
            FaceAnimCon.SetBool("BlinkCDBool", false);
        }

        if (FaceAnimCon.GetFloat("LookAroundCDFloat") > 0f)
        {
            FaceAnimCon.SetFloat("LookAroundCDFloat", FaceAnimCon.GetFloat("LookAroundCDFloat") - 1);
        }
        else
        {
            FaceAnimCon.SetBool("LookAroundCDBool", false);
        }
    }

    //Called when the blink animation ends, prevents the character from blinking for a random period of time
    void BlinkEvent()
    {
        FaceAnimCon.SetBool("BlinkCDBool", true);
        FaceAnimCon.SetFloat("BlinkCDFloat", Random.Range(150.0f, 600.0f));
    }

    void LookAroundEvent()
    {
        FaceAnimCon.SetBool("LookAroundCDBool", true);
        FaceAnimCon.SetFloat("LookAroundCDFloat", Random.Range(350.0f, 1000.0f));
    }
}
