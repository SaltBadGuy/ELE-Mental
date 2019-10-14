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
        FaceAnimCon.SetFloat("BlinkF", FaceAnimCon.GetFloat("BlinkF") - 1);

        //Simple Decrement Counter for Blink to come off cooldown. F means float, B means bool.
        if (FaceAnimCon.GetFloat("BlinkF") > 0f)
        {
            FaceAnimCon.SetFloat("BlinkF", FaceAnimCon.GetFloat("BlinkF") - 1);
        }
        else
        {
            FaceAnimCon.SetBool("BlinkB", false);
        }

        //Simple Decrement Counter for Look to come off cooldown. F means float, B means bool.
        if (FaceAnimCon.GetFloat("LookF") > 0f)
        {
            FaceAnimCon.SetFloat("LookF", FaceAnimCon.GetFloat("LookF") - 1);
        }
        else
        {
            FaceAnimCon.SetBool("LookB", false);
        }
    }

    //Called when the blink animation ends, prevents the character from blinking for a random period of time
    void BlinkEvent()
    {
        FaceAnimCon.SetBool("BlinkB", true);
        FaceAnimCon.SetFloat("BlinkF", Random.Range(150.0f, 600.0f));
    }

    void LookEvent()
    {
        FaceAnimCon.SetBool("LookB", true);
        FaceAnimCon.SetFloat("LookF", Random.Range(350.0f, 1000.0f));
    }
}
