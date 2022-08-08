//This script is Just used for Demo1.
//this script is pretty basic...so there is not a lot of comments.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateShockWave_OnDash : MonoBehaviour 
{

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
                ShockWave1.Get(thisCamera).StartIt(Player.Instance.gameObject, Speed, MaxRadius, Amp, WS);
        }
	}


}
