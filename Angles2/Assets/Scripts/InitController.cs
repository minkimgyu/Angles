using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// addressable �ʱ�ȭ
// SceneController �ʱ�ȭ
// Audio �ʱ�ȭ
// input �ʱ�ȭ --> ��ü input�� --> ������ ��� �ɵ�

public class InitController : MonoBehaviour
{
    AddressableHandler addressableHandler;

    // Start is called before the first frame update
    void Start()
    {
        TimeController timeController = new TimeController();
        ServiceLocater.Provide(timeController);

        SceneController controller = new SceneController();
        ServiceLocater.Provide(controller);

        SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        ServiceLocater.Provide(soundPlayer);

        GameObject addressableObject = new GameObject();
        addressableObject.AddComponent<AddressableHandler>();

        addressableHandler = addressableObject.GetComponent<AddressableHandler>();
        DontDestroyOnLoad(addressableObject);

        addressableHandler.Load(Initialize);
    }

    public void Initialize()
    {
        ISoundPlayable soundPlayable = ServiceLocater.ReturnSoundPlayer();
        soundPlayable.Initialize(addressableHandler.AudioAssetDictionary);

        ServiceLocater.ReturnSceneController().ChangeScene("MenuScene");
    }
}
