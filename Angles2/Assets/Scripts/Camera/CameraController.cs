using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _fieldOfView = 70;
    [SerializeField] float _followSpeed = 1;
    Camera _mainCamera;

    Func<Vector3> ReturnTargetPos;

    public void Initialize()
    {
        _mainCamera = Camera.main;
        _mainCamera.fieldOfView = _fieldOfView;
    }

    public void OnFollowRequested(Func<Vector3> ReturnTargetPos)
    {
        this.ReturnTargetPos = ReturnTargetPos;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = ReturnTargetPos();

        Vector2 pos = Vector2.Lerp(_mainCamera.transform.position, targetPos, Time.deltaTime * _followSpeed);
        _mainCamera.transform.position = new Vector3(pos.x, pos.y, _mainCamera.transform.position.z);
    }
}
