using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;
using Random = UnityEngine.Random;

public class ShootMultipleLaser : BaseSkill
{
    List<ITarget> _targets;

    Timer _delayTimer;
    ShootMultipleLaserData _data;
    BaseFactory _effectFactory;

    public ShootMultipleLaser(ShootMultipleLaserData data, BaseFactory effectFactory) : base(Type.Basic, data.MaxUpgradePoint)
    {
        _data = data;
        _delayTimer = new Timer();
        _targets = new List<ITarget>();
        _effectFactory = effectFactory;
    }

    void ShootLaser(float angle)
    {
        Transform casterTransform = _caster.GetComponent<Transform>();
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.SpreadBullets, casterTransform.position, 0.3f);

        float x = Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(angle * Mathf.Deg2Rad);
        Vector3 direction = new Vector3(x, y, 0);
        Vector3 spawnPosition = casterTransform.position + direction * _data.DistanceFromCaster;

        DamageableData damageData = new DamageableData
        (
            _caster,
            new DamageStat(
                _data.Damage,
                _upgradeableRatio.AttackDamage,
                _data.AdRatio,
                _upgradeableRatio.TotalDamageRatio
            ),
            _data.TargetTypes,
            _data.GroggyDuration
        );

        RaycastHit2D[] hit2D = Physics2D.RaycastAll(spawnPosition, direction, _data.MaxDistance);
        Array.Sort(hit2D, (x, y) => x.distance.CompareTo(y.distance));

        Vector2 stopPoint = spawnPosition + (direction * _data.MaxDistance);

        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider == null) continue;

            if (hit2D[i].collider.gameObject.layer == LayerMask.NameToLayer("Obstacle")) // º®¿¡ ºÎµúÈù °æ¿ì
            {
                stopPoint = hit2D[i].point;
                break;
            }
            else
            {
                ITarget target = hit2D[i].collider.gameObject.GetComponent<ITarget>();
                if (target == null || target.IsTarget(_data.TargetTypes) == false) continue;

                IDamageable damageable = hit2D[i].collider.gameObject.GetComponent<IDamageable>();
                if (damageable == null) continue;

                Damage.Hit(damageData, damageable);
            }
        }

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.LaserEffect);
        effect.ResetPosition(Vector3.zero);
        effect.ResetColor(new Color(183f / 255f, 47f / 255f, 253f / 255f), new Color(255f / 255f, 93f / 255f, 158f / 255f));
        effect.ResetLine(spawnPosition, stopPoint);
        effect.Play();

        Debug.DrawLine(spawnPosition, stopPoint, Color.blue, 3);
    }

    public override void OnUpdate()
    {

        switch (_delayTimer.CurrentState)
        {
            case Timer.State.Ready:
                if (_targets.Count == 0) return;
                _delayTimer.Start(_data.Delay);

                CastingComponent castingComponent = _caster.GetComponent<CastingComponent>();
                if (castingComponent != null) castingComponent.CastSkill(_data.Delay);

                break;
            case Timer.State.Finish:

                List<int> pickCount = new List<int>();
                while (pickCount.Count < _data.ShootableLaserCount)
                {
                    int random = Random.Range(1, _data.TotalLaserCount + 1);
                    if(pickCount.Contains(random) == false) pickCount.Add(random);
                }

                for (int i = 0; i < pickCount.Count; i++)
                {
                    float angle = 360f / _data.TotalLaserCount * pickCount[i];
                    ShootLaser(angle);
                }
                _delayTimer.Reset();
                break;
            default:
                break;
        }
    }

    public override void OnCaptureEnter(ITarget target)
    {
        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return;

        _targets.Add(target);
    }

    public override void OnCaptureExit(ITarget target)
    {
        bool isTarget = target.IsTarget(_data.TargetTypes);
        if (isTarget == false) return;

        _targets.Remove(target);
    }
}
