using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Shortcut : MonoBehaviour
{

    public PlayerMap inputMap;

    public GameObject player;

    public GameObject noteGreen;
    public GameObject noteYellow;
    public GameObject noteBlue;
    public GameObject noteRed;

    public GameObject door1;
    public GameObject door2;
    public GameObject door3;


    public AudioClip audioGreen;
    public AudioClip audioYellow;
    public AudioClip audioBlue;
    public AudioClip audioRed;

    public GameObject animationYellow;
    public GameObject animationBlue;
    public GameObject animationRed;

    private CharacterController controller;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        inputMap = new PlayerMap();
        inputMap.Enable();

        inputMap.Player.ShortCut1.performed += PressKey1;
        inputMap.Player.ShortCut2.performed += PressKey2;
        inputMap.Player.ShortCut3.performed += PressKey3;
        inputMap.Player.ShortCut4.performed += PressKey4;
        inputMap.Player.ShortCut5.performed += PressKey5;
        inputMap.Player.ShortCut6.performed += PressKey6;


        controller = player.GetComponent<CharacterController>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    public void PressKey1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        controller.enabled = false;
        player.transform.position = new Vector3(581.9f, 25.39f, -7);
        controller.enabled = true;

        noteGreen.gameObject.SetActive(true);
        noteYellow.gameObject.SetActive(true);
        noteBlue.gameObject.SetActive(true);
        noteRed.gameObject.SetActive(true);

        animationYellow.gameObject.SetActive(true);
        animationBlue.gameObject.SetActive(true);
        animationRed.gameObject.SetActive(true);

        ShorcutPuerta(false, door1);
        ShorcutPuerta(false, door2);
        ShorcutPuerta(false, door3);

        /*Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", false);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", false);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);*/

        playerMovement.ResetImage();
        playerMovement.SetStartPosition(player.transform.position);
        playerMovement.currentIndexHotSpot = 2;

    }

    public void PressKey2(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        controller.enabled = false;
        player.transform.position = new Vector3(642, 52, -7);
        controller.enabled = true;

        noteGreen.gameObject.SetActive(false);
        noteYellow.gameObject.SetActive(true);
        noteBlue.gameObject.SetActive(true);
        noteRed.gameObject.SetActive(true);

        animationYellow.gameObject.SetActive(true);
        animationBlue.gameObject.SetActive(true);
        animationRed.gameObject.SetActive(true);

        ShorcutPuerta(true, door1);
        ShorcutPuerta(false, door2);
        ShorcutPuerta(false, door3);

        /*Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", true);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", false);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);*/

        playerMovement.ResetImage();
        playerMovement.SetStartPosition(player.transform.position);

        playerMovement.SpawnImage(noteGreen.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioGreen);

        playerMovement.currentIndexHotSpot = 2;
    }


    public void PressKey3(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        controller.enabled = false;
        player.transform.position = new Vector3(706, 49, -7);
        controller.enabled = true;

        noteGreen.gameObject.SetActive(false);
        noteYellow.gameObject.SetActive(false);
        noteBlue.gameObject.SetActive(true);
        noteRed.gameObject.SetActive(true);

        animationYellow.gameObject.SetActive(false);
        animationBlue.gameObject.SetActive(true);
        animationRed.gameObject.SetActive(true);

        ShorcutPuerta(true, door1);
        ShorcutPuerta(true, door2);
        ShorcutPuerta(false, door3);


        /*Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", true);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", true);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);*/

        playerMovement.ResetImage();
        playerMovement.SetStartPosition(player.transform.position);

        playerMovement.SpawnImage(noteGreen.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioGreen);

        playerMovement.SpawnImage(noteYellow.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioYellow);

        playerMovement.currentIndexHotSpot = 3;
    }


    public void PressKey4(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        controller.enabled = false;
        player.transform.position = new Vector3(711, 89, -7);
        controller.enabled = true;

        noteGreen.gameObject.SetActive(false);
        noteYellow.gameObject.SetActive(false);
        noteBlue.gameObject.SetActive(false);
        noteRed.gameObject.SetActive(true);

        animationYellow.gameObject.SetActive(false);
        animationBlue.gameObject.SetActive(false);
        animationRed.gameObject.SetActive(true);

        ShorcutPuerta(true, door1);
        ShorcutPuerta(true, door2);
        ShorcutPuerta(false, door3);

        /*Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", true);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", true);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);*/

        playerMovement.ResetImage();
        playerMovement.SetStartPosition(player.transform.position);

        playerMovement.SpawnImage(noteGreen.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioGreen);

        playerMovement.SpawnImage(noteYellow.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioYellow);

        playerMovement.SpawnImage(noteBlue.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioBlue);

        playerMovement.currentIndexHotSpot = 5;
    }

    public void PressKey5(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        controller.enabled = false;
        player.transform.position = new Vector3(674, 112, -7);
        controller.enabled = true;

        noteGreen.gameObject.SetActive(false);
        noteYellow.gameObject.SetActive(false);
        noteBlue.gameObject.SetActive(false);
        noteRed.gameObject.SetActive(false);

        animationYellow.gameObject.SetActive(false);
        animationBlue.gameObject.SetActive(false);
        animationRed.gameObject.SetActive(false);

        ShorcutPuerta(true, door1);
        ShorcutPuerta(true, door2);
        ShorcutPuerta(false, door3);

        /*Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", true);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", true);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);*/

        playerMovement.ResetImage();
        playerMovement.SetStartPosition(player.transform.position);

        playerMovement.SpawnImage(noteGreen.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioGreen);

        playerMovement.SpawnImage(noteYellow.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioYellow);

        playerMovement.SpawnImage(noteBlue.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioBlue);

        playerMovement.SpawnImage(noteRed.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioRed);

        playerMovement.currentIndexHotSpot = 6;
    }

    public void PressKey6(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        controller.enabled = false;
        player.transform.position = new Vector3(674, 112, -7);
        controller.enabled = true;

        noteGreen.gameObject.SetActive(false);
        noteYellow.gameObject.SetActive(false);
        noteBlue.gameObject.SetActive(false);
        noteRed.gameObject.SetActive(false);

        animationYellow.gameObject.SetActive(false);
        animationBlue.gameObject.SetActive(false);
        animationRed.gameObject.SetActive(false);

        ShorcutPuerta(true, door1);
        ShorcutPuerta(true, door2);
        ShorcutPuerta(true, door3);

        /*Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", true);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", true);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);*/

        playerMovement.ResetImage();
        playerMovement.SetStartPosition(player.transform.position);

        playerMovement.SpawnImage(noteGreen.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioGreen);

        playerMovement.SpawnImage(noteYellow.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioYellow);

        playerMovement.SpawnImage(noteBlue.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioBlue);

        playerMovement.SpawnImage(noteRed.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioRed);

        playerMovement.currentIndexHotSpot = 6;
    }


    private void ShorcutPuerta(bool open, GameObject door)
    {
        FinishLevel finishScript = door.GetComponentInChildren<FinishLevel>(true);
        finishScript.doorOpen = false;

        if (finishScript != null)
        {
            GameObject finishObject = finishScript.gameObject;

            Animator[] animators = finishObject.GetComponentsInChildren<Animator>(true);

            foreach (Animator anim in animators)
            {
                anim.SetBool("IsOpened", open);
            }
        }

        Transform puerta = door.transform.Find("PuertaObject");

        if (puerta != null)
        {
            puerta.gameObject.SetActive(!open);
        }
    }
}
