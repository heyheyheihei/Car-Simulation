using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 時間経過と共にCarインスタンスを作成するクラス
 */ 
public class StartPoint : MonoBehaviour
{
    public string[] linkIDs;
    public GameObject Carprefab;

    public string spName = "";
    public int maxNumberOfCar;
    public float[] carVelo;
    public int[] volume;
    public float StartX;
    public float StartY;
    float count = 0;
    int flag = 0;
    float maxCount;

    GameObject carIns;
    float dTime;
    Timer timer;

    void Start()
    {
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        dTime = timer.dTime;

        //CSVの読み取り結果から出発時間を決定
        maxCount = Random.Range(volume[0], volume[1]);
    }

    void Update()
    {
        if (count > maxCount)
        {
            //Carprefabを基にインスタンスを作成
            carIns = Instantiate(Carprefab, transform.position, transform.rotation);

            carIns.GetComponent<Car>().StartX = StartX;
            carIns.GetComponent<Car>().StartY = StartY;
            carIns.GetComponent<Car>().velo = 0.083333333f;
            carIns.GetComponent<Car>().carName = spName;
            carIns.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.7f);
            carIns.GetComponent<Car>().startPoint = this.gameObject;

            count = 0;
            flag = 1;
        }
        if (flag != 1) { 
        count += dTime;
        }
    }
}


