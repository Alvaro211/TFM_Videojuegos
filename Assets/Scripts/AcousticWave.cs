using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcousticWave : MonoBehaviour
{
    [SerializeField] private float xiaoShi_shiJian = 1.5f;
    [SerializeField] private GameObject wave;


    public void xianShi(float duration)
    {
        wave.SetActive(true);
        RawImage rawIamge = this.GetComponent<RawImage>();
        if(rawIamge != null)
            wave.GetComponent<Image>().color = rawIamge.color;
        Invoke(nameof(xiaoShi), duration);
    }

    void xiaoShi()
    {
        wave.SetActive(false);
    }
}