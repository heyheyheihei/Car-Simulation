using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
 * 
 * 
 */ 
public class StartPointMaster : MonoBehaviour {

    public GameObject StartPointPrefab;
    public string START_POINT_DATA_NAME = "StartPoint_test";
    public string GOAL_POINT_DATA_NAME = "GoalPoint";
    public DijkstraNode GoalPoint;

    List<GameObject> spList = new List<GameObject>();
    public List<DijkstraNode> GoalPointList = new List<DijkstraNode>();

    //StartPointとGoalPointをcsvファイルから読み込む処理
    void Start () {
   
        TextAsset StartFile = Resources.Load("CSV/" + START_POINT_DATA_NAME) as TextAsset;
        TextAsset GoalFile = Resources.Load("CSV/" + GOAL_POINT_DATA_NAME) as TextAsset;

        StringReader S_reader = new StringReader(StartFile.text);
        StringReader G_reader = new StringReader(GoalFile.text);
        
        Timer timer = GameObject.Find("Timer").GetComponent<Timer>();

        GameObject sp;
        string[] S_rows;
        string[] G_rows;

        //1行目はインデックス
        S_reader.ReadLine();

        while(S_reader.Peek() > -1)
        {
            S_rows = S_reader.ReadLine().Split(',');

            sp = Instantiate(StartPointPrefab, new Vector3(float.Parse(S_rows[1]), float.Parse(S_rows[2]), 0f), transform.rotation);

            sp.GetComponent<StartPoint>().StartX = float.Parse(S_rows[1]);
            sp.GetComponent<StartPoint>().StartY = float.Parse(S_rows[2]);
            sp.GetComponent<StartPoint>().spName = S_rows[0];
            sp.GetComponent<StartPoint>().volume = new int[2] { int.Parse(S_rows[6]), int.Parse(S_rows[7]) };
            spList.Add(sp);
        }

        /*
         *GoalPoint一覧が保存されているファイルを読み込み，リストに保存する処理 
         */
        G_reader.ReadLine();

        while(G_reader.Peek() > -1)
        {
            G_rows = G_reader.ReadLine().Split(',');
            GoalPoint = new DijkstraNode(float.Parse(G_rows[1]), float.Parse(G_rows[2]));
            GoalPointList.Add(GoalPoint);
        }


    }
	
	void Update () {
		
	}
}
