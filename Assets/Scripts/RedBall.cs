using UnityEngine;

public class RedBall : MonoBehaviour
{


    public GameObject SiWang_TeXiao;

    private void OnTriggerEnter(Collider other)
    {
        // Si la bola es golpeada por otro objeto con el tag "Ball"
        if (other.CompareTag("Ball"))
        {
            this.gameObject.SetActive(false);
            var i = Instantiate(SiWang_TeXiao);
            i.transform.position = transform.position;
            Destroy(i, 2);
        }

    }


}