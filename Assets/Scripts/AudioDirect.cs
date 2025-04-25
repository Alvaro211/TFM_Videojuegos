using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDirect : MonoBehaviour
{
    public Transform player;//玩家位置
    public Transform direct;//红色箭头
    public Transform audioTransform;//声音来源
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


            Vector3 dir = audioTransform.position - player.position;//计算声音与玩家位置向量
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
