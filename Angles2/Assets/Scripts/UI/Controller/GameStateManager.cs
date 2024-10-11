using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ���� ������ �����ϴ� Ŭ����
public class GameState
{
    BaseViewer _coinViewer;

    public GameState(BaseViewer coinViewer)
    {
        _coinViewer = coinViewer;
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
}