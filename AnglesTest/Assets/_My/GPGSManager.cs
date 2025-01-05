using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using System;

public class GPGSManager
{
    private const string fileName = "file.dat";

    Action<bool> OnLoginComplete;
    Action<bool> OnSaveComplete;
    Action<bool> OnLoadComplete;
    Action<bool> OnComformComplete;

    #region 로그인

    public void Login(Action<bool> OnLoginComplete)
    {
        this.OnLoginComplete = OnLoginComplete;
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
            string userID = PlayGamesPlatform.Instance.GetUserId();

            Debug.Log(displayName);
            Debug.Log(userID);
            Debug.Log("로그인 성공");
            OnLoginComplete?.Invoke(true);
        }
        else
        {
            Debug.Log("로그인 실패");
            OnLoginComplete?.Invoke(false);
        }

        OnLoginComplete = null;
    }

    #endregion

    #region 세이브

    public void Save(Action<bool> OnSaveComplete)
    {
        this.OnSaveComplete = OnSaveComplete;
        OpenSaveGame();
    }

    void OpenSaveGame()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution
        (
            fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            OnSavedGameOpened
        );
    }

    void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        if (status == SavedGameRequestStatus.Success)
        {
            // 세이브 적용
            Debug.Log("저장 성공");

            var update = new SavedGameMetadataUpdate.Builder().Build();
            string jData = ServiceLocater.ReturnSaveManager().GetSaveJsonData();

            Debug.Log(jData);

            byte[] bytes = Encoding.UTF8.GetBytes(jData);
            savedGameClient.CommitUpdate(game, update, bytes, OnSavedGameWritten);
        }
        else
        {
            // 실패함
            OnSaveComplete?.Invoke(false);
            OnSaveComplete = null;
            Debug.Log("저장 실패");
        }
    }

    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("저장 최종 성공");
            OnSaveComplete?.Invoke(true);
        }
        else
        {
            // 실패함
            Debug.Log("저장 최종 실패");
            OnSaveComplete?.Invoke(false);
        }

        OnSaveComplete = null;
    }

    #endregion

    #region 불러오기

    public void Load(Action<bool> OnLoadComplete)
    {
        this.OnLoadComplete = OnLoadComplete;
        OpenLoadGame();
    }

    void OpenLoadGame()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution
        (
            fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            LoadGameData
        );
    }

    private void LoadGameData(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("로드 성공");
            savedGameClient.ReadBinaryData(metadata, OnSavedGameDataRead);
        }
        else
        {
            // 실패함
            OnLoadComplete?.Invoke(false);
            OnLoadComplete = null;
            Debug.Log("로드 실패");
        }
    }

    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] loadedData)
    {
        string jData = Encoding.UTF8.GetString(loadedData);

        if (jData == "")
        {
            Debug.Log("데이터가 없음 초기 데이터 저장");
            Save(OnLoadComplete); // OnLoadComplete 콜백 넘겨서 완료 이벤트 받기
        }
        else
        {
            
            Debug.Log("로드 데이터 : " + jData);

            // 이후 세이브 된 데이터를 다시 불러온다.
            ServiceLocater.ReturnSaveManager().Save(jData);
            ServiceLocater.ReturnSaveManager().Load();
            OnLoadComplete?.Invoke(true);
            OnLoadComplete = null;
        }
    }

    #endregion

    #region 삭제하기

    public void Delete()
    {
        DeleteGameData();
    }

    private void DeleteGameData()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution
        (
            fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            DeleteSaveGame
        );
    }

    private void DeleteSaveGame(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("삭제 성공");
            savedGameClient.Delete(metadata);
        }
        else
        {
            // 실패함
            Debug.Log("삭제 실패");
        }
    }

    #endregion
}