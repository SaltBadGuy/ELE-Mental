﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEndDes : MonoBehaviour
{

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
