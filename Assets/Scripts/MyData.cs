using System;
using UnityEngine;
[Serializable]
public class MyData
{
    [Serializable]
    public struct PlayerStats
    {
        public float positionX;
        public float positionY;
        public float positionZ;

        public bool noteGreen;
        public bool noteYellow;
        public bool noteBlue;
        public bool noteRed;
    }
    [SerializeField]
    public PlayerStats player;

    public bool continueGame;

    public float musicVolume;
    public float effectVolume;
    public bool helpControls;

    public bool isOpenDoorGreen;
    public bool isOpenDoorGreenYellow;
    public bool isOpenDoorBoss;

    public bool isSeenCinematic2;
    public bool isSeenCinematic3;
    public bool isSeenCinematic4;

    public bool isTakeColeccionable1;
    public bool isTakeColeccionable2;
    public bool isTakeColeccionable3;
}

