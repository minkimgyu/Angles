using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : BaseInputHandler
{
    enum KeyState
    {
        Up,
        Down,
    }

    KeyState _state = KeyState.Up;
    Vector2 _direction = Vector2.zero;

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _direction.Set(x, y);
        _direction = _direction.normalized;

        if (x != 0 || y != 0)
        {
            switch (_state)
            {
                case KeyState.Up:
                    _state = KeyState.Down;
                    OnInputStartRequested?.Invoke();
                    OnInputRequested?.Invoke(_direction);
                    break;
                case KeyState.Down:
                    OnInputRequested?.Invoke(_direction);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (_state)
            {
                case KeyState.Down:
                    _state = KeyState.Up;
                    OnInputEndRequested?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
}
