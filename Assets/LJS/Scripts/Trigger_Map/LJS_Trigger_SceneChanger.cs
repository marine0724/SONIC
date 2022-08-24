using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LJS_Trigger_SceneChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == LJS_Player.Instance.gameObject)
        { 
                SceneManager.LoadScene("LHS_Scene");
        }
    }
}
