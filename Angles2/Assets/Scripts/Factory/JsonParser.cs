using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonParser
{
    public T JsonToData<T>(string jdata)
    {
        return JsonConvert.DeserializeObject<T>(jdata);
    }

    public string DataToJson<T>(T data)
    {
        return JsonConvert.SerializeObject(data);
    }
}
