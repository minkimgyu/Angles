using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인 게임 정보를 저장하는 클레스
public class DungeonGameState
{
    BaseViewer _coinViewer;
    BaseViewer _enemyDieViewer;

    public DungeonGameState(BaseViewer coinViewer, BaseViewer enemyDieViewer)
    {
        _coinViewer = coinViewer;
        _enemyDieViewer = enemyDieViewer;

        CoinCount = 0;
        EnemyDieCount = 0;
    }

    int _coinCount;
    public int CoinCount
    {
        get => _coinCount;
        set
        {
            _coinCount = value;
            _coinViewer.UpdateViewer(_coinCount);
        }
    }

    int _enemyDieCount;
    public int EnemyDieCount
    {
        get => _enemyDieCount;
        set
        {
            _enemyDieCount = value;
            _enemyDieViewer.UpdateViewer(_enemyDieCount);
        }
    }
}

// 게임 데이터 컨트롤러
public class DungeonGameStateController : MonoBehaviour
{
    [SerializeField] BaseViewer _coinViewer;
    [SerializeField] BaseViewer _enemyDieViewer;

    DungeonGameState _gameState;

    public void Initialize()
    {
        _gameState = new DungeonGameState(_coinViewer, _enemyDieViewer);
        GameStateEventBus.Register(GameStateEventBus.State.ChangeCoin, new ChangeValueCommand(ChangeCoin));
        GameStateEventBus.Register(GameStateEventBus.State.ReturnCoin, new ReturnValueCommand(ReturnCoin));

        GameStateEventBus.Register(GameStateEventBus.State.ChangeEnemyDieCount, new ChangeValueCommand(AddEnemyDieCount));
        GameStateEventBus.Register(GameStateEventBus.State.ReturnEnemyDieCount, new ReturnValueCommand(ReturnEnemyDieCount));
    }

    public void ChangeCoin(int coin)
    {
        _gameState.CoinCount += coin;
    }

    public int ReturnCoin()
    {
        return _gameState.CoinCount;
    }

    public void AddEnemyDieCount(int dieCount)
    {
        _gameState.EnemyDieCount += dieCount;
    }

    public int ReturnEnemyDieCount()
    {
        return _gameState.EnemyDieCount;
    }
}