using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// æ‡¡°¿œ∂ß ±Ù∫˝¿Ã∞Ì ΩÕ¥Ÿ.
public class LHS_WNcolor : MonoBehaviour
{
    public Color startColor = Color.green;
    public Color endColor = Color.black;
    [Range(0, 10)]
    public float speed = 1;
    Renderer render;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Renderer>().material.color = Color.green;
        //GetComponent<Renderer>().material.color = Color.black;

        render.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
    }
}
