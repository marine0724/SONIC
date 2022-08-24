//This script is Just used for Demo1.
//this script is pretty basic...so there is not a lot of comments.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LJS_CreateShockWave_OnDash : MonoBehaviour 
{
    public static LJS_CreateShockWave_OnDash Instance;

    private void Awake()
    {
        Instance = this;
    }


    //values
    public float MaxRadius;
    public float Speed;
    public float Amp;
    public float WS;



    public Camera thisCamera;

    //setting variables

	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ShockShock();
        }
	}

    public void ShockShock()
    {
        LJS_ShockWave1.Get(thisCamera).StartIt(LJS_Player.Instance.gameObject, Speed, MaxRadius, Amp, WS);

    }


}
