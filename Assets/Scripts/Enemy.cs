using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
/*    private enum di_ren_zhuang_tai
    {
        巡逻,
        迷惑,
        追逐
    }
    private enum zhui_zhu_dui_xiang_lei
    {
        玩家,
        光球,
        无
    }
    [SerializeField]
    private di_ren_zhuang_tai ZhuangTai = di_ren_zhuang_tai.巡逻;*/
    public float patrolDistance = 5f; // Distancia que avanzar?en Z
    public float waitTime = 2f;         // Tiempo de espera en cada punto
    public float searchRadius = 15;
    [SerializeField] float XunLuoSuDu;
    [SerializeField] float ZhuiZhuShuDu;
    public bool horizontal = true;
    public bool isStunned = false;

    [Header("感叹号")]
    [SerializeField] private GameObject GanTan;
    [SerializeField] private GameObject WenHao;
    public GameObject gantanPref; //可以废弃的变量
    private Vector3 MuBiao_WeiZhi;
    private NavMeshAgent agen;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingForward = true;

    private Animator anim;
    private SpriteRenderer TuPian;


    private Vector3 WanJa_WeiZhi;
    private Vector3 GuangQui_WeiZhi;
    private GameObject GuangQui;

    private float DengDai_JiShi = 0;
    private float ZhuiShaJianZhi_JiShi = 0;
    private float MiHuo_JiShi = 0;
    private float FangQiZhuiZhu_JiShi = 0;


    NavMeshPath WanJia_LuJing;
    NavMeshPath GuangQiu_LuJing;

    void Start()
    {
        agen = GetComponent<NavMeshAgent>();
        startPosition = transform.position; // Guarda la posici髇 inicial
        WanJia_LuJing = new();
        GuangQiu_LuJing = new();

        GanTan.SetActive(false);
        WenHao.SetActive(false);

        if (!horizontal)
            targetPosition = startPosition + new Vector3(0, 0, patrolDistance);
        else
            targetPosition = startPosition + new Vector3(patrolDistance, 0, 0);

        // Moverse al primer destino
        agen.SetDestination(targetPosition);


        TuPian = this.gameObject.GetComponentInChildren<SpriteRenderer>();




        anim = this.transform.GetChild(0).transform.GetComponent<Animator>();
    }
    bool NengZuiDao;
    GameObject playerObj;
    void Update()
    {

        if (playerObj == null)
        {
            playerObj = GameObject.FindObjectOfType<PlayerMovement>().gameObject;
            WanJa_WeiZhi = playerObj.transform.position;
        }
        else
        {
            WanJa_WeiZhi = playerObj.transform.position;
        }

        jianzaofangxiang();

        if ((Vector3.Distance(transform.position, WanJa_WeiZhi) <= searchRadius
        && agen.CalculatePath(WanJa_WeiZhi, WanJia_LuJing)
        && WanJia_LuJing.status == NavMeshPathStatus.PathComplete)
        ||
         (GuangQui != null && GuangQui.activeInHierarchy && Vector3.Distance(transform.position, GuangQui_WeiZhi) < searchRadius
        && agen.CalculatePath(GuangQui_WeiZhi, GuangQiu_LuJing)
        && GuangQiu_LuJing.status == NavMeshPathStatus.PathComplete)
        )
        {
           // ZhuangTai = di_ren_zhuang_tai.追逐;
            NengZuiDao = true;

        }
        else
        {
            NengZuiDao = false;

        }



       /* switch (ZhuangTai)
        {
            case di_ren_zhuang_tai.巡逻:
                XunLuoXingWei();
                break;
            case di_ren_zhuang_tai.迷惑:
                MiHuo_xingwei();

                break;
            case di_ren_zhuang_tai.追逐:
                ZhuiZhu_xingwei();
                break;
            default:
                break;
        }*/


    }


    void MiHuo_xingwei()
    {

        MiHuo_JiShi += Time.deltaTime;
        if (MiHuo_JiShi < 5f)
        {
            anim.SetBool("IsIdle", false);
            anim.SetBool("IsChasing", false);
            anim.SetBool("IsConfuse", true);
            isStunned = true;


        }
        else
        {
            MiHuo_JiShi = 0;
            fanghuixunluo();
            isStunned = false;




        }

    }

    void ZhuiZhu_xingwei()
    {

        DengDai_JiShi = 0;

        //zhui_zhu_dui_xiang_lei MuBiaoLeiXing = PanDuanMuBiaoLeiXing();
        anim.SetBool("IsChasing", true);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsConfuse", false);
        agen.SetDestination(MuBiao_WeiZhi);//设置目标
        agen.speed =ZhuiZhuShuDu;
        GanTan.SetActive(true);
        WenHao.SetActive(false);
        if (Vector2.Distance(MuBiao_WeiZhi, transform.position) <= agen.stoppingDistance) //这里是当敌人追到目标后的逻辑
        {

           /* switch (MuBiaoLeiXing)
            {
                case zhui_zhu_dui_xiang_lei.玩家:
                    GanTan.SetActive(true);
                    WenHao.SetActive(false);
                    anim.SetBool("IsIdle", true);
                    anim.SetBool("IsChasing", false);
                    anim.SetBool("IsConfuse", true);
                    agen.speed = XunLuoSuDu;
                    agen.ResetPath();
                    ZhuiShaJianZhi_JiShi += Time.deltaTime;
                    if (ZhuiShaJianZhi_JiShi > 0.5f)
                    {
                        ZhuiShaJianZhi_JiShi = 0;
                        fanghuixunluo();
                    }
                    break;
                case zhui_zhu_dui_xiang_lei.光球: //因为其它脚本的调用,迷惑状态被分成了两截,该状态下,一开始就要调方法写这里,持续调用的,两边都写
                    anim.SetBool("IsChasing", false);
                    anim.SetBool("IsIdle", false);
                    anim.SetBool("IsConfuse", true);
                    GanTan.SetActive(false);
                    WenHao.SetActive(true);
                    isStunned = true;
                    MiHuo_JiShi += Time.deltaTime;
                    ZhuangTai = di_ren_zhuang_tai.迷惑;
                    break;
                default:
                    fanghuixunluo();
                    break;
            }*/

        }
        else if (!NengZuiDao) //追逐过程中,目标消失或路径无法到达
        {

            FangQiZhuiZhu_JiShi += Time.deltaTime;
            if (FangQiZhuiZhu_JiShi > 2)
            {
                fanghuixunluo();
                FangQiZhuiZhu_JiShi = 0;
                GanTan.SetActive(false);
                WenHao.SetActive(false);

            }
        }
    }
    private void XunLuoXingWei()
    {


        if (agen.remainingDistance <= agen.stoppingDistance)
        {
            if (DengDai_JiShi < waitTime)
            {
                anim.SetBool("IsIdle", true);
                DengDai_JiShi += Time.deltaTime;

            }
            else
            {
                DengDai_JiShi = 0;
                anim.SetBool("IsIdle", false);
                agen.SetDestination(movingForward ? startPosition : targetPosition);
                movingForward = !movingForward;
            }
        }

    }


    /*zhui_zhu_dui_xiang_lei PanDuanMuBiaoLeiXing()
    {
        if (GuangQui != null && GuangQui.activeInHierarchy && Vector3.Distance(transform.position, GuangQui_WeiZhi) < searchRadius &&
            Vector3.Distance(transform.position, GuangQui_WeiZhi) < Vector3.Distance(transform.position, WanJa_WeiZhi))
        {
            MuBiao_WeiZhi = GuangQui_WeiZhi;

            return zhui_zhu_dui_xiang_lei.光球;
        }
        else if (Vector3.Distance(transform.position, WanJa_WeiZhi) < searchRadius)
        {
            MuBiao_WeiZhi = WanJa_WeiZhi;
            isStunned = WenHao.activeInHierarchy;//因为玩家可能在敌人迷惑状态中穿过敌人
            return zhui_zhu_dui_xiang_lei.玩家;

        }
        else
        {
            return zhui_zhu_dui_xiang_lei.无;
        }
    }*/

    public void MoveToBall(GameObject ballPosition)
    {
        GuangQui = ballPosition;
        GuangQui_WeiZhi = ballPosition.transform.position;
    }


    private void jianzaofangxiang()
    {
        float directionToPlayer = agen.destination.x - transform.position.x; //负数为向左走

        if (directionToPlayer < 0)
        {
            TuPian.transform.rotation = Quaternion.Euler(0.0f, -180f, 0.0f);
        }
        else if (directionToPlayer > 0)
        {
            TuPian.transform.rotation = Quaternion.Euler(0.0f, 0f, 0.0f);
        }


    }

    private void fanghuixunluo()
    {
        agen.speed = XunLuoSuDu;
        agen.SetDestination(movingForward ? startPosition : targetPosition);
      //  ZhuangTai = di_ren_zhuang_tai.巡逻;
        anim.SetBool("IsChasing", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsConfuse", false);

        GanTan.SetActive(false);
        WenHao.SetActive(false);
    }
}

