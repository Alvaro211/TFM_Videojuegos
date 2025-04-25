using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDirect : MonoBehaviour
{
    public Transform player;//���λ��
    public Transform direct;//��ɫ��ͷ
    public Transform audioTransform;//������Դ
    public GameObject[] allBall;
    Dictionary<GameObject, float> allDistance = new Dictionary<GameObject, float>();
    private void Start()
    {
        allBall = GameObject.FindGameObjectsWithTag("TargetBall");
    }
    // Update is called once per frame
    void Update()
    {
        if (audioTransform != null)
        {


            Vector3 dir = audioTransform.position - player.position;//�������������λ������
            direct.up = -dir;

            foreach (var g in allBall)
            {
                if (!allDistance.ContainsKey(g))
                {
                    allDistance.Add(g, 0);
                }
                else
                {
                    allDistance[g] = Vector3.SqrMagnitude(player.position - g.transform.position);
                }
            }


            GameObject minKey = null;
            float minValue = float.MaxValue;

            foreach (KeyValuePair<GameObject, float> pair in allDistance)
            {
                if (pair.Value < minValue)
                {
                    minValue = pair.Value;
                    minKey = pair.Key;
                }
            }

            audioTransform = minKey.transform;
        }
    }
}
