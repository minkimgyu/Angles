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
    Canvas _canvas; // UI�� ��ġ�� ĵ����
    const float _edgeOffset = 80f; // ȭ�� �𼭸����� ����

    // ȭ�� �߽ɰ� Ÿ�� ���� ���
    Vector2 _screenCenter;
    // �𼭸��� ���� ȭ��ǥ ��ġ ���
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

        // ȭ�� �߽ɰ� Ÿ�� ���� ���
        _screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        // �𼭸��� ���� ȭ��ǥ ��ġ ���
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
            // Ÿ���� ȭ�� �ۿ� �ִ� ���
            if (screenPoint.z < 0 || screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 || screenPoint.y > Screen.height)
            {
                // Ÿ�� ���� ���
                Vector2 direction = ((Vector2)screenPoint - _screenCenter).normalized;

                // ȭ�� �𼭸� ������ ���
                Vector2 edgePosition = Vector2.zero;
                float slope = direction.y / direction.x;

                if (Mathf.Abs(slope) > _halfHeight / _halfWidth) // ��/�Ʒ��� ��� ���
                {
                    edgePosition.y = Mathf.Sign(direction.y) * _halfHeight;
                    edgePosition.x = edgePosition.y / slope;
                }
                else // ��/�쿡 ��� ���
                {
                    edgePosition.x = Mathf.Sign(direction.x) * _halfWidth;
                    edgePosition.y = edgePosition.x * slope;
                }

                // ȭ��ǥ Ȱ��ȭ
                // ȭ��ǥ UI ��ġ �� ȸ�� ���� 
                _arrorViewers[i].UpdatePosition(edgePosition, direction);
                _arrorViewers[i].TurnOnViewer(true);
            }
            else
            {
                // Ÿ���� ȭ�� �ȿ� ������ ȭ��ǥ �����
                _arrorViewers[i].TurnOnViewer(false);
            }
        }
    }
}
