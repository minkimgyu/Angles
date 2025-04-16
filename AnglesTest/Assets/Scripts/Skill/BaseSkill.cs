using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Skill.Strategy;

// 무기는 Visitor 제거해주기
// PlayerData를 FSM 내부에서 캐싱해서 사용
// 

namespace Skill
{
    abstract public class BaseSkill : ISkillUpgradable
    {
        public enum Name
        {
            Statikk,
            Knockback,
            Impact,
            ContactAttack,

            SpawnRifleShooter, // weapon
            SpawnRocketShooter, // weapon

            SpawnBlackhole, // weapon
            SpawnBlade, // projectile
            SpawnStickyBomb, // projectile

            UpgradeShooting,
            UpgradeDamage,
            UpgradeCooltime,

            SpreadBullets,
            SpreadReflectableBullets,
            SpreadTrackableMissiles,

            RushAttack,

            ShootMultipleLaser,
            ShootFewLaser,

            MultipleShockwave,
            Shockwave,
            MagneticField,
            SelfDestruction,

            SpreadMultipleBullets,
            ReviveImpact,
        }

        public enum Type
        {
            Basic,
            Active,
            Passive
        }

        public BaseSkill(Type skillType, int maxUpgradePoint)
        {
            _skillType = skillType;
            _maxUpgradePoint = maxUpgradePoint;
            _upgradePoint = 1;
            // 0, 1, 2, 3, 4 까지 도달
            // 0, 1, 2
        }

        protected IUpgradeVisitor _upgrader;

        protected IUseConstraintStrategy _useConstraintStrategy;
        protected ITargetStrategy _targetingStrategy;
        protected IDetectStrategy _detectingStrategy;

        protected IActionStrategy _actionStrategy;
        protected IRoutineStrategy _delayStrategy;

        protected IEffectStrategy _effectStrategy;
        protected ISoundStrategy _soundStrategy;

        protected Type _skillType;
        public Type SkillType { get { return _skillType; } }

        protected ICaster _caster;
        protected IUpgradeableSkillData _upgradeableRatio;

        int _maxUpgradePoint;
        public int MaxUpgradePoint { get { return _maxUpgradePoint; } }

        int _upgradePoint;
        public int UpgradePoint { get { return _upgradePoint; } }

        //public virtual void Upgrade(int step)
        //{
        //    for (int i = 0; i < step; i++) Upgrade();
        //}

        public virtual void Upgrade()
        {
            _upgradePoint++;
            _actionStrategy.OnUpgrade();
        }

        public virtual void Initialize(IUpgradeableSkillData upgradeableRatio, ICaster caster)
        {
            _upgradeableRatio = upgradeableRatio;
            _caster = caster;

            _useConstraintStrategy = new NoConstraintStrategy();
            _targetingStrategy = new Skill.Strategy.NoTargetingStrategy();
            _detectingStrategy = new Strategy.NoDetectingStrategy();
            _actionStrategy = new NoActionStrategy();
            _delayStrategy = new NoDelayStrategy();
            _effectStrategy = new NoEffectStrategy();
            _soundStrategy = new NoSoundStrategy();
        }

        public void AddViewEvent(Action<float, int, bool> viewEvent)
        {
            _useConstraintStrategy.AddViewEvent(viewEvent);
        }

        public void RemoveViewEvent(Action<float, int, bool> viewEvent)
        {
            _useConstraintStrategy.RemoveViewEvent(viewEvent);
        }

        public bool CanUse()
        {
            return _useConstraintStrategy.CanUse();
        }

        public void Use()
        {
            _useConstraintStrategy.Use();
        }

        public virtual void OnAdd()
        {
            _actionStrategy.OnAdd();
        }

        public virtual bool OnReflect(GameObject targetObject, Vector2 contactPos, Vector2 contactNormal)
        {
            //List<IDamageable> damageables = _targetingStrategy.TargetDamageables(collision);
            //if (damageables.Count == 0) return false; // 타겟이 없는 경우

            //_actionStrategy.HitTargets(damageables, collision);
            ////_effectStrategy.SpawnEffect(collision.contacts[0].point);
            //_soundStrategy.PlaySound();

            // 자식 객체에서 추가 구현해보기

            return true; // 타겟이 존재하는 경우
        }

        public virtual void OnDamaged(float ratio)
        {
            _delayStrategy.OnDamaged(ratio);
        }

        public virtual void OnRevive()
        {
            _actionStrategy.OnRevive();
        }

        public virtual void OnCaptureEnter(ITarget target)
        {
            _detectingStrategy.OnTargetEnter(target);
        }

        public virtual void OnCaptureExit(ITarget target)
        {
            _detectingStrategy.OnTargetExit(target);
        }

        public virtual void OnCaptureEnter(ITarget target, IDamageable damageable)
        {
            _detectingStrategy.OnTargetEnter(target, damageable);
        }

        public virtual void OnCaptureExit(ITarget target, IDamageable damageable)
        {
            _detectingStrategy.OnTargetExit(target, damageable);
        }

        public virtual void OnUpdate()
        {
            _useConstraintStrategy.OnUpdate();
            _delayStrategy.OnUpdate();
        }
    }
}