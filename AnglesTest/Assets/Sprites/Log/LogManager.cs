using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 이 파일은 내부 테스트 용도로만 사용한다.
// 파일 저장 기능 때문에 렉 걸림
public class LogManager : MonoBehaviour
{
    string _filePath;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _filePath = Application.persistentDataPath + "/log.txt";
        Application.logMessageReceived += OnLogMessageReceived;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= OnLogMessageReceived;
    }

    private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
    {
        // 파일 열기 (추가 모드)
        StreamWriter writer = new StreamWriter(_filePath, true);

        string log = $"{type.ToString()}: {condition} stackTrace => {stackTrace}\n\n\n";
        Debug.Log(log);
        writer.WriteLine(log);  // 텍스트 추가
        writer.Close();  // 파일 닫기
    }
}
