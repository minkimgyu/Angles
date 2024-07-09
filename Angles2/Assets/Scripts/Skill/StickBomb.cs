using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageUtility;
using System;

public class StickBomb : ActiveSkill
{
    float _damage;
    float _range;
    float _explosionDelay;
    List<ITarget.Type> _targetType;
    List<Tuple<float, IAttachable>> _targetDatas;

    public StickBomb(float probability, float damage, float range, float explosionDelay, List<ITarget.Type> damageableTypes)
    {
        _targetDatas = new List<Tuple<float, IAttachable>>();
        _probability = probability;

        _damage = damage;
        _range = range;
        _explosionDelay = explosionDelay;
        _targetType = damageableTypes;
    }

    public override void OnUpdate()
    {
        if (_targetDatas.Count == 0) return;

        for (int i = 0; i < _targetDatas.Count; i++)
        {
            float duration = Time.time - _targetDatas[i].Item1;
            if (duration >= _explosionDelay)
            {
                Vector3 pos = _targetDatas[i].Item2.ReturnPosition();
                Explode(pos);

                _targetDatas.RemoveAt(i);
                i--;
            }
        }
    }

    void Explode(Vector3 targetPos)
    {
        Debug.Log("Explode");

        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Explosion);
        effect.ResetPosition(targetPos);
        effect.Play();

        DamageData damageData = new DamageData(_damage, _targetType);
        Damage.HitCircleRange(damageData, targetPos, _range, true, Color.red, 3);
    }

    public override void OnReflect(Collision2D collision)
    {
        IAttachable attachable = collision.gameObject.GetComponent<IAttachable>();
        if (attachable == null) return;

        Debug.Log("Attach");

        BaseEffect effect = EffectFactory.Create(BaseEffect.Name.Signal);
        effect.ResetPosition(attachable);
        effect.ResetDestoryDelay(_explosionDelay);

        effect.Play();

        Tuple<float, IAttachable> targetData = new Tuple<float, IAttachable>(Time.time, attachable);
        _targetDatas.Add(targetData);
    }
}
