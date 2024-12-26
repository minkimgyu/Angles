using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBParser : MonoBehaviour
{
    [ContextMenu("Create DB")]
    public void CreateDBJson()
    {
        JsonParser jsonParser = new JsonParser();
        FileIO fileIO = new FileIO(jsonParser, ".txt");

        string fileName = "Database";
        string fileLocation = "JsonData";

        Database database = new Database();
        fileIO.SaveData(database, fileLocation, fileName);
    }
}