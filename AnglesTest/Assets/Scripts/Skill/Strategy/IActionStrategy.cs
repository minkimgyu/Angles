using DamageUtility;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Skill.Strategy.SpawnBladeStrategy;

namespace Skill.Strategy
{
    public interface IActionStrategy
    {
        // 성공적으로 실행했을 때 true 반환
        //void OnReflect(GameObject targetObject, Vector3 contactPos) { }
        void Execute(List<IDamageable> damageables, HitTargetStrategy.ChangeableData data) { }
        void Execute(IDamageable damageables, HitTargetStrategy.ChangeableData data) { }
        void Execute(IFollowable followable, SpawnStickyBombStrategy.ChangeableData data) { }
        BaseWeapon Execute(IFollowable followable, SpawnShooterStrategy.ChangeableData data) { return default; }
        void Execute(SpawnBladeStrategy.ChangeableData data) { }
        void Execute(SpawnBlackholeStrategy.ChangeableData data) { }
        void Execute(SpreadMissileStrategy.ChangeableData data, List<ITarget> targets) { }
        void Execute(SpreadBulletStrategy.ChangeableData data) { }

        void OnAdd() { }
        void OnUpgrade() { }
        void OnRevive() { }
    }

    public class NoActionStrategy : IActionStrategy
    {
    }

    public class UpgradeStatStrategy : IActionStrategy
    {
        ICaster _caster;
        IStatModifier _statModifier;

        Func<int> GetUpgradePoint;

        public void OnUpgrade() => UpgradeStat();
        public void OnAdd() => UpgradeStat();

        public UpgradeStatStrategy(ICaster caster, IStatModifier statModifier, Func<int> getUpgradePoint)
        {
            _caster = caster;
            _statModifier = statModifier;
            GetUpgradePoint = getUpgradePoint;
        }

        void UpgradeStat()
        {
            IStatUpgradable visitor = _caster.GetComponent<IStatUpgradable>();
            if (visitor == null) return;

            visitor.Upgrade(_statModifier, GetUpgradePoint() - 1);
        }
    }

    public class SpawnShooterStrategy : IActionStrategy
    {
        public struct ChangeableData
        {
            float _damage;
            float _delay;

            public ChangeableData(float damage, float delay)
            {
                _damage = damage;
                _delay = delay;
            }

            public float Damage { get => _damage; }
            public float Delay { get => _delay; }
        }

        ICaster _caster;
        IUpgradeableSkillData _upgradeableSkillData;
        BaseWeapon.Name _shooterName;

        float _adRatio;

        float _groggyDuration;
        List<ITarget.Type> _targetTypes;
        BaseFactory _weaponFactory;

        public SpawnShooterStrategy(
            ICaster caster,
            IUpgradeableSkillData upgradeableSkillData,
            BaseWeapon.Name shooterName,
            float adRatio,
            float damage,
            float delay,

            float groggyDuration,
            List<ITarget.Type> targetTypes,
            BaseFactory weaponFactory)
        {
            _caster = caster;
            _upgradeableSkillData = upgradeableSkillData;
            _shooterName = shooterName;

            _adRatio = adRatio;

            _groggyDuration = groggyDuration;
            _targetTypes = targetTypes;
            _weaponFactory = weaponFactory;
        }

        public BaseWeapon Execute(IFollowable followable, ChangeableData data)
        {
            BaseWeapon weapon = _weaponFactory.Create(_shooterName);
            if (weapon == null) return null;

            DamageableData damageData = new DamageableData
            (
                _caster,
                new DamageStat(
                    data.Damage,
                    _upgradeableSkillData.AttackDamage,
                    _adRatio,
                    _upgradeableSkillData.TotalDamageRatio
                ),
                _groggyDuration
            );

            ShooterDataModifier shooterDataModifier = new ShooterDataModifier(damageData, data.Delay, _targetTypes);
            Transform casterTransform = _caster.GetComponent<Transform>();

            weapon.ModifyData(shooterDataModifier);
            weapon.Activate();
            weapon.ResetPosition(casterTransform.position);
            weapon.InjectFollower(followable);
            return weapon;
        }
    }

    public class SpawnBladeStrategy : IActionStrategy
    {
        public struct ChangeableData
        {
            float _damage;
            float _sizeMultiply;
            float _lifetime;

            public ChangeableData(float damage, float sizeMultiply, float lifetime)
            {
                _damage = damage;
                _sizeMultiply = sizeMultiply;
                _lifetime = lifetime;
            }

