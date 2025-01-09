using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowPointerController : MonoBehaviour
{
    List<ITarget> _targets;
    List<ArrowPointViewer> _arrorViewers;

    [SerializeField] Transform _parent;
    InGameFactory _inGameFactory;
    Canvas _canvas; // UI가 위치한 캔버스
    const float _edgeOffset = 80f; // 화면 모서리와의 여백

    // 화면 중심과 타겟 방향 계산
    Vector2 _screenCenter;
    // 모서리를 따라 화살표 위치 계산
    float _canvasWidth;
    float _canvasHeight;
    float _halfWidth;
    float _halfHeight;

    public void Initialize(InGameFactory inGameFactory)
    {
        _inGameFactory = inGameFactory;
        _targets = new List<ITarget>();
        _arrorViewers = new List<ArrowPointViewer>();

        _canvas = _parent.GetComponent<Canvas>();

        // 화면 중심과 타겟 방향 계산
        _screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        // 모서리를 따라 화살표 위치 계산
        _canvasWidth = _canvas.pixelRect.width;
        _canvasHeight = _canvas.pixelRect.height;
        _halfWidth = _canvasWidth / 2 - _edgeOffset;
        _halfHeight = _canvasHeight / 2 - _edgeOffset;
    }

    public void AddTarget(ITarget target)
    {
        _targets.Add(target);

         ArrowPointViewer arrowViewer = 
            (ArrowPointViewer)_inGameFactory.GetFactory(InGameFactory.Type.Viewer).
            Create(BaseViewer.Name.ArrowPointViewer);

        arrowViewer.transform.SetParent(_parent);
        arrowViewer.Initialize();
        _arrorViewers.Add(arrowViewer);
    }

    private void Update()
    {
        int targetCount = _targets.Count;
        for (int i = _targets.Count - 1; i >= 0; i--)
        {
            try
            {
                if (_targets[i] as UnityEngine.Object == null)
                {
                    _targets.RemoveAt(i);

                    GameObject viewer = _arrorViewers[i].gameObject;
                    _arrorViewers.RemoveAt(i);
                    Destroy(viewer);
                    continue;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                throw;
            }

            Vector3 screenPoint = Camera.main.WorldToScreenPoint(_targets[i].GetPosition());
            // 타겟이 화면 밖에 있는 경우
            if (screenPoint.z < 0 || screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 || screenPoint.y > Screen.height)
            {
                // 타겟 방향 계산
                Vector2 direction = ((Vector2)screenPoint - _screenCenter).normalized;

                // 화면 모서리 교차점 계산
                Vector2 edgePosition = Vector2.zero;
                float slope = direction.y / direction.x;

                if (Mathf.Abs(slope) > _halfHeight / _halfWidth) // 위/아래에 닿는 경우
                {
                    edgePosition.y = Mathf.Sign(direction.y) * _halfHeight;
                    edgePosition.x = edgePosition.y / slope;
                }
                else // 좌/우에 닿는 경우
                {
                    edgePosition.x = Mathf.Sign(direction.x) * _halfWidth;
                    edgePosition.y = edgePosition.x * slope;
                }

                // 화살표 활성화
                // 화살표 UI 위치 및 회전 설정 
                _arrorViewers[i].UpdatePosition(edgePosition, direction);
                _arrorViewers[i].TurnOnViewer(true);
            }
            else
            {
                // 타겟이 화면 안에 있으면 화살표 숨기기
                _arrorViewers[i].TurnOnViewer(false);
            }
        }
    }
}
