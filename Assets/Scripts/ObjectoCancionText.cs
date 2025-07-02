using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class ObjetoCancionText : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            if (text.text == "1")
            {
                text.text = "<b>↑</b>";
                this.transform.position = new Vector3((this.transform.position.x - 0.5f), (this.transform.position.y + 1.5f), this.transform.position.z);
            }
            else if (text.text == "2")
            {
                text.text = "<b>→</b>";
                this.transform.position = new Vector3((this.transform.position.x - 2f), (this.transform.position.y + 2f), this.transform.position.z);
            }
            else if (text.text == "3")
            {
                text.text = "<b>↓</b>";
                this.transform.position = new Vector3((this.transform.position.x - 0.5f), (this.transform.position.y + 1.5f), this.transform.position.z);
            }
            else if (text.text == "4")
            {
                text.text = "<b>←</b>";
                this.transform.position = new Vector3((this.transform.position.x - 2f), (this.transform.position.y + 2f), this.transform.position.z);
            }

            text.fontSize = 40f;
        }
        else
        {
            if (text.text == "<b>↑</b>")
            {
                text.text = "1";
                this.transform.position = new Vector3((this.transform.position.x + 0.5f), (this.transform.position.y - 1.5f), this.transform.position.z);
            }
            else if (text.text == "<b>→</b>")
            {
                text.text = "2";
                this.transform.position = new Vector3((this.transform.position.x + 2f), (this.transform.position.y - 2f), this.transform.position.z);
            }
            else if (text.text == "<b>↓</b>")
            {
                text.text = "3";
                this.transform.position = new Vector3((this.transform.position.x + 0.5f), (this.transform.position.y - 1.5f), this.transform.position.z);
            }
            else if (text.text == "<b>←</b>")
            {
                text.text = "4";
                this.transform.position = new Vector3((this.transform.position.x + 2f), (this.transform.position.y - 2f), this.transform.position.z);
            }

            text.fontSize = 30f;
        }
    }


}
