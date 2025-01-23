using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// �� ������ ���� �׽�Ʈ �뵵�θ� ����Ѵ�.
// ���� ���� ��� ������ �� �ɸ�
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
        // ���� ���� (�߰� ���)
        StreamWriter writer = new StreamWriter(_filePath, true);

        string log = $"{type.ToString()}: {condition} stackTrace => {stackTrace}\n\n\n";
        Debug.Log(log);
        writer.WriteLine(log);  // �ؽ�Ʈ �߰�
        writer.Close();  // ���� �ݱ�
    }
}
