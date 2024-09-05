using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteracter
{
    //List<SkillUpgradeData> ReturnSkillUpgradeDatas();
    void MovePosition(Vector3 pos);
    IFollowable ReturnFollower();
    void GetHeal(float point);
}
