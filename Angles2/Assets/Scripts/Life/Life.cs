using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Life : MonoBehaviour, IDamageable
{
    [SerializeField] float _maxHp;
    [SerializeField] float _hp;

    public enum LifeState
    {
        Alive,
        Die
    }

    protected LifeState _lifeState = LifeState.Alive;

    protected abstract void OnDie();

    public virtual void GetHeal(float point)
    {
        _hp += point;
        if (_maxHp < _hp)
        {
            _hp = _maxHp;
        }
    }

    public virtual void GetDamage(float damage)
    {
        _hp -= damage;
        if (_hp < 0)
        {
            _hp = 0;
            _lifeState = LifeState.Die;
            OnDie();
        }
    }
}
