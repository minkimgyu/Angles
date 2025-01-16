using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// addressable �ʱ�ȭ
// SceneController �ʱ�ȭ
// Audio �ʱ�ȭ
// input �ʱ�ȭ --> ��ü input�� --> ������ ��� �ɵ�

public class InitController : MonoBehaviour
{
    [SerializeField] Image _loadingPregressBar;
    [SerializeField] TMP_Text _loadingPregressTxt;

    GPGSManager _gPGSManager;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
        Screen.SetResolution(Screen.width, Screen.height, true);

        _gPGSManager = new GPGSManager();
        _gPGSManager.Login(OnLoginCompleted);
    }

    void OnLoginCompleted(bool nowSuccess)
    {
        SetUp();
    }

    void SetUp()
    {
        _loadingPregressBar.fillAmount = 0;
        _loadingPregressTxt.text = $"{0} %";

        AddressableHandler addressableHandler = CreateAddressableHandler();
        addressableHandler.AddProgressEvent((value) => { _loadingPregressBar.fillAmount = value; _loadingPregressTxt.text = $"{value * 100} %"; });
        addressableHandler.Load(() => { Initialize(addressableHandler); });
    }

    void Initialize(AddressableHandler addressableHandler)
    {
        TimeController timeController = new TimeController();
        SceneController sceneController = new SceneController();

        SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        soundPlayer.Initialize(addressableHandler.SoundAsset);

        SaveManager saveController = new SaveManager(new SaveData(0));
        LocalizationHandler localizationHandler = new LocalizationHandler(addressableHandler.LocalizationAsset);
        

        ServiceLocater.Provide(timeController);
        ServiceLocater.Provide(sceneController);
        ServiceLocater.Provide(soundPlayer);
        ServiceLocater.Provide(saveController);
        ServiceLocater.Provide(localizationHandler);

        InjectSettingController();
        InjectAdMobManager(() =>
        {
            InjectAdTimer(() =>
            {
                ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.MenuScene);
            });
        });
    }

    void InjectSettingController()
    {
        // �� ������ ���� �ݿ��ϰ� SettingController ����
        SettingController settingController = FindObjectOfType<SettingController>();
        settingController.Initialize();
        ServiceLocater.Provide(settingController);
    }

    void InjectAdMobManager(Action OnComplete)
    {
        AdMobManager adMobManager = FindObjectOfType<AdMobManager>();
        ServiceLocater.Provide(adMobManager);

        // �ʱ�ȭ �Ϸ� �� ����
        adMobManager.Initialize(() =>
        {
            OnComplete?.Invoke();
        });
    }

    void InjectAdTimer(Action OnComplete)
    {
        // �� ������ ���� �ݿ��ϰ� AdTimer ����
        AdTimer adTimer = FindObjectOfType<AdTimer>();
        ServiceLocater.Provide(adTimer);

        // �ʱ�ȭ �Ϸ� �� ����
        adTimer.Initialize(() =>
        {
            OnComplete?.Invoke();
        });
    }

    AddressableHandler CreateAddressableHandler()
    {
        GameObject addressable = new GameObject();
        addressable.name = "Addressable";
        AddressableHandler addressableHandler = addressable.AddComponent<AddressableHandler>();
        DontDestroyOnLoad(addressable);

        return addressableHandler;
    }
}
