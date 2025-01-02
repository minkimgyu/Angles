using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 인 게임 정보를 저장하는 클레스
// Chapter, Survival 둘로 나누기

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

// 직접 변수에 접근하여 수정하는 것이 아닌
// 싱글톤을 통해서 GameState 변경

// 게임 데이터 컨트롤러
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