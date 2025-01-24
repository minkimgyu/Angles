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

    #region �α���

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
            Debug.Log("�α��� ����");
            OnLoginComplete?.Invoke(true);
        }
        else
        {
            Debug.Log("�α��� ����");
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

    #region ���̺�

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
            // ���̺� ����
            Debug.Log("���� ����");

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
            // ������
            OnSaveComplete?.Invoke(false);
            OnSaveComplete = null;
            Debug.Log("���� ����");
        }
    }

    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("���� ���� ����");
            OnSaveComplete?.Invoke(true);
        }
        else
        {
            // ������
            Debug.Log("���� ���� ����");
            OnSaveComplete?.Invoke(false);
        }

        OnSaveComplete = null;
    }

    #endregion

    #region �ҷ�����

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
            Debug.Log("�ε� ����");
            savedGameClient.ReadBinaryData(metadata, OnSavedGameDataRead);
        }
        else
        {
            // ������
            OnLoadComplete?.Invoke(false, default);
            OnLoadComplete = null;
            Debug.Log("�ε� ����");
        }
    }

    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] loadedData)
    {
        if (loadedData.Length == 0)
        {
            Debug.Log("�����Ͱ� ���� �ʱ� ������ ����");
            OnLoadComplete?.Invoke(false, default);
            OnLoadComplete = null;
        }
        else
        {
            string jData = Encoding.UTF8.GetString(loadedData);
            Debug.Log("�ε� ������ : " + jData);
            OnLoadComplete?.Invoke(true, jData);
            OnLoadComplete = null;
        }
    }

    #endregion

    #region �����ϱ�

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
            Debug.Log("���� ����");
            savedGameClient.Delete(metadata);
        }
        else
        {
            // ������
            Debug.Log("���� ����");
        }
    }

    #endregion
}