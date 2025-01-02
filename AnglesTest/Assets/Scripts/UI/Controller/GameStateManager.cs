using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// �� ���� ������ �����ϴ� Ŭ����
// Chapter, Survival �ѷ� ������

public class GameState
{
    CoinViewer _coinViewer;

    public GameState(CoinViewer coinViewer)
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
            _coinViewer.UpdateCoinCount(_coinCount);
        }
    }
}

// ���� ������ �����Ͽ� �����ϴ� ���� �ƴ�
// �̱����� ���ؼ� GameState ����

// ���� ������ ��Ʈ�ѷ�
public class GameStateManager : Singleton<GameStateManager>
{
    GameState _gameState;
    Action<int> OnGetCoin;

    public void Initialize(GameState gameState, Action<int> OnGetCoin = null)
    {
        _gameState = gameState;
        this.OnGetCoin = OnGetCoin;
    }

    public void ChangeCoin(int coin)
    {
        _gameState.CoinCount += coin;
        if(coin > 0) OnGetCoin?.Invoke(coin);
    }

    public int ReturnCoin()
    {
        return _gameState.CoinCount;
    }
}