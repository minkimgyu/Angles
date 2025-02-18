using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveStrategy
{
    void OnUpdate() { }
    void OnFixedUpdate() { }
    void OnCollision(Collision2D collision) { }

    void AddTarget(ITarget target) {  }
}
