using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonParser
{
    public T JsonToData<T>(string jdata)
    {
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        return JsonConvert.DeserializeObject<T>(jdata, settings);
    }

    public string DataToJson<T>(T data)
    {
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        return JsonConvert.SerializeObject(data, Formatting.Indented, settings);
    }
}
