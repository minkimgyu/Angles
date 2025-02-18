using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ProjectileWeapon : BaseWeapon, IProjectable
{
    //protected float _force;
    //protected MoveComponent _moveComponent;

    //// 여기서 virtual 함수로 둘 다 만들어주기
    //// 자식 클래스에서 마저 구현하기

    //public virtual void Shoot(Vector3 direction, float force)
    //{
    //    transform.right = direction;
    //    _force = force;

    //    _moveComponent.Stop();
    //    _moveComponent.AddForce(direction, _force);
    //}

}
