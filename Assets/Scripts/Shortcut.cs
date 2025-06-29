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


        controller = player.GetComponent<CharacterController>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    public void PressKey1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        controller.enabled = false;
        player.transform.position = new Vector3(580, 21, -7);
        controller.enabled = true;

        noteGreen.gameObject.SetActive(true);
        noteYellow.gameObject.SetActive(true);
        noteBlue.gameObject.SetActive(true);
        noteRed.gameObject.SetActive(true);

        animationYellow.gameObject.SetActive(true);
        animationBlue.gameObject.SetActive(true);
        animationRed.gameObject.SetActive(true);

        Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", false);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", false);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);

        playerMovement.ResetImage();
        playerMovement.SetStartPosition(player.transform.position);

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

        Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", true);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", false);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);

        playerMovement.ResetImage();
        playerMovement.SetStartPosition(player.transform.position);

        playerMovement.SpawnImage(noteGreen.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioGreen);
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

        Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", true);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", true);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);

        playerMovement.ResetImage();
        playerMovement.SetStartPosition(player.transform.position);

        playerMovement.SpawnImage(noteGreen.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioGreen);

        playerMovement.SpawnImage(noteYellow.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioYellow);
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

        Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", true);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", true);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);

        playerMovement.ResetImage();
        playerMovement.SetStartPosition(player.transform.position);

        playerMovement.SpawnImage(noteGreen.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioGreen);

        playerMovement.SpawnImage(noteYellow.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioYellow);

        playerMovement.SpawnImage(noteBlue.GetComponent<Renderer>().materials[0].color);
        playerMovement.AddSequence(audioBlue);
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

        Animator anim1 = door1.GetComponent<Animator>();
        anim1.SetBool("IsOpened", true);

        Animator anim2 = door2.GetComponent<Animator>();
        anim2.SetBool("IsOpened", true);

        Animator anim3 = door3.GetComponent<Animator>();
        anim3.SetBool("IsOpened", false);

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
    }

}