            public float Damage { get => _damage; }
            public float SizeMultiply { get => _sizeMultiply; }
            public float Lifetime { get => _lifetime; }
        }

        ICaster _caster;
        IUpgradeableSkillData _upgradeableSkillData;

        float _adRatio;
        float _force;


        float _groggyDuration;
        List<ITarget.Type> _targetTypes;
        BaseFactory _weaponFactory;

        public SpawnBladeStrategy(
            ICaster caster,
            IUpgradeableSkillData upgradeableSkillData,
            float adRatio,
            float force,

            float groggyDuration,
            List<ITarget.Type> targetTypes,
            BaseFactory weaponFactory)
        {
            _caster = caster;
            _upgradeableSkillData = upgradeableSkillData;
            _adRatio = adRatio;
            _force = force;

            _groggyDuration = groggyDuration;
            _targetTypes = targetTypes;
            _weaponFactory = weaponFactory;
        }

        public void Execute(ChangeableData data)
        {
            BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Blade);
            if (weapon == null) return;

            DamageableData damageData = new DamageableData
            (
                _caster,
                new DamageStat(
                    data.Damage,
                    _upgradeableSkillData.AttackDamage,
                    _adRatio,
                    _upgradeableSkillData.TotalDamageRatio
                ),
                _groggyDuration
            );

            Transform casterTransform = _caster.GetComponent<Transform>();

            BladeDataModifier stickyBombDataModifier = new BladeDataModifier(
                damageData,
                data.SizeMultiply,
                data.Lifetime,
                _targetTypes);

            weapon.ModifyData(stickyBombDataModifier);
            weapon.Activate();
            weapon.ResetPosition(casterTransform.position);

            IProjectable projectile = weapon.GetComponent<IProjectable>();
            if (projectile == null) return;

