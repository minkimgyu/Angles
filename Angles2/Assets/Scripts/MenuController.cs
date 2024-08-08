using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Button _startButton;
    [SerializeField] Button _exitButton;

    // Start is called before the first frame update
    void Start()
    {
        _startButton.onClick.AddListener(OnStartRequested);
        _exitButton.onClick.AddListener(OnExitRequested);

        ServiceLocater.Initialize();

        SceneController controller = new SceneController();
        ServiceLocater.Provide(controller);
    }

    public void OnStartRequested()
    {
        ISceneControllable controller = ServiceLocater.ReturnSceneController();
        controller.ChangeScene("PlayScene");
    }

    public void OnExitRequested()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
