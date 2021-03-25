using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pawn2 : PawnFSM
{
    private Animator animator;
    private NavMeshAgent agent;
    private Coroutine async;
    public GameObject bullet;
    public float hp = 20;
    public float attack;
    public float attackDistance;
    public float cd = 1;
    public float speed;   

    private float time = 5.0f;
    private int i = 0;    
    private bool AttackLock;//攻击锁，防止重复攻击

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.speed = speed;

        AttackLock = false;//开始时是锁住的
    }

    public override void StateIdle()
    {
        agent.speed = speed;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 5.0f;
            i = Random.Range(0, target.Length);//计时器，再不同的巡逻点之间进行随机选择性巡逻
            //Debug.Log(i);
        }
        agent.SetDestination(target[i].transform.position);//朝向目的移动
        animator.SetFloat("speed", speed);
        if (distance < attackDistance * 2 && distance != 0)
        {
            //Debug.Log(distance);
            ChangeWalk();
        }
    }
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
            if (distance < attackDistance && distance != 0)
            {
                //Debug.Log(distance);
                ChangeAttack();
            }
        }
    }
    public override void StateAttack()
    {
        if (AttackLock == false)
        {
            int j = Random.Range(0, 2);//随机数攻击动画
            AttackLock = true;//锁住
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
            Bullet bc = bullet.GetComponent<Bullet>();
            bc.enemyTag = enemyTag;
            bc.range = attackDistance * 10;
            bc.attack = attack;
            Instantiate(bullet, transform.position + transform.up * 4 + transform.forward * 8, transform.rotation);
            if (async != null)
            {
                StopCoroutine(StateChange());
            }
            async = StartCoroutine(StateChange());
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
        if (hp <= 0)
        {
            hp = 0;
            ChangeDead();
        }
    }
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
}