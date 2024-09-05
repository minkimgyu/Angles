using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TestSystem에서는 다룰 게임 데이터가 다를 수 있기 때문에 추상 클레스를 활용하여 데이터를 변경한다.
abstract public class BaseGameState : MonoBehaviour
{
    public virtual void AddCoin(int coin) { }
    public virtual int ReturnCoin() { return default; }

    public virtual void AddEnemyDieCount(int dieCount) { }
}