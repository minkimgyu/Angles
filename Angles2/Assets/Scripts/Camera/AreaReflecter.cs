using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaReflecter : MonoBehaviour
{
    float _width;
    float _height;
    float _size;

    [SerializeField] BoxCollider2D _up;
    [SerializeField] BoxCollider2D _down;
    [SerializeField] BoxCollider2D _left;
    [SerializeField] BoxCollider2D _right;

    public void Initialize(float width, float height, float size)
    {
        _width = width;
        _height = height;
        _size = size;

        Vector2 verticalSize = new Vector2(_width, _size);
        Vector2 horizontalSize = new Vector2(_size, _height);

        _up.transform.localPosition = new Vector3(0, _height / 2 + _size / 2, 0);
        _up.size = verticalSize;

        _down.transform.localPosition = new Vector3(0, -_height / 2 - _size / 2, 0);
        _down.size = verticalSize;

        _left.transform.localPosition = new Vector3(_width / 2 + _size / 2, 0, 0);
        _left.size = horizontalSize;

        _right.transform.localPosition = new Vector3(-_width / 2 - _size / 2, 0, 0);
        _right.size = horizontalSize;
    }
}
