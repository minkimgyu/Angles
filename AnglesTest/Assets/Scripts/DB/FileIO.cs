using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class FileIO
{
    JsonParser _parser;
    string _extension;

    public FileIO(JsonParser parser, string extension)
    {
        _parser = parser;
        _extension = extension;
        // Application.dataPath + filePath + fileName + _extension
    }

    string ReturnPath(string filePath, string fileName)
    { return $"{Application.dataPath}/{filePath}/{fileName}{_extension}"; }

    public T LoadData<T>(string jData)
    {
        return _parser.JsonToObject<T>(jData);
    }

    public T LoadData<T>(string filePath, string fileName)
    {
        string path = ReturnPath(filePath, fileName);
        if (File.Exists(path) == false) return default;

        string jData = File.ReadAllText(path);
        return _parser.JsonToObject<T>(jData);
    }

    public void SaveData(object objectToParse, string filePath, string fileName)
    {
        string path;
        path = ReturnFilePath(filePath, fileName); // 겹치지 않는 이름을 찾음

        Debug.Log(path);

        string jsonAsset = _parser.ObjectToJson(objectToParse);

        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonAsset);
        //string encodedJson = System.Convert.ToBase64String(bytes);
        //File.WriteAllText(path, encodedJson); // 이런 방식으로 생성시켜줌

        File.WriteAllText(path, jsonAsset); // 이런 방식으로 생성시켜줌
        AssetDatabase.Refresh();
    }

    string ReturnFilePath(string filePath, string fileName)
    {
        string path = ReturnPath(filePath, fileName);
        if (File.Exists(path) == false) return path;

        Debug.LogError("이미 해당 경로에 파일이 존재함");

        string originName = fileName;
        int index = 0;

        do
        {
            if (index > 0) originName = $"{originName} {index}";

            path = ReturnPath(filePath, originName);
            if (File.Exists(path) == false) break; // 존재하지 않으면 break
            else index++;
        }
        while (true);

        return path;
    }
}