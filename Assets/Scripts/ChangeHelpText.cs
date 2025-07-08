using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeHelpText : MonoBehaviour
{
    public TextMeshPro text;
    public SpriteRenderer iconLB;

    public SpriteRenderer iconStickR;
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
            Cursor.visible = false;
            text.gameObject.SetActive(false);
            iconLB.gameObject.SetActive(true);

            iconStickR.gameObject.SetActive(true);
        }
        else
        {
            Cursor.visible = true;
            text.gameObject.SetActive(true);
            iconLB.gameObject.SetActive(false);

            iconStickR.gameObject.SetActive(false);
        }
    }
}
