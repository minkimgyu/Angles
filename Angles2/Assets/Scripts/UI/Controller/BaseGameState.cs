using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TestSystem������ �ٷ� ���� �����Ͱ� �ٸ� �� �ֱ� ������ �߻� Ŭ������ Ȱ���Ͽ� �����͸� �����Ѵ�.
abstract public class BaseGameState : MonoBehaviour
{
    public virtual void AddCoin(int coin) { }
    public virtual int ReturnCoin() { return default; }

    public virtual void AddEnemyDieCount(int dieCount) { }
}