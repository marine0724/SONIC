using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_Enemy : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = LJS_Player.Instance.transform.position - transform.position;
        
    }


}
