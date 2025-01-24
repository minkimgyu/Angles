using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CloudSaveController : MonoBehaviour
{
    [SerializeField] GameObject _content;
    [SerializeField] DecisionMessageViewer _decisionMessageViewer;

    [SerializeField] Transform _cloudViewerParent;
    [SerializeField] Transform _localViewerParent;

    [SerializeField] Button _uploadBtn;
    [SerializeField] Button _retrieveBtn;

    public enum State
    {
        Upload,
        Retrieve
    }

    void CreateCloudViewer(Sprite icon, string info, Transform parent, bool nowUnlock = true)
    {
        CloudSaveViewer coinDataViewer = (CloudSaveViewer)_viewerFactory.Create(BaseViewer.Name.CloudSaveViewer);
        coinDataViewer.ChangeIcon(icon);
        coinDataViewer.ChangeInfo(info);
        coinDataViewer.transform.SetParent(parent);
        coinDataViewer.transform.localScale = Vector3.one;

        if (nowUnlock == false) coinDataViewer.Lock();
    }

    private void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) // Social.localUser.authenticated == true
        {
            _uploadBtn.interactable = false;
            _retrieveBtn.interactable = false;

            // 인터넷 연결 없음
            // Debug.Log("인터넷 연결에 연결되지 않았습니다.");
        }
        else // Application.internetReachability != NetworkReachability.NotReachable
        {
            _uploadBtn.interactable = true;
            _retrieveBtn.interactable = true;
        }
    }

    void CreateCloudViewer(SaveData saveData, Transform parent)
    {
        CreateCloudViewer(_goldIconAsset, saveData._gold.ToString(), parent);

        foreach (GameMode.Type key in Enum.GetValues(typeof(GameMode.Type)))
        {
            List<GameMode.Level> levels = GameMode.GetLevels(key);

            switch (key)
            {
                case GameMode.Type.Chapter:

                    for (int i = 0; i < levels.Count; i++)
                    {
                        string info = $"{saveData._levelInfos[levels[i]].CompleteLevel} / {_levelDatas[levels[i]].MaxLevel}";
                        CreateCloudViewer(_levelIconAsset[levels[i]], info, parent);
                    }

                    break;
                case GameMode.Type.Survival:

                    for (int i = 0; i < levels.Count; i++)
                    {
                        string completeTime = $"{ saveData._levelInfos[levels[i]].CompleteDuration / 60 }:{ (saveData._levelInfos[levels[i]].CompleteDuration % 60).ToString("D2")}";
                        string totalDuration = $"{_levelDatas[levels[i]].TotalDuration / 60 }:{ (_levelDatas[levels[i]].TotalDuration % 60).ToString("D2")}";
                        CreateCloudViewer(_levelIconAsset[levels[i]], $"{completeTime} / {totalDuration}", parent);
                    }

                    break;
                case GameMode.Type.Tutorial:

                    for (int i = 0; i < levels.Count; i++)
                    {
                        string info = $"{saveData._levelInfos[levels[i]].CompleteLevel} / {_levelDatas[levels[i]].MaxLevel}";
                        CreateCloudViewer(_levelIconAsset[levels[i]], info, parent);
                    }

                    break;
            }
        }

        foreach (StatData.Key key in Enum.GetValues(typeof(StatData.Key)))
        {
            if (saveData._statInfos.ContainsKey(key) == false) continue;

            int currentLevel = saveData._statInfos[key]._currentLevel;
            int maxLevel = _statDatas[key].MaxLevel;
            CreateCloudViewer(_statIconAsset[key], $"{currentLevel} / {maxLevel}", parent);
        }

        foreach (SkinData.Key key in Enum.GetValues(typeof(SkinData.Key)))
        {
            if (saveData._skinInfos.ContainsKey(key) == false) continue;
            CreateCloudViewer(_skinIconAsset[key], "", parent, saveData._skinInfos[key]._nowUnlock);
        }
    }

    // 모든 자식 객체 파괴하기
    void DestroyCloudViewer(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    Dictionary<SkinData.Key, Sprite> _skinIconAsset;
    Dictionary<StatData.Key, Sprite> _statIconAsset;
    Dictionary<GameMode.Level, Sprite> _levelIconAsset;
    Sprite _goldIconAsset;

    Dictionary<GameMode.Level, ILevelInfo> _levelDatas;
    Dictionary<StatData.Key, StatData> _statDatas;
    BaseFactory _viewerFactory;

    State _state;

    public void Activate()
    {
        bool active = !_content.activeSelf;
        _content.SetActive(active);

        // 키면 생성, 끄면 파괴
        if(active == true)
        {
            IGPGS gPGS = ServiceLocater.ReturnGPGS();
            //gPGS.ShowSelectUI();

            gPGS.Load((canLoad, saveData) =>
            {
                if (canLoad == false) return;

                JsonParser jsonParser = new JsonParser();
                SaveData cloudSave = jsonParser.JsonToObject<SaveData>(saveData);

                CreateCloudViewer(cloudSave, _cloudViewerParent);
            });

            SaveData clientData = ServiceLocater.ReturnSaveManager().GetSaveData();
            CreateCloudViewer(clientData, _localViewerParent);
        }
        else
        {
            DestroyCloudViewer(_cloudViewerParent);
            DestroyCloudViewer(_localViewerParent);
        }
    }

    void OnOkRequested()
    {
        IGPGS gPGS = ServiceLocater.ReturnGPGS();

        switch (_state)
        {
            case State.Upload:
                gPGS.Save((canSave) => {
                    Debug.Log($"저장 여부: {canSave}");

                    if (canSave == false) return;




                    // 세이브 성공 시
                    gPGS.Load((canLoad, saveJson) => {
                        if (canLoad == false) return;

                        JsonParser jsonParser = new JsonParser();
                        SaveData cloudData = jsonParser.JsonToObject<SaveData>(saveJson);

                        DestroyCloudViewer(_cloudViewerParent); // 부수고
                        CreateCloudViewer(cloudData, _cloudViewerParent); // 다시 생성해줌
                    });
                });
                break;

            case State.Retrieve:
                gPGS.Load((canLoad, saveJson) => {
                    if (canLoad == false) return;

                    ServiceLocater.ReturnSaveManager().Save(saveJson);
                    ServiceLocater.ReturnSaveManager().Load();

                    SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

                    DestroyCloudViewer(_localViewerParent); // 부수고
                    CreateCloudViewer(saveData, _localViewerParent); // 다시 생성해줌
                });
                break;
        }
    }

    public void Initialize(
        Dictionary<SkinData.Key, Sprite> skinIconAsset,
        Dictionary<StatData.Key, Sprite> statIconAsset,
        Dictionary<GameMode.Level, Sprite> levelIconAsset,
        Sprite goldIconAsset,

        Dictionary<GameMode.Level, ILevelInfo> levelDatas,
        Dictionary<StatData.Key, StatData> statDatas,
        BaseFactory viewerFactory)
    {
        _content.SetActive(false);

        _skinIconAsset = skinIconAsset;
        _statIconAsset = statIconAsset;
        _levelIconAsset = levelIconAsset;

        _goldIconAsset = goldIconAsset;

        _levelDatas = levelDatas;
        _statDatas = statDatas;
        _viewerFactory = viewerFactory;

        _decisionMessageViewer.Initialize(() =>
        {
            OnOkRequested();
            _decisionMessageViewer.Activate(false);
        }, () =>
        {
            _decisionMessageViewer.Activate(false);
        });

        _uploadBtn.onClick.AddListener(() =>
        {
            _state = State.Upload;
            string uploadText = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.UploadToCloud);
            _decisionMessageViewer.UpdateInfo(uploadText);

            _decisionMessageViewer.Activate(true);
        });

        _retrieveBtn.onClick.AddListener(() =>
        {
            _state = State.Retrieve;
            string retrieveToLocal = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.RetrieveToLocal);
            _decisionMessageViewer.UpdateInfo(retrieveToLocal);

            _decisionMessageViewer.Activate(true);
        });
    }
}
