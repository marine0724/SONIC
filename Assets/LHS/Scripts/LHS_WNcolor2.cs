using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_WNcolor2 : MonoBehaviour
{
    Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        render.material.color = Color.black;
    }
}
