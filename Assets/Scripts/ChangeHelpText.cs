using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeHelpText : MonoBehaviour
{
    public TextMeshPro text;
    public SpriteRenderer iconLB;
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
            text.gameObject.SetActive(false);
            iconLB.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(true);
            iconLB.gameObject.SetActive(false);
        }
    }
}
