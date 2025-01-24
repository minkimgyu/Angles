using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using System;

public interface IGPGS
{
    void Login(Action<bool> OnLoginComplete);
    void Save(Action<bool> OnSaveComplete);
    void Load(Action<bool, string> OnLoadComplete);
    //void ShowSelectUI();
}

public class NullGPGSManager : IGPGS
{
    public void Login(Action<bool> OnLoginComplete) { }
    public void Save(Action<bool> OnSaveComplete) { }
    public void Load(Action<bool, string> OnLoadComplete) { }
    //public void ShowSelectUI() { }
}

public class GPGSManager : IGPGS
{
    private const string fileName = "file.dat";

    Action<bool> OnLoginComplete;
    Action<bool> OnSaveComplete;
    Action<bool, string> OnLoadComplete;

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

    //public void ShowSelectUI()
    //{
    //    uint maxNumToDisplay = 5;
    //    bool allowCreateNew = false;
    //    bool allowDelete = true;

    //    ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
    //    savedGameClient.ShowSelectSavedGameUI("Select saved game",
    //        maxNumToDisplay,
    //        allowCreateNew,
    //        allowDelete,
    //        OnSavedGameSelected);
    //}


    //public void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    //{
    //    if (status == SelectUIStatus.SavedGameSelected)
    //    {
    //        // handle selected game save
    //    }
    //    else
    //    {
    //        // handle cancel or error
    //    }
    //}

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
            DataSource.ReadNetworkOnly,
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

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder()
                .WithUpdatedPlayedTime(TimeSpan.Zero)
                .WithUpdatedDescription("Saved game at " + DateTime.Now);

            string jData = ServiceLocater.ReturnSaveManager().GetSaveJsonData();

            Debug.Log(jData);

            byte[] bytes = Encoding.UTF8.GetBytes(jData);

            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            savedGameClient.CommitUpdate(game, updatedMetadata, bytes, OnSavedGameWritten);
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

    public void Load(Action<bool, string> OnLoadComplete)
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
            DataSource.ReadNetworkOnly,
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
            OnLoadComplete?.Invoke(false, default);
            OnLoadComplete = null;
            Debug.Log("로드 실패");
        }
    }

    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] loadedData)
    {
        if (loadedData.Length == 0)
        {
            Debug.Log("데이터가 없음 초기 데이터 저장");
            OnLoadComplete?.Invoke(false, default);
            OnLoadComplete = null;
        }
        else
        {
            string jData = Encoding.UTF8.GetString(loadedData);
            Debug.Log("로드 데이터 : " + jData);
            OnLoadComplete?.Invoke(true, jData);
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
            DataSource.ReadNetworkOnly,
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