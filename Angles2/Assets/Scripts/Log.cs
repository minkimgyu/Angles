using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        JsonParser _jsonParser = new JsonParser();


        Debug.Log(_jsonParser.DataToJson(new BladeData(100, 10, 3, 1)));
        Debug.Log(_jsonParser.DataToJson(new BulletData(100, 10, 3)));
        Debug.Log(_jsonParser.DataToJson(new RocketData(100, 10, 3, 105, 3)));

        Debug.Log(_jsonParser.DataToJson(new BlackholeData(100, 10, 6, 0.1f, 10)));
        Debug.Log(_jsonParser.DataToJson(new ShooterData(100, 5, 1, 2)));
        Debug.Log(_jsonParser.DataToJson(new StickyBombData(100, 3, 3)));


        Debug.Log(_jsonParser.DataToJson(new StatikkData(1, 101, 3, 3, new List<ITarget.Type> { ITarget.Type.Red })));
        Debug.Log(_jsonParser.DataToJson(new KnockbackData(1, 100, new SerializableVector2(5.5f, 3), new SerializableVector2(1.5f, 0),
            new List<ITarget.Type> { ITarget.Type.Red })));

        Debug.Log(_jsonParser.DataToJson(new ImpactData(1, 100, 5, new List<ITarget.Type> { ITarget.Type.Red })));

        Debug.Log(_jsonParser.DataToJson(new SpawnStickyBombData(1, new List<ITarget.Type> { ITarget.Type.Red })));
        Debug.Log(_jsonParser.DataToJson(new SpawnBlackholeData(1, new List<ITarget.Type> { ITarget.Type.Red })));
        Debug.Log(_jsonParser.DataToJson(new SpawnBladeData(1, new List<ITarget.Type> { ITarget.Type.Red })));
        Debug.Log(_jsonParser.DataToJson(new SpawnShooterData(1, new List<ITarget.Type> { ITarget.Type.Red })));
    }
}
