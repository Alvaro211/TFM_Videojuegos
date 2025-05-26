using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool helpControls = true;
    public bool canMove = true;
    public bool playerMovePlatform = false;
    public bool defeatBoss = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
   
    public void ChangeHelpControls()
    {
        helpControls = !helpControls;
    }
}

