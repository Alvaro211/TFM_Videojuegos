using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAudioListener : MonoBehaviour
{
    public PlayerMovement player;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position;
    }
}
