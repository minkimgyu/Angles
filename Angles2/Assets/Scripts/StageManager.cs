using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static StageManager _instance;
    public static StageManager Instance { get { return _instance; } }

    [SerializeField] int _coinCount;
    public int CoinCount { get { return _coinCount; } set { _coinCount = value; } }

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        BaseLife player = LifeFactory.Create(BaseLife.Name.Player);
        player.transform.position = Vector2.zero;

        //BaseLife triangle1 = LifeFactory.Create(BaseLife.Name.Triangle);
        //triangle1.transform.position = new Vector2(3, 3);

        //BaseLife rectangle1 = LifeFactory.Create(BaseLife.Name.Rectangle);
        //rectangle1.transform.position = new Vector2(3, -3);

        //BaseLife pentagon = LifeFactory.Create(BaseLife.Name.Pentagon);
        //pentagon.transform.position = new Vector2(-3, -3);

        //BaseLife hexagon = LifeFactory.Create(BaseLife.Name.Hexagon);
        //hexagon.transform.position = new Vector2(-3, 3);
    }
}
