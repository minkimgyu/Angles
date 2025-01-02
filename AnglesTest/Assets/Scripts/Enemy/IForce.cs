using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IForce : IPos
{
    bool CanApplyForce();
    void ApplyForce(Vector3 direction, float force, ForceMode2D mode); // ���� ������ force�� ������
}
