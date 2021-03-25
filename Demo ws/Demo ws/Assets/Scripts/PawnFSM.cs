using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State//Idle0 布置时不动, Idle 待机巡逻, Walk 追击, Attack 攻击, Dead 死亡,
{
    Idle0, Idle, Walk, Attack, Dead,
}

public enum AIMode //AI类型：current 最先的；First 最近的
{
    current,
    First
}
public class PawnFSM : MonoBehaviour//FSM框架
{
    [HideInInspector]
    public State currentState;//状态
    [HideInInspector]
    public Transform enemyPawn;//敌人位置
    [HideInInspector]
    public Transform[] target;//待机巡逻目标点
    [HideInInspector]
    public float distance = 0;//判断敌我距离，默认0
    [HideInInspector]
    public string enemyTag;//敌方Tag
    //[HideInInspector]
    //public List<Transform> EnemyList;
    public AIMode aiMode;//AI类型
    //private Transform currentEnemy;
    //private Transform FirstEnemy;
    private void Start()
    {
        currentState = State.Idle0;//默认状态下为站立
        if (gameObject.tag == "Blue")//我方Tag为蓝，则敌方为红；反之...
            enemyTag = "Red";
        else
            enemyTag = "Blue";

    }

    private void Update()
    {
        if (enemyPawn != null)
        {
            distance = Vector3.Distance(enemyPawn.position, transform.position);//敌我距离
        }
        switch (currentState)
        {
            case State.Idle0:
                break;
            case State.Idle:
                StateIdle();
                break;
            case State.Walk:
                StateWalk();
                break;
            case State.Attack:
                StateAttack();
                break;
            case State.Dead:
                StateDead();
                break;
        }
    }
    public virtual void StateIdle()//站立状态
    {

    }
    public virtual void StateWalk()//行走状态
    {

    }
    public virtual void StateAttack()//攻击状态
    {

    }
    public virtual void StateDead()//死亡状态
    {

    }
    public virtual void ChangeIdle()
    {
        currentState = State.Idle;
    }
    public virtual void ChangeWalk()
    {
        currentState = State.Walk;
    }
    public virtual void ChangeAttack()
    {
        currentState = State.Attack;
    }
    public virtual void ChangeDead()
    {
        currentState = State.Dead;
    }
    void OnTriggerStay(Collider other)
    {
        //视野范围内敌方
        if (other.gameObject.CompareTag(enemyTag))
        {
            if (enemyPawn == null)//先锁定的优先
            {
                enemyPawn = other.gameObject.transform;
            }
            else if (aiMode == AIMode.First)//靠近的优先
            {
                if (distance > Vector3.Distance(other.gameObject.transform.position, transform.position))
                    enemyPawn = other.gameObject.transform;
            }
        }

    }
}