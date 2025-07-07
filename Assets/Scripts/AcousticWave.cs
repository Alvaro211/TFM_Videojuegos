using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcousticWave : MonoBehaviour
{
    [SerializeField] private float xiaoShi_shiJian = 1.5f;
    [SerializeField] private GameObject wave;


    public void xianShi(float duration)
    {
        wave.SetActive(true);
        Invoke(nameof(xiaoShi), duration);
    }

    void xiaoShi()
    {
        wave.SetActive(false);
    }
}