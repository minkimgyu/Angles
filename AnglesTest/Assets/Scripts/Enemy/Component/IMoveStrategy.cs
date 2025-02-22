using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveStrategy
{
    void OnUpdate() { }
    void OnFixedUpdate() { }
    void OnCollision(Collision2D collision) { }

    void ApplyForce(Vector3 direction, float force, ForceMode2D mode) {  }
    void InjectTarget(ITarget target) {  }
    void InjectPathfindEvent(Func<Vector2, Vector2, BaseLife.Size, List<Vector2>> FindPath) {  }
}
