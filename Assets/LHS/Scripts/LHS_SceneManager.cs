using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ¾À ÀüÈ¯
public class LHS_SceneManager : MonoBehaviour
{
    public static LHS_SceneManager Instance;

    float currentTime = 0;
    float createTime = 4.0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LHS_Start1")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SceneManager.LoadScene("LHS_Start2");
            }
        }

        Ending();
    }

    public bool isDieOn = false;


    //¾À ÀüÈ¯ ÇÔ¼ö
    public void BattelScene()
    {
        SceneManager.LoadScene("SonicSample");

        if (SceneManager.GetActiveScene().name == "LHS_Start2")
        {
            Destroy(LHS_DestroyBG.Instance.gameObject);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Ending()
    {
        if (isDieOn)
        { 
            currentTime += Time.deltaTime;

            if(currentTime > createTime)
            {
                SceneManager.LoadScene(4);

                currentTime = 0;
            }       
        }
    }
}
