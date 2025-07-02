using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWenhao : MonoBehaviour
{
   /* public GameObject wenhao;

    private void Start()
    {
        Destroy(GetComponent<Collider>());
    }

    private void OnEnable()
    {
        var s = gameObject.AddComponent<SphereCollider>();
        s.radius = 15f;
        s.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            if (other.GetComponent<Enemy>().gantanPref != null)
            {
                Destroy(other.GetComponent<Enemy>().gantanPref);
            }

            GameObject obj = Instantiate(wenhao);
            obj.GetComponent<CameraForward>().target = other.transform;
            Destroy(obj, 5f);
            Destroy(GetComponent<Collider>());
        }
    }*/
}
