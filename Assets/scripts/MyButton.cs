using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * CSVファイルに出力する変数を処理するためのクラス 
 */

public class CarResult
{
    public string carName;
    public string spName;
    public float carTime;
    public float escapeTime;
    public float startTime;

    public CarResult(string _name, string _spName, float _carTime, float _escapeTime, float _startTime)
    {
        carName = _name;
        spName = _spName;
        carTime = _carTime;
        escapeTime = _escapeTime;
        startTime = _startTime;
    }

}


/* 
 * Unity上の結果出力ボタンを押した際にcsvファイルを出力するためのメソッド 
 */
 public class MyButton : MonoBehaviour {

    public List<CarResult> resultList = new List<CarResult>();

    public void OnClick()
    {
        Debug.Log("結果出力");
        GetComponent<AudioSource>().Play();
        StreamWriter sw = new StreamWriter(Application.dataPath + "/Outputs/result.csv", false);

        sw.WriteLine("carName,startPointName,travelTime,escapeTime,startTime");

        foreach(CarResult r in resultList)
        {
            sw.WriteLine(r.carName + "," + r.spName + "," + r.carTime.ToString() + "," + r.escapeTime.ToString() + "," + r.startTime.ToString());
        }

        sw.Flush();
        sw.Close();
    }

}
