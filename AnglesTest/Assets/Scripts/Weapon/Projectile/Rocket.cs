using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;

public class Rocket : BaseWeapon, IProjectable
{
    BaseFactory _effectFactory;
    RocketData _data;

    public override void InjectData(RocketData data)
    {
        _data = data;
    }

    public void Shoot(Vector3 direction, float force)
    {
        transform.right = direction;
        _force = force;

        _moveComponent.Stop();
        _moveComponent.AddForce(direction, _force);
    }

    float _force;
    MoveComponent _moveComponent;

    public override void Initialize(BaseFactory effectFactory)
    {
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();

        _lifeTimeStrategy = new ChangeableLifeTimeStrategy(_data, () => { Destroy(gameObject); });
        _sizeStrategy = new NoSizeStrategy();
        _attackStrategy = new RocketAttackStrategy(_data, OnHit);

        _effectFactory = effectFactory;
    }

    void SpawnExplosionEffect()
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.ExplosionEffect);
        effect.ResetPosition(transform.position);
        effect.Play();
    }

    void OnHit()
    {
        // 폭발을 우선 적용하고 이후에 접촉한 적에 대해 데미지를 가한다.
        SpawnExplosionEffect();

        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Explosion, transform.position, 0.4f);
        Damage.HitCircleRange(_data.DamageableData, transform.position, _data.Range, true, Color.red, 3);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
       _attackStrategy.OnTargetEnter(collider);
    }

    public override void ModifyData(RocketDataModifier modifier)
    {
        _data = modifier.Visit(_data);
    }
}