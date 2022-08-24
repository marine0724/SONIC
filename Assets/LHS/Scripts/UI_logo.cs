using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_logo : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip jumpfx;


    public float createTime = 1f;
    float currentTime = 0;

    bool sound = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > createTime)
        {
            if(sound == false)
            {
                audioSource.PlayOneShot(jumpfx);
                sound = true;
            }
        }
    }
}
