using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWenhao : MonoBehaviour
{
    public GameObject wenhao;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            GameObject obj = Instantiate(wenhao);
            obj.GetComponent<CameraForward>().target = other.transform;
            Destroy(obj, 5f);
        }
    }
}
