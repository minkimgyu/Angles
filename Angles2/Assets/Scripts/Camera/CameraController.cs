using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] Rect _cameraArea;

    [SerializeField] Vector2 _mapPos;
    [SerializeField] Vector2 _mapSize;

    Vector2 MapMinPos { get { return _mapPos - _mapSize / 2; } }
    Vector2 MapMaxPos { get { return _mapPos + _mapSize / 2; } }

    [SerializeField] float _fieldOfView = 70;
    [SerializeField] float _followSpeed = 1;
    Camera _mainCamera;

    IPos _followTarget;

    public void Initialize()
    {
        _mainCamera = Camera.main;
        _mainCamera.fieldOfView = _fieldOfView;
        _mainCamera.transform.position = new Vector3(_cameraArea.position.x, _cameraArea.position.y, -10);

        AreaReflecter areaReflecter = GetComponentInChildren<AreaReflecter>();
        areaReflecter.Initialize(_mapSize.x, _mapSize.y, 3);
    }

    public void SetFollower(IPos followTarget)
    {
        _followTarget = followTarget;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_followTarget == null) return;
        Vector3 targetPos = _followTarget.ReturnPosition();

        float xPos = Mathf.Clamp(targetPos.x, MapMinPos.x + _cameraArea.width / 2, MapMaxPos.x - _cameraArea.width / 2);
        float yPos = Mathf.Clamp(targetPos.y, MapMinPos.y + _cameraArea.height / 2, MapMaxPos.y - _cameraArea.height / 2);
        targetPos.Set(xPos, yPos, -10);

        Vector2 pos = Vector2.Lerp(_mainCamera.transform.position, targetPos, Time.deltaTime * _followSpeed);
        _mainCamera.transform.position = new Vector3(pos.x, pos.y, _mainCamera.transform.position.z);
    }
}
