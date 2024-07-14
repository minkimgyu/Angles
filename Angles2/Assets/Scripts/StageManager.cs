using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BaseLife player = LifeFactory.Create(BaseLife.Name.Player);
        player.transform.position = Vector2.zero;

        BaseLife triangle1 = LifeFactory.Create(BaseLife.Name.Triangle);
        triangle1.transform.position = new Vector2(3, 3);

        BaseLife rectangle1 = LifeFactory.Create(BaseLife.Name.Rectangle);
        rectangle1.transform.position = new Vector2(3, -3);

        //BaseLife pentagon = LifeFactory.Create(BaseLife.Name.Pentagon);
        //pentagon.transform.position = new Vector2(-3, -3);

        //BaseLife hexagon = LifeFactory.Create(BaseLife.Name.Hexagon);
        //hexagon.transform.position = new Vector2(-3, 3);
    }
}
