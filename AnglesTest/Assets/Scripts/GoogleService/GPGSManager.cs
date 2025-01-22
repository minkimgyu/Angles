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
            // ���̺� ����
            Debug.Log("���� ����");

            var update = new SavedGameMetadataUpdate.Builder().Build();
            string jData = ServiceLocater.ReturnSaveManager().GetSaveJsonData();

            Debug.Log(jData);

            byte[] bytes = Encoding.UTF8.GetBytes(jData);
            savedGameClient.CommitUpdate(game, update, bytes, OnSavedGameWritten);
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
            Debug.Log("�ε� ����");
            savedGameClient.ReadBinaryData(metadata, OnSavedGameDataRead);
        }
        else
        {
            // ������
            OnLoadComplete?.Invoke(false);
            OnLoadComplete = null;
            Debug.Log("�ε� ����");
        }
    }

    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] loadedData)
    {
        string jData = Encoding.UTF8.GetString(loadedData);

        if (jData == "")
        {
            Debug.Log("�����Ͱ� ���� �ʱ� ������ ����");
            Save(OnLoadComplete); // OnLoadComplete �ݹ� �Ѱܼ� �Ϸ� �̺�Ʈ �ޱ�
        }
        else
        {
            
            Debug.Log("�ε� ������ : " + jData);

            // ���� ���̺� �� �����͸� �ٽ� �ҷ��´�.
            ServiceLocater.ReturnSaveManager().Save(jData);
            ServiceLocater.ReturnSaveManager().Load();
            OnLoadComplete?.Invoke(true);
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