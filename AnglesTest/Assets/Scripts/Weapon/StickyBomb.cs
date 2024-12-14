using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class StickyBomb : BaseWeapon
{
    IFollowable _followable;

    BaseFactory _effectFactory;
    StickyBombData _data;

    public override void ResetData(StickyBombData data)
    {
        _data = data;
    }

    public override void ModifyData(List<WeaponDataModifier> modifiers)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            _data = modifiers[i].Visit(_data);
        }
    }

    public override void Initialize(BaseFactory effectFactory) 
    {
        _effectFactory = effectFactory;
        _lifetimeComponent = new LifetimeComponent(_data, () => { Explode(); Destroy(gameObject); });
        _sizeModifyComponent = new NoSizeModifyComponent();
    }

    public override void ResetFollower(IFollowable followable) 
    {
        _followable = followable;
    }

    void Explode()
    {
        Debug.Log("Explode");
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Explosion, transform.position, 0.4f);

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ExplosionEffect);
        effect.ResetPosition(transform.position);
        //effect.ResetSize(_data._range); 
        effect.Play();

        Damage.HitCircleRange(_data._damageableData, transform.position, _data._range, true, Color.red, 3);
    }

    protected override void Update()
    {
        base.Update();
        if (_followable as UnityEngine.Object == null) return;

        bool canAttach = _followable.CanFollow();
        if (canAttach == true)
        {
            Vector3 pos = _followable.ReturnPosition();
            transform.position = pos;
        }
    }
}
