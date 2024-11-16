using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SettingController : MonoBehaviour, ISettable
{
    [SerializeField] GameObject _content;
    [SerializeField] Button _bgmBtn;
    [SerializeField] GameObject _bgmXGo;

    [SerializeField] Button _sfxBtn;
    [SerializeField] GameObject _sfxXGo;

    [SerializeField] Button _exitBtn;
    [SerializeField] Button _resumeBtn;

    public void Activate(bool on)
    {
        // 세이브 데이터 불러와서 적용
        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
        ISoundPlayable playable = ServiceLocater.ReturnSoundPlayer();

        _bgmXGo.SetActive(saveData._muteBGM);
        _sfxXGo.SetActive(saveData._muteSFX);

        playable.MuteBGM(saveData._muteBGM);
        playable.MuteSFX(saveData._muteSFX);
        _content.SetActive(on);
    }

    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);

        _resumeBtn.onClick.AddListener
        (
            () =>
            {
                ISoundPlayable playable = ServiceLocater.ReturnSoundPlayer();
                bool isBGMMute = playable.GetBGMMute();
                bool isSFXMute = playable.GetSFXMute();

                ServiceLocater.ReturnSaveManager().ChangeBGMMute(isBGMMute);
                ServiceLocater.ReturnSaveManager().ChangeSFXMute(isSFXMute);
                Activate(false);
            }
        );

        _bgmBtn.onClick.AddListener
        (
            () => 
            {
                ISoundPlayable playable = ServiceLocater.ReturnSoundPlayer();
                bool isMute = playable.GetBGMMute();

                playable.MuteBGM(!isMute);
                _bgmXGo.SetActive(!isMute);
            }
        );

        _sfxBtn.onClick.AddListener
        (
            () =>
            {
                ISoundPlayable playable = ServiceLocater.ReturnSoundPlayer();
                bool isMute = playable.GetSFXMute();

                playable.MuteSFX(!isMute);
                _sfxXGo.SetActive(!isMute);
            }
        );

        _exitBtn.onClick.AddListener
        (
            () => 
            {
                string sceneName = ServiceLocater.ReturnSceneController().GetCurrentSceneName();
                switch (sceneName)
                {
                    case "LobbyScene": // 로비 씬의 경우
                        ServiceLocater.ReturnSceneController().ChangeScene("MenuScene");
                        break;
                    default: // 플레이 씬의 경우
                        ServiceLocater.ReturnSceneController().ChangeScene("LobbyScene");
                        break;
                }
                Activate(false);
            }
        );

        Activate(false);
    }
}
