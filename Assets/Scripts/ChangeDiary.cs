using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChangeDiary : MonoBehaviour
{
    public Image diary;

    public Sprite diaryN;
    public Sprite diaryLB;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null && diary.sprite == diaryN)
            diary.sprite = diaryLB;
        else if (Gamepad.current == null && diary.sprite == diaryLB)
            diary.sprite = diaryN;
    }
}
