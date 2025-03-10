using DamageUtility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StickyBombAttackStrategy : IWeaponActionStrategy
{
    Transform _myTransform;
    StickyBombData _stickyBombData;

    public StickyBombAttackStrategy(Transform myTransform, StickyBombData stickyBombData)
    {
        _myTransform = myTransform;
        _stickyBombData = stickyBombData;
    }

    public void OnLifetimeCompleted() 
    {
        Damage.HitCircleRange(_stickyBombData.DamageableData, _myTransform.position, _stickyBombData.Range, true, Color.red, 3);
    }
}
