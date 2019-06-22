using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ゲーム開始後のタイマーを管理するクラス
 */ 

public class Timer : MonoBehaviour {
    
    public float Time = 0f;
    public float dTime = 1.0f;

	void Start ()
    {
		
	}
	
	void Update ()
    {
       Time += dTime;
       this.GetComponent<Text>().text = this.Time.ToString() + " sec from earthquake occurred";
    }
}
