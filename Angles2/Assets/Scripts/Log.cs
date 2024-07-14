using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        JsonParser _jsonParser = new JsonParser();


        //Debug.Log(_jsonParser.DataToJson(new BladeData(100, 10, 3, 1)));
        //Debug.Log(_jsonParser.DataToJson(new BulletData(100, 10, 3)));
        //Debug.Log(_jsonParser.DataToJson(new RocketData(100, 10, 3, 105, 3)));

        //Debug.Log(_jsonParser.DataToJson(new BlackholeData(100, 10, 6, 0.1f, 10)));
        //Debug.Log(_jsonParser.DataToJson(new ShooterData(100, 5, 3, 1, 2.0f)));
        //Debug.Log(_jsonParser.DataToJson(new StickyBombData(100, 3, 3)));


        //Debug.Log(_jsonParser.DataToJson(new StatikkData(1, 101, 3, 3, new List<ITarget.Type> { ITarget.Type.Red })));
        //Debug.Log(_jsonParser.DataToJson(new KnockbackData(1, 100, new SerializableVector2(5.5f, 3), new SerializableVector2(1.5f, 0),
        //    new List<ITarget.Type> { ITarget.Type.Red })));

        //Debug.Log(_jsonParser.DataToJson(new ImpactData(1, 100, 5, new List<ITarget.Type> { ITarget.Type.Red })));

        //Debug.Log(_jsonParser.DataToJson(new SpawnStickyBombData(1, new List<ITarget.Type> { ITarget.Type.Red })));
        //Debug.Log(_jsonParser.DataToJson(new SpawnBlackholeData(1, new List<ITarget.Type> { ITarget.Type.Red })));
        //Debug.Log(_jsonParser.DataToJson(new SpawnBladeData(1, 3f, new List<ITarget.Type> { ITarget.Type.Red })));
        //Debug.Log(_jsonParser.DataToJson(new SpawnShooterData(1, new List<ITarget.Type> { ITarget.Type.Red })));


        SpreadBulletsData spreadBulletsData = new SpreadBulletsData(
            1f,
            20f,
            5f,
            3f,
            3f,
            5,
            new List<ITarget.Type> { ITarget.Type.Blue }
        );

        Debug.Log(_jsonParser.DataToJson(spreadBulletsData));


        SelfDestructionData selfDestructionData = new SelfDestructionData(
            1f,
            20f,
            5f,
            3f,
            new List<ITarget.Type> { ITarget.Type.Blue }
        );

        Debug.Log(_jsonParser.DataToJson(selfDestructionData));

        MagneticFieldData magneticFieldData = new MagneticFieldData(
           1f,
           20f,
           5f,
           3f,
           new List<ITarget.Type> { ITarget.Type.Blue }
       );

        Debug.Log(_jsonParser.DataToJson(magneticFieldData));

        ShockwaveData shockwaveData = new ShockwaveData(
           1f,
           20f,
           5f,
           3f,
           new List<ITarget.Type> { ITarget.Type.Blue }
       );

        Debug.Log(_jsonParser.DataToJson(shockwaveData));




        PlayerData playerData = new PlayerData(
            100, 
            ITarget.Type.Blue, 
            10, 
            15, 
            0.5f,

            15,
            0.5f,
            0.5f,
            3,
            1,
            1.5f,

            0.15f,
            0.3f
        );
        Debug.Log(_jsonParser.DataToJson(playerData));

        TriangleData triangleData = new TriangleData(
            100,
            ITarget.Type.Red,
            5,
            new List<BaseSkill.Name> { BaseSkill.Name.MagneticField }
        );
        Debug.Log(_jsonParser.DataToJson(triangleData));

        RectangleData rectangleData = new RectangleData(
            100,
            ITarget.Type.Red,
            5,
            new List<BaseSkill.Name> { BaseSkill.Name.MagneticField }
        );
        Debug.Log(_jsonParser.DataToJson(rectangleData));

        PentagonData pentagonData = new PentagonData(
            100,
            ITarget.Type.Red,
            5,
            new List<BaseSkill.Name> { BaseSkill.Name.SpreadBullets },
            4f,
            2f
        );
        Debug.Log(_jsonParser.DataToJson(pentagonData));

        HexagonData hexagonData = new HexagonData(
            100,
            ITarget.Type.Red,
            5,
            new List<BaseSkill.Name> { BaseSkill.Name.Shockwave },
            4f,
            2f
        );
        Debug.Log(_jsonParser.DataToJson(hexagonData));
    }
}
