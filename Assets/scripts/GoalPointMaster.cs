using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * スタート地点に対して最寄りのゴール地点を検索（検索したゴール地点をダイクストラ法で使う為）する為のクラス
 */ 
public class GoalPointMaster
{
    StartPointMaster _StartPointMaster;
    public DijkstraNode NearestGoal;
    float length;
    float shortest_length;

    public GoalPointMaster()
    {
        _StartPointMaster = GameObject.Find("StartPointMaster").GetComponent<StartPointMaster>();
    }

    /*
     * 引数で受け取ったスタート地点の座標に直線距離で一番近いゴール地点を検索するメソッド
     * 引数　スタート地点の情報
     * return  最寄りのゴール地点
     */ 
   public DijkstraNode Calculate_SG_Distance(DijkstraNode StartPoint)
    {
        NearestGoal = null;
        shortest_length = int.MaxValue;

        foreach(DijkstraNode g in _StartPointMaster.GoalPointList)
        {
            length = Mathf.Sqrt(Mathf.Pow(StartPoint.x - g.x, 2f) + Mathf.Pow(StartPoint.y - g.y, 2f));

            if(length <= shortest_length)
            {
                shortest_length = length;
                NearestGoal = g;
            }
        }
        return NearestGoal;
    }
}