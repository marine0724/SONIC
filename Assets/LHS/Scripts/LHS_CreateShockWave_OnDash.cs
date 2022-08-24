//This script is Just used for Demo1.
//this script is pretty basic...so there is not a lot of comments.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LHS_CreateShockWave_OnDash : MonoBehaviour 
{
    public static LHS_CreateShockWave_OnDash Instance;

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
        LHS_ShockWave1.Get(thisCamera).StartIt(Player.Instance.gameObject, Speed, MaxRadius, Amp, WS);

    }


}
