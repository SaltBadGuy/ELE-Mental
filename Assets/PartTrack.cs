using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartTrack : MonoBehaviour
{

    public Animator anim;
    public Transform TargetObj;
    public Vector3 TargetPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(TargetObj.position.x, TargetObj.position.y, TargetObj.position.z);
    }
}
