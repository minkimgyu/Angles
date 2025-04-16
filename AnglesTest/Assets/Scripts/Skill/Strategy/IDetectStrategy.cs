using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill.Strategy
{
    // �ֺ� ���� �����ؼ� ������ ������
    public interface IDetectStrategy
    {
        List<IDamageable> DetectDamageables() { return default; }
        List<ITarget> DetectTargets() { return default; }

        void OnTargetEnter(ITarget target) { }
        void OnTargetExit(ITarget target) { }

        void OnTargetEnter(ITarget target, IDamageable damageable) { }
        void OnTargetExit(ITarget target, IDamageable damageable) { }
    }

    public class NoDetectingStrategy : IDetectStrategy
    {
    }

    public class TargetDetectingStrategy : IDetectStrategy
    {
        List<ITarget.Type> _targetTypes;
        List<ITarget> _targetDatas;

        public TargetDetectingStrategy(List<ITarget.Type> targetTypes)
        {
            _targetDatas = new List<ITarget>();
            _targetTypes = targetTypes;
        }

        public List<ITarget> DetectTargets() { return _targetDatas; }

        public void OnTargetEnter(ITarget target)
        {
            // Ȯ���� Attack Ŭ�������� ����
            if (target.IsTarget(_targetTypes) == false) return;
            _targetDatas.Add(target);
        }

        public void OnTargetExit(ITarget target)
        {
            // Ȯ���� Attack Ŭ�������� ����
            if (target.IsTarget(_targetTypes) == false) return;
            _targetDatas.Remove(target);
        }
    }

    public class DamageableDetectingStrategy : IDetectStrategy
    {
        List<IDamageable> _damageables;
        List<ITarget.Type> _targetTypes;

        public DamageableDetectingStrategy(List<ITarget.Type> targetTypes)
        {
            _damageables = new List<IDamageable>();
            _targetTypes = targetTypes;
        }

        public List<IDamageable> DetectDamageables() { return _damageables; }

        public void OnTargetEnter(ITarget target, IDamageable damageable)
        {
            // Ȯ���� Attack Ŭ�������� ����
            if (target.IsTarget(_targetTypes) == false) return;
            _damageables.Add(damageable);
        }

        public void OnTargetExit(ITarget target, IDamageable damageable)
        {
            IDamageable findDamageable = _damageables.Find(x => x == damageable);
            _damageables.Remove(findDamageable);
        }
    }
}