using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEffects : MonoBehaviour
{

    public float OffsetRan;
    public Vector3 OffsetVec;

    // Start is called before the first frame update
    void Start()
    {
        //We generate a random offset which will end up producing a vector that will make the particle move left/right slightly parallel to the direction it's going.
        OffsetRan = Random.Range(-0.2f, 0.2f);

        if (OffsetRan > 0)
        {
            OffsetVec = (transform.up * OffsetRan);
        }
        else
        {
            OffsetVec = (transform.up * OffsetRan);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Moves the particle both back and with a left/right offset relative/parallel to the direction it's going
        transform.position += ((-transform.right * 0.1f) + OffsetVec) * Time.deltaTime;
     }
}
