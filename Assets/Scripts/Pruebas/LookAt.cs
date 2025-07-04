using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public GameObject ball;
    // redball, blueball, yellowball, 
    private Vector3 vector;
    // Update is called once per frame
    void Update()
    {
        vector = new Vector3( this.transform.position.x, this.transform.position.y, ball.transform.position.z- this.transform.position.z) ;
       this.transform.rotation = Quaternion.LookRotation(vector);
    }
}
