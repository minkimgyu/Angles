using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// addressable 초기화
// SceneController 초기화
// Audio 초기화
// input 초기화 --> 전체 input임 --> 솔직히 없어도 될듯

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
        if (nowSuccess == true)
        {
            // 로그인이 되는 경우만 데이터를 로드함
            _gPGSManager.Load(OnLoadCompleted);
        }
        else
        {
            // 로그인에 실패하는 경우 게임 정보 로드 없이 바로 초기화 진행
            SetUp();
        }
    }

    void OnLoadCompleted(bool nowSuccess)
    {
        // 로드 성공 여부와 관계 없이 초기화 진행
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
        //bool haveSaveFile = saveController.HaveSaveFile();

        //// 세이브 파일이 있는 경우
        //if (haveSaveFile == true)
        //{
        //    saveController.Load(); // 세이브 데이터 로드 적용
        //}
        //else
        //{
        //    // 세이브 파일이 없는 경우
        //    // 서버에서 데이터를 로드해와야함

        //    // 만약 서버에도 세이브 파일이 없는 경우
        //    // 기본 데이터를 로드해야함

        //    _gPGSManager.Load();
        //}


        LocalizationHandler localizationHandler = new LocalizationHandler(addressableHandler.LocalizationAsset);

        ServiceLocater.Provide(timeController);
        ServiceLocater.Provide(sceneController);
        ServiceLocater.Provide(soundPlayer);
        ServiceLocater.Provide(saveController);
        ServiceLocater.Provide(localizationHandler);

        // 위 내용을 전부 반영하고 SettingController 적용
        SettingController settingController = FindObjectOfType<SettingController>();
        settingController.Initialize();
        ServiceLocater.Provide(settingController);

        ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.MenuScene);
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
