using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Button _startButton;
    [SerializeField] Button _exitButton;

    // Start is called before the first frame update
    void Start()
    {
        _startButton.onClick.AddListener(OnStartRequested);
        _exitButton.onClick.AddListener(OnExitRequested);
    }

    public void OnStartRequested()
    {
        ServiceLocater.ReturnSoundPlayer().PlaySFX(ISoundPlayable.SoundName.Click);

        ISceneControllable controller = ServiceLocater.ReturnSceneController();
        controller.ChangeScene("LobbyScene");
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
