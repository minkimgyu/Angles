using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Rocket : ProjectileWeapon
{
    BaseFactory _effectFactory;
    RocketData _data;

    public override void ResetData(RocketData data)
    {
        _data = data;
    }

    public override void Initialize(BaseFactory effectFactory)
    {
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _lifetimeComponent = new LifetimeComponent(_data, () => { Destroy(gameObject); });
        _sizeModifyComponent = new NoSizeModifyComponent();

        _effectFactory = effectFactory;
    }

    void Hit()
    {
        // 폭발을 우선 적용하고 이후에 접촉한 적에 대해 데미지를 가한다.
        SpawnExplosionEffect();

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Explosion, transform.position, 0.4f);
        Damage.HitCircleRange(_data.DamageableData, transform.position, _data.Range, true, Color.red, 3);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        ITarget target = collision.GetComponent<ITarget>();
        if (target == null) // 벽의 경우
        {
            Hit();
            return;
        }

        if (target.IsTarget(_data.DamageableData._targetType) == true)
        {
            Hit();
            return;
        }
    }

    void SpawnExplosionEffect()
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ExplosionEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    public override void ModifyData(RocketDataModifier modifier)
    {
        _data = modifier.Visit(_data);
    }
}