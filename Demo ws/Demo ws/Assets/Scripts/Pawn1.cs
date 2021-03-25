using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pawn1 : PawnFSM
{


    private NavMeshAgent agent;
    private Animator animator;
    private Coroutine async;
    private Vector3 temVec;//敌我向量

    public float hp;//生命值
    public float attack;//攻击力
    public float attackDistance;//攻击范围
    public float cd;//攻击cd
    public float speed;//移动速度

    private float time = 5.0f;//巡逻目标点时间，每time换一个目标点
    private int i = 0;//巡逻目标点指示器
    private bool AttackLock;//攻击锁，防止重复攻击

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.speed = speed;

        AttackLock = false;//锁住
    }
    ///待机
    public override void StateIdle()
    {
        agent.speed = speed;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 5.0f;
            i = Random.Range(0, target.Length);//计时器，再不同的巡逻点之间进行随机选择性巡逻
        }
        agent.SetDestination(target[i].transform.position);//朝向目的移动
        animator.SetFloat("speed", speed);
        if (distance < attackDistance * 2)
        {
            //Debug.Log(distance);
            ChangeWalk();
        }
    }
    ///追击
    public override void StateWalk()
    {
        if (enemyPawn == null)
        {
            distance = 0;
            ChangeIdle();
        }
        else
        {
            agent.speed = speed;
            agent.SetDestination(enemyPawn.transform.position);
            animator.SetFloat("speed", speed);
            if (distance < attackDistance)
            {
                //Debug.Log(distance);
                ChangeAttack();
            }
        }
    }
    ///攻击
    public override void StateAttack()
    {
        if (enemyPawn == null)
        {
            distance = 0;
            ChangeIdle();
        }
        else
        {
            if (AttackLock == false)
            {
                int j = Random.Range(0, 2);//攻击动画随机
                AttackLock = true;//攻击锁
                agent.speed = 0;
                gameObject.transform.LookAt(enemyPawn.transform.position);
                switch (j)
                {
                    case 0:
                        animator.SetTrigger("attack1");
                        break;
                    case 1:
                        animator.SetTrigger("attack2");
                        break;
                    default:
                        animator.SetTrigger("attack3");
                        break;
                }
                if (RectangleAttack(transform, enemyPawn, attackDistance, attackDistance))
                {
                    var hit = enemyPawn.gameObject;
                    if (hit.GetComponent<Pawn1>() != null)
                    {
                        Pawn1 pawn = hit.GetComponent<Pawn1>();
                        if (pawn != null)
                        {
                            //Debug.Log("hp：-10");
                            pawn.TakeDamage(attack);
                        }
                    }
                    else if (hit.GetComponent<Pawn2>() != null)
                    {
                        Pawn2 pawn = hit.GetComponent<Pawn2>();
                        if (pawn != null)
                        {
                            //Debug.Log("hp：-10");
                            pawn.TakeDamage(attack);
                        }
                    }
                }
                if (async != null)
                {
                    StopCoroutine(StateChange());
                }
                async = StartCoroutine(StateChange());
            }
        }
    }
    ///死亡
    public override void StateDead()
    {
        agent.speed = 0;
        animator.SetTrigger("die");
        Destroy(gameObject, 1.0f);
    }
    //扣血
    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp < 0)
        {
            hp = 0;
            ChangeDead();
        }
    }
    //协程：
    private IEnumerator StateChange()
    {
        yield return new WaitForSeconds(cd);
        AttackLock = false;//锁住攻击
        async = null;//让协程为空，只触发最后一次调用的协程。
        if (enemyPawn == null)
        {
            distance = 0;
            ChangeIdle();
        }
        else
        {
            if (distance > attackDistance)//距离大于攻击距离就进入追敌
                ChangeWalk();
            else//距离小于攻击距离，继续下一次攻击。            
                ChangeAttack();
        }
    }
    /// <summary>
    /// 矩形攻击判定
    /// </summary>
    /// <param name="Pawn">小兵位置</param>
    /// <param name="target">敌方位置</param>
    /// <param name="rectangleWide">矩形的宽度</param>
    /// <param name="rectangleHigh">矩形的高度</param>
    /// <returns></returns>
    public bool RectangleAttack(Transform Player, Transform target, float rectangleWide, float rectangleHigh)
    {
        //得到小兵位置指向敌方物体的方向向量
        Vector3 dirVector = target.position - Player.position;
        //与小兵位置正前方做点乘 得到投影
        float forwardDistance = Vector3.Dot(dirVector, Player.forward.normalized);
        if (forwardDistance > 0 && forwardDistance < rectangleHigh)
        {
            //此时在矩形的高度范围内与小兵位置正右方标量继续做点乘得到投影
            float rightDistance = Vector3.Dot(dirVector, Player.right.normalized);
            //绝对值小于矩形的宽度的一半
            if (Mathf.Abs(rightDistance) <= rectangleWide * 0.5f)
            {
                return true;
            }
            return false;
        }
        return false;
    }
}
