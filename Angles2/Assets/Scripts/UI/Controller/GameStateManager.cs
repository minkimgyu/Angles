using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ���� ������ �����ϴ� Ŭ����
public class GameState
{
    BaseViewer _coinViewer;
    BaseViewer _enemyDieViewer;

    public GameState(BaseViewer coinViewer, BaseViewer enemyDieViewer)
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

// ���� ������ �����Ͽ� �����ϴ� ���� �ƴ�
// �̱����� ���ؼ� GameState ����

// ���� ������ ��Ʈ�ѷ�
public class GameStateManager : Singleton<GameStateManager>
{
    GameState _gameState;

    public void Initialize(GameState gameState)
    {
        _gameState = gameState;
    }

    public void ChangeCoin(int coin)
    {
        _gameState.CoinCount += coin;
    }

    public int ReturnCoin()
    {
        return _gameState.CoinCount;
    }

    public void ChangeEnemyDieCount(int dieCount)
    {
        _gameState.EnemyDieCount += dieCount;
    }

    public int ReturnEnemyDieCount()
    {
        return _gameState.EnemyDieCount;
    }
}