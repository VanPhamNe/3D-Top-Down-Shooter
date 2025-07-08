using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected string boolName;
    protected float stateTimer; //  Thoi gian cho tung trang thai
    protected bool triggered; // Kiem tra trang thai da duoc kich hoat hay chua
    public EnemyState(Enemy enemy,EnemyStateMachine stateMachine,string boolName)
    {
        this.enemyBase = enemy;
        this.stateMachine = stateMachine;
        this.boolName = boolName;
    }
    public virtual void Enter()
    {
        enemyBase.anim.SetBool(boolName, true); // Bat dau trang thai hien tai
        triggered = false; // Dat trang thai da duoc kich hoat ve false

    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime; // Giam thoi gian cua state hien tai
        if (triggered)
        {
            //Debug.Log("Chang state now");
        }
    }
    public virtual void Exit()
    {
       enemyBase.anim.SetBool(boolName, false); // Ket thuc trang thai hien tai
    }
    public void AnimationTrigger() => triggered = true; // Kich hoat trang thai da duoc kich hoat
    public virtual void AbilityTrigger()
    {

    }
}