            Vector2 direction = casterTransform.right;
            projectile.Shoot(direction, _force);
        }
    }

    public class SpawnStickyBombStrategy : IActionStrategy
    {
        public struct ChangeableData
        {
            float _damage;

            public ChangeableData(float damage)
            {
                _damage = damage;
            }

            public float Damage { get => _damage; }
        }

        ICaster _caster;
        IUpgradeableSkillData _upgradeableSkillData;

        float _adRatio;
        float _lifeTime;

        float _groggyDuration;
        List<ITarget.Type> _targetTypes;
        BaseFactory _weaponFactory;

        public SpawnStickyBombStrategy(
            ICaster caster,
            IUpgradeableSkillData upgradeableSkillData,
            float adRatio,
            float lifeTime,

            float groggyDuration,
            List<ITarget.Type> targetTypes,
            BaseFactory weaponFactory)
        {
            _caster = caster;
            _upgradeableSkillData = upgradeableSkillData;
            _adRatio = adRatio;

            _lifeTime = lifeTime;

            _groggyDuration = groggyDuration;
            _targetTypes = targetTypes;
            _weaponFactory = weaponFactory;
        }

        public void Execute(IFollowable followable, ChangeableData data)
        {
            BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.StickyBomb);
            if (weapon == null) return;

            DamageableData damageData = new DamageableData
            (
                _caster,
                new DamageStat(
                    data.Damage,
                    _upgradeableSkillData.AttackDamage,
                    _adRatio,
                    _upgradeableSkillData.TotalDamageRatio
                ),
                _groggyDuration
            );

            StickyBombDataModifier stickyBombDataModifier = new StickyBombDataModifier(damageData, _targetTypes);

            weapon.ModifyData(stickyBombDataModifier);
            weapon.Activate();
            weapon.ResetPosition(followable.GetPosition());
            weapon.InjectFollower(followable);
        }
    }

    public class SpawnBlackholeStrategy : IActionStrategy
    {
        public struct ChangeableData
        {
            float _damage;
            float _lifetime;
            float _sizeMultiply;

            public ChangeableData(float damage, float lifetime, float sizeMultiply)
            {
                _damage = damage;
                _lifetime = lifetime;
                _sizeMultiply = sizeMultiply;
            }

            public float Damage { get => _damage; }
            public float Lifetime { get => _lifetime; }
            public float SizeMultiply { get => _sizeMultiply; }
        }

        ICaster _caster;
        IUpgradeableSkillData _upgradeableSkillData;

        float _groggyDuration;
        BaseFactory _weaponFactory;
        List<ITarget.Type> _targetTypes;

        public SpawnBlackholeStrategy(
            ICaster caster,
            IUpgradeableSkillData upgradeableSkillData,

            float groggyDuration,
            BaseFactory weaponFactory,
            List<ITarget.Type> targetTypes)
        {
            _caster = caster;
            _upgradeableSkillData = upgradeableSkillData;

            _groggyDuration = groggyDuration;
            _weaponFactory = weaponFactory;
            _targetTypes = targetTypes;
        }

        public void Execute(ChangeableData data)
        {
            BaseWeapon weapon = _weaponFactory.Create(BaseWeapon.Name.Blackhole);
            if (weapon == null) return;

            Transform casterTransform = _caster.GetComponent<Transform>();
            DamageableData damageData = new DamageableData
            (
                _caster,
                new DamageStat(data.Damage),
                _groggyDuration
            );

            BlackholeDataModifier blackholeDataModifier = new BlackholeDataModifier(damageData, data.SizeMultiply, data.Lifetime, _targetTypes);

            weapon.ModifyData(blackholeDataModifier);
            weapon.Activate();
            weapon.ResetPosition(casterTransform.position);
        }
    }

    public class SpreadMissileStrategy : IActionStrategy
    {
        public struct ChangeableData
        {
            float _damage;

            public ChangeableData(float damage)
            {
                _damage = damage;
            }

            public float Damage { get => _damage; set => _damage = value; }
        }

        ICaster _caster;
        IUpgradeableSkillData _upgradeableSkillData;

        float _bulletCount;
        float _adRatio;
        float _groggyDuration;
        float _distanceFromCaster;
        BaseWeapon.Name _bulletName;
        BaseFactory _weaponFactory;
        List<ITarget.Type> _targetTypes;

        public SpreadMissileStrategy(
            ICaster caster,
            IUpgradeableSkillData upgradeableSkillData,
            float bulletCount,
            float adRatio,
            float groggyDuration,
            float distanceFromCaster,
            BaseWeapon.Name bulletName,
            BaseFactory weaponFactory,
            List<ITarget.Type> targetTypes)
        {
            _caster = caster;
            _upgradeableSkillData = upgradeableSkillData;
            _bulletCount = bulletCount;
            _adRatio = adRatio;
            _groggyDuration = groggyDuration;
            _distanceFromCaster = distanceFromCaster;
            _bulletName = bulletName;
            _weaponFactory = weaponFactory;
            _targetTypes = targetTypes;
        }

        public void Execute(ChangeableData data, List<ITarget> targets)
        {
            for (int i = 1; i <= _bulletCount; i++)
            {
                float angle = 360f / _bulletCount * i;
                ShootBullet(angle, data, targets);
            }
        }

        Vector2 GetDirection(float angle)
        {
            float x = Mathf.Sin(angle * Mathf.Deg2Rad);
            float y = Mathf.Cos(angle * Mathf.Deg2Rad);
            return new Vector2(x, y);
        }

        void ShootBullet(float angle, ChangeableData data, List<ITarget> targets)
        {
            Vector3 direction = GetDirection(angle);

            Transform casterTransform = _caster.GetComponent<Transform>();
            Vector3 spawnPosition = casterTransform.position + direction * _distanceFromCaster;

            BaseWeapon weapon = _weaponFactory.Create(_bulletName);
            if (weapon == null) return;

            DamageableData damageData = new DamageableData
            (
                _caster,
                new DamageStat(
                    data.Damage,
                    _upgradeableSkillData.AttackDamage,
                    _adRatio,
                    _upgradeableSkillData.TotalDamageRatio
                ),
                _groggyDuration
            );

            TrackableMissileDataModifier bulletDataModifier = new TrackableMissileDataModifier(damageData, _targetTypes);

            weapon.ModifyData(bulletDataModifier);
            weapon.Activate();

            weapon.ResetPosition(spawnPosition, direction);

            ITrackable projectile = weapon.GetComponent<ITrackable>();
            if (projectile == null) return;

            if (targets == null || targets.Count == 0) return; // 타겟 가져오기
            projectile.InjectTarget(targets[0]);
        }
    }

    public class SpreadBulletStrategy : IActionStrategy
    {
        public struct ChangeableData
        {
            float _damage;
            float _force;

            public ChangeableData(float damage, float force)
            {
                _damage = damage;
                _force = force;
            }

            public float Damage { get => _damage; }
            public float Force { get => _force; }
        }

        ICaster _caster;
        IUpgradeableSkillData _upgradeableSkillData;

        float _bulletCount;
        float _adRatio;
        float _groggyDuration;
        float _distanceFromCaster;
        BaseWeapon.Name _bulletName;
        BaseFactory _weaponFactory;
        List<ITarget.Type> _targetTypes;

        public SpreadBulletStrategy(
            ICaster caster,
            IUpgradeableSkillData upgradeableSkillData,
            float bulletCount,
            float adRatio,
            float groggyDuration,
            float distanceFromCaster,
            BaseWeapon.Name bulletName,
            BaseFactory weaponFactory,
            List<ITarget.Type> targetTypes)
        {
            _caster = caster;
            _upgradeableSkillData = upgradeableSkillData;
            _bulletCount = bulletCount;
            _adRatio = adRatio;
            _groggyDuration = groggyDuration;
            _distanceFromCaster = distanceFromCaster;
            _bulletName = bulletName;
            _weaponFactory = weaponFactory;
            _targetTypes = targetTypes;
        }

        public void Execute(ChangeableData data)
        {
            for (int i = 1; i <= _bulletCount; i++)
            {
                float angle = 360f / _bulletCount * i;
                ShootBullet(angle, data);
            }
        }

        Vector2 GetDirection(float angle)
        {
            float x = Mathf.Sin(angle * Mathf.Deg2Rad);
            float y = Mathf.Cos(angle * Mathf.Deg2Rad);
            return new Vector2(x, y);
        }

        void ShootBullet(float angle, ChangeableData data)
        {
            Vector3 direction = GetDirection(angle);

            Transform casterTransform = _caster.GetComponent<Transform>();
            Vector3 spawnPosition = casterTransform.position + direction * _distanceFromCaster;

            BaseWeapon weapon = _weaponFactory.Create(_bulletName);
            if (weapon == null) return;

            DamageableData damageData = new DamageableData
            (
                _caster,
                new DamageStat(
                    data.Damage,
                    _upgradeableSkillData.AttackDamage,
                    _adRatio,
                    _upgradeableSkillData.TotalDamageRatio
                ),
                _groggyDuration
            );

            BulletDataModifier bulletDataModifier = new BulletDataModifier(damageData, _targetTypes);

            weapon.ModifyData(bulletDataModifier);
            weapon.Activate();

            weapon.ResetPosition(spawnPosition, direction);

            IProjectable projectile = weapon.GetComponent<IProjectable>();
            if (projectile == null) return;

            projectile.Shoot(direction, data.Force);
        }
    }

    public class HitTargetStrategy : IActionStrategy
    {
        public struct ChangeableData
        {
            float _damage;

            public ChangeableData(float damage)
            {
                _damage = damage;
            }

            public float Damage { get => _damage; set => _damage = value; }
        }

        ICaster _caster;
        IUpgradeableSkillData _upgradeableSkillData;
        float _damage;

        float _adRatio;
        float _groggyDuration;

        public HitTargetStrategy(
            ICaster caster,
            IUpgradeableSkillData upgradeableSkillData,
            float adRatio,
            float groggyDuration)
        {
            _caster = caster;
            _upgradeableSkillData = upgradeableSkillData;
            _adRatio = adRatio;
            _groggyDuration = groggyDuration;
        }

        public HitTargetStrategy(
            ICaster caster,
            IUpgradeableSkillData upgradeableSkillData,
            float adRatio)
        {
            _caster = caster;
            _upgradeableSkillData = upgradeableSkillData;
            _adRatio = adRatio;
            _groggyDuration = 0;
        }

        public HitTargetStrategy(
           ICaster caster,
           IUpgradeableSkillData upgradeableSkillData)
        {
            _caster = caster;
            _upgradeableSkillData = upgradeableSkillData;
            _adRatio = 0;
            _groggyDuration = 0;
        }

        public void Execute(List<IDamageable> damageables, ChangeableData data)
        {
            //Debug.Log("Execute");
            if (damageables == null || damageables.Count == 0) return;

            DamageableData damageData = new DamageableData
            (
                _caster,
                new DamageStat(
                    data.Damage,
                    _upgradeableSkillData.AttackDamage,
                    _adRatio,
                    _upgradeableSkillData.TotalDamageRatio
                ),
                _groggyDuration
            );

            for (int i = 0; i < damageables.Count; i++)
            {
                damageables[i].GetDamage(damageData);
            }
        }

        public void Execute(IDamageable damageable, ChangeableData data)
        {
            //Debug.Log("Execute");

            DamageableData damageData = new DamageableData
            (
                _caster,
                new DamageStat(
                    data.Damage,
                    _upgradeableSkillData.AttackDamage,
                    _adRatio,
                    _upgradeableSkillData.TotalDamageRatio
                ),
                _groggyDuration
            );


            damageable.GetDamage(damageData);
        }
    }
}