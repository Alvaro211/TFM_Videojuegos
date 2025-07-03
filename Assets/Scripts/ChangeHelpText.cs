using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeHelpText : MonoBehaviour
{
    public TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            if (text.text == " F ")
            {
                text.text = " LB ";
            }
        }
        else
        {
            if (text.text == " LB ")
            {
                text.text = " F ";
            }
        }
    }
}
