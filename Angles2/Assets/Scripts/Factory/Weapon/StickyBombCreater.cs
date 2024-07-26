using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StickyBombData : BaseWeaponData
{
    public float _range;
    public float _explosionDelay;

    public StickyBombData(float damage, float range, float explosionDelay) : base(damage)
    {
        _range = range;
        _explosionDelay = explosionDelay;
    }
}

public class StickyBombCreater : WeaponCreater
{
    public override BaseWeapon Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseWeapon weapon = obj.GetComponent<BaseWeapon>();
        if (weapon == null) return null;

        StickyBombData data = Database.Instance.WeaponData[BaseWeapon.Name.StickyBomb] as StickyBombData;
        weapon.Initialize(data);
        return weapon;
    }
}
