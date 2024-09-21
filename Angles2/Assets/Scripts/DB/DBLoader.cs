using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using UnityEngine.Networking;

public class DBLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //DBConnectionCheck();
        StartCoroutine(DBCreater());

        DataBaseInsert("insert into Person(ID, Name, Birthday) VALUES(4, \"��\", \"2001-01-26\")");
        DataBaseRead("Select * From Person");
    }

    public void DataBaseInsert(string query)
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();

        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;
        dbCommand.ExecuteNonQuery();

        dbCommand.Dispose();
        dbCommand = null;

        dbConnection.Close();
        dbConnection = null;
    }

    public void DataBaseRead(string query)
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();

        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;

        IDataReader dataReader = dbCommand.ExecuteReader();
        while (dataReader.Read())
        {
            Debug.Log(dataReader.GetInt32(0) + ", " + dataReader.GetString(1) + "," + dataReader.GetString(2));
        }

        dataReader.Dispose();
        dataReader = null;

        dbCommand.Dispose();
        dbCommand = null;

        dbConnection.Close();
        dbConnection = null;
    }

    public void DBConnectionCheck()
    {
        try
        {
            string filepath = GetDBFilePath();
            Debug.Log(filepath);

            IDbConnection dbConnection = new SqliteConnection(filepath);
            dbConnection.Open();

            if (dbConnection.State == ConnectionState.Open)
            {
                Debug.Log("DB ����");
            }
            else
            {
                Debug.Log("���� ����");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    IEnumerator DBCreater()
    {
        string filePath = Application.dataPath + "/test.db"; // �����Ϳ��� ����� ���
        string sourcePath = Application.dataPath + "/StreamingAssets" + "/test.db"; // �ҽ� ���

        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = Application.persistentDataPath + "/test.db";
            if (!File.Exists(filePath))
            {
                UnityWebRequest unityWebRequest = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets" + "/test.db");
                unityWebRequest.downloadedBytes.ToString();
                yield return unityWebRequest.SendWebRequest().isDone;
                File.WriteAllBytes(filePath, unityWebRequest.downloadHandler.data);
            }
        }
        else
        {
            try
            {
                File.Copy(sourcePath, filePath);
                Debug.Log("DB ���� ���� �Ϸ�: " + filePath);
            }
            catch (Exception e)
            {
                Debug.Log("DB ���� ���� ����: " + e.Message);
            }
        }

        //Debug.Log("DB ���� �Ϸ�!");
        yield return null;
    }

    public string GetDBFilePath()
    {
        string str = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
        {
            str = "URI=file:" + Application.persistentDataPath + "/test.db";
        }
        else
        {
            str = "URI=file:" + Application.dataPath + "/test.db";
        }

        return str;
    }
}
