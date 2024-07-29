using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillBubble : MonoBehaviour, IInteractable
{
    TrackComponent _trackComponent;
    [SerializeField] float _moveSpeed = 8f;
    [SerializeField] BaseSkill.Name _skillName;

    private void Start()
    {
        _trackComponent = GetComponent<TrackComponent>();
        _trackComponent.Initialize(_moveSpeed);
    }

    public void OnInteractEnter(InteractEnterData data)
    {
        _trackComponent.ResetFollower(data.Followable);
    }

    public void OnInteract(InteractData data) 
    {
        Action<BaseSkill.Name> AddSkill = data.AddSkill;
        AddSkill?.Invoke(_skillName);
        Destroy(gameObject);
    }

    public void OnInteractExit(InteractExitData data) { }

    public UnityEngine.Object ReturnObject()
    {
        return this;
    }
}
