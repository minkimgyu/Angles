using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteracter
{
    ISkillUser ReturnSkillUser();
    void MovePosition(Vector3 pos);
    IFollowable ReturnFollower();
    void GetHeal(float point);
}
