using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_DashEffect : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        transform.forward = player.transform.forward;
    }
}
