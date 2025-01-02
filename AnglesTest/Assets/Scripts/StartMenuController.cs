using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Button _startButton;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _languageButton;

    // Start is called before the first frame update
    void Start()
    {
        _startButton.onClick.AddListener(OnStartRequested);
        _exitButton.onClick.AddListener(OnExitRequested);

        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
        _languageButton.GetComponentInChildren<TMP_Text>().text = (saveData._language.ToString()).Substring(0, 3);

        _languageButton.onClick.AddListener(ChangeLocalization);

        string startWord = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.Start);
        _startButton.GetComponentInChildren<TMP_Text>().text = startWord;

        string endWord = ServiceLocater.ReturnLocalizationHandler().GetWord(ILocalization.Key.End);
        _exitButton.GetComponentInChildren<TMP_Text>().text = endWord;
    }

    void ChangeLocalization()
    {
        int enumCount = Enum.GetValues(typeof(ILocalization.Language)).Length;
        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();

        ILocalization.Language language = (ILocalization.Language)(((int)saveData._language + 1) % enumCount);
        ServiceLocater.ReturnSaveManager().ChangeLanguage(language);

        ServiceLocater.ReturnSettingController().ChangeLanguage(); // settingController 언어 설정 변경
        ServiceLocater.ReturnSceneController().ChangeScene(ISceneControllable.SceneName.MenuScene);
    }

    public void OnStartRequested()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click);
        ISceneControllable controller = ServiceLocater.ReturnSceneController();
        controller.ChangeScene(ISceneControllable.SceneName.LobbyScene);
    }

    public void OnExitRequested()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
