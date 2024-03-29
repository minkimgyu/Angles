using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class StateYellowHexaagonAttack : StateFollowEnemyAttack
{
    YellowHexagonEnemy loadYellowHexagonEnemy;

    float storedTime;
    bool attackFlag = false;
    bool canAttack = true;

    public StateYellowHexaagonAttack(YellowHexagonEnemy yellowHexagonEnemy) : base(yellowHexagonEnemy)
    {
        loadYellowHexagonEnemy = yellowHexagonEnemy;
    }

    // 계속 발사해야해서 unitask로 제작      
    public override void ExecuteInRangeMethod()
    {
        attackFlag = true;
    }

    public override void OperateUpdate()
    {
        base.OperateUpdate();

        if(attackFlag)
        {
            if(canAttack == true)
            {
                // state를 스킬 사용 시 --> 정지 --> 추적으로 바꿔줌
<<<<<<< Updated upstream
                loadYellowHexagonEnemy.SkillController.UseSkill(BaseSkill.UseConditionType.InRange);
                loadYellowHexagonEnemy.SetState(BaseFollowEnemy.State.Fix);
=======
                loadYellowHexagonEnemy.BattleComponent.UseSkill(SkillUseConditionType.InRange);
                loadYellowHexagonEnemy.MoveComponent.Stop();
                //loadYellowHexagonEnemy.SetState(BaseFollowEnemy.State.Fix);
>>>>>>> Stashed changes
                canAttack = false;
            }
        }

        if(canAttack == false)
        {
            storedTime += Time.deltaTime;
            if(storedTime > loadYellowHexagonEnemy.AttackDelay.IntervalValue)
            {
                storedTime = 0;
                canAttack = true;
            }
        }
    }

    public override void ExecuteInOutsideMethod()
    {
        attackFlag = false;
    }
}

public class StateYellowHexaagonFix : BaseState<BaseFollowEnemy.State>
{
    YellowHexagonEnemy loadYellowHexagonEnemy;

    float storedTime;
    bool canRevertToPreviousState = false;

    public StateYellowHexaagonFix(YellowHexagonEnemy yellowHexagonEnemy)
    {
        loadYellowHexagonEnemy = yellowHexagonEnemy;
    }

    public override void OnMessage(Telegram<BaseFollowEnemy.State> telegram)
    {
    }

    public override void OperateEnter()
    {
        canRevertToPreviousState = false;
        loadYellowHexagonEnemy.MoveComponent.Stop();
    }

    public override void OperateExit()
    {
        canRevertToPreviousState = true;
        storedTime = 0;
    }

    public override void OperateUpdate()
    {
<<<<<<< Updated upstream
        loadYellowHexagonEnemy.Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
=======
        //loadYellowHexagonEnemy.Rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        //loadYellowHexagonEnemy.FixState = true;
>>>>>>> Stashed changes

        if (canRevertToPreviousState == false)
        {
            storedTime += Time.deltaTime;
            if (storedTime > loadYellowHexagonEnemy.FixTime.IntervalValue)
            {
                storedTime = 0;
                canRevertToPreviousState = true;
            }
        }

        if (canRevertToPreviousState)
        {
<<<<<<< Updated upstream
            loadYellowHexagonEnemy.Rigidbody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
=======
            //loadYellowHexagonEnemy.Rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            //loadYellowHexagonEnemy.FixState = false;
>>>>>>> Stashed changes
            loadYellowHexagonEnemy.SetState(BaseFollowEnemy.State.Follow);
        } 
    }
}