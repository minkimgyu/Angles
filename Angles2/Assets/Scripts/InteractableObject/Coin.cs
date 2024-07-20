using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour, IInteractable
{
    [SerializeField] int _upCount = 1;
    [SerializeField] float _minDistance = 0.1f;
    [SerializeField] float _moveSpeed = 8f;

    bool _nowEnter;
    IFollowable _followableTarget;
    MoveComponent _moveComponent;

    Vector2 _followDir;

    private void Start()
    {
        _nowEnter = false;
        _moveComponent = GetComponent<MoveComponent>();
        _moveComponent.Initialize();
    }

    bool NowInRange()
    {
        return Vector3.Distance(transform.position, _followableTarget.ReturnPosition()) <= _minDistance;
    }

    private void Update()
    {
        if (_nowEnter == false) return;

        if (_followableTarget == null || _followableTarget.CanFollow() == false)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPos = _followableTarget.ReturnPosition();
        _followDir = (targetPos - transform.position).normalized;

        bool nowInRange = NowInRange();
        if (nowInRange == true)
        {
            StageManager.Instance.CoinCount += _upCount;
            Destroy(gameObject);
            return;
        }
    }

    private void FixedUpdate()
    {
        if (_nowEnter == false) return;
        if (_followableTarget == null || _followableTarget.CanFollow() == false) return;

        _moveComponent.Move(_followDir, _moveSpeed);
    }

    public void OnInteractEnter(IFollowable followable)
    {
        _followableTarget = followable;
        _nowEnter = true;
    }

    public void OnInteractEnter() { }
    public void OnInteract(List<SkillUpgradeData> skillDatas) { }
    public void OnInteractExit() { }
}
