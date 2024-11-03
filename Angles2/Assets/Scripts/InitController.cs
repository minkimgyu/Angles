using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

// addressable 초기화
// SceneController 초기화
// Audio 초기화
// input 초기화 --> 전체 input임 --> 솔직히 없어도 될듯

public class InitController : MonoBehaviour
{
    [SerializeField] Image _loadingPregressBar;
    [SerializeField] TMP_Text _loadingPregressTxt;

    // Start is called before the first frame update
    void Start()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
        Screen.SetResolution(Screen.width, Screen.height, true);

        _loadingPregressBar.fillAmount = 0;
        _loadingPregressTxt.text = $"{0} %";

        AddressableHandler addressableHandler = CreateAddressableHandler();
        addressableHandler.AddProgressEvent((value) => { _loadingPregressBar.fillAmount = value; _loadingPregressTxt.text = $"{value * 100} %"; });
        addressableHandler.Load(() => { Initialize(addressableHandler); });
    }

    public void Initialize(AddressableHandler addressableHandler)
    {
        Database database = new Database();
        FactoryCollection factoryCollection = new FactoryCollection(addressableHandler, database);

        CoreSystem coreSystem = CreateCoreSystem(addressableHandler, factoryCollection, database);

        TimeController timeController = new TimeController();
        SceneController controller = new SceneController();

        SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        soundPlayer.Initialize(coreSystem.AddressableHandler.SoundAsset);

        SaveManager saveController = new SaveManager(database.DefaultSaveData);

        ServiceLocater.Provide(timeController);
        ServiceLocater.Provide(controller);
        ServiceLocater.Provide(soundPlayer);
        ServiceLocater.Provide(saveController);

        ServiceLocater.ReturnSceneController().ChangeScene("MenuScene");
    }

    CoreSystem CreateCoreSystem(AddressableHandler addressableHandler, FactoryCollection factoryCollection, Database database)
    {
        GameObject gameSystem = new GameObject();
        gameSystem.name = "gameSystem";
        gameSystem.AddComponent<CoreSystem>();

        CoreSystem coreSystem = gameSystem.GetComponent<CoreSystem>();
        coreSystem.Initialize(addressableHandler, factoryCollection, database);
        DontDestroyOnLoad(coreSystem);
        return coreSystem;
    }

    AddressableHandler CreateAddressableHandler()
    {
        GameObject addressable = new GameObject();
        addressable.name = "Addressable";
        addressable.AddComponent<AddressableHandler>();
        AddressableHandler addressableHandler = addressable.GetComponent<AddressableHandler>();
        DontDestroyOnLoad(addressable);

        return addressableHandler;
    }
}
