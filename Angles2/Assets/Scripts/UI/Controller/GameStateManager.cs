using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인 게임 정보를 저장하는 클레스
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

// 직접 변수에 접근하여 수정하는 것이 아닌
// 싱글톤을 통해서 GameState 변경

// 게임 데이터 컨트롤러
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