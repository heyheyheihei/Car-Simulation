using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 車の情報を持ったクラス
 */ 
public class Car : MonoBehaviour
{
    public float dTime;
    public string[] linkIDs;
    List<string> linkChanger = new List<string>();

    List<Link> linklist = new List<Link>();
    Link nowLink;
    public int myIndex; //リンク内で何番目の車かを表す変数
    int nowLinkIndex = 0;
    public Node[] nodes = new Node[2];
    public float endNodeDis;
    public float runDistance = 0;
    public float rect;

    LinkList LINKS;

    public string carName = "";
    public float maxCarsDistance = 0.025f;
    public float velo = 1f;

    public float frontDis;
    public float StartX;
    public float StartY;

    public GameObject startPoint;
    public float time = 0f;
    public float startTime = 0f;

    public string nowLinkName;

    public Timer timer;
    public int result;
    List<DijkstraLink> RouteList;

    DijkstraNode StartPoint;
    DijkstraNode GoalPoint;
    Dijkstra dijkstra;
    GoalPointMaster goalPointMaster;
    StartPointMaster StartPointMaster;

    void Start()
    {
        LINKS = GameObject.Find("LinkList").GetComponent<LinkList>();
        dijkstra = new Dijkstra();
        goalPointMaster = new GoalPointMaster();

        StartPoint = new DijkstraNode(StartX, StartY);
        //一番近いゴール地点を探すメソッドを呼び出す
        GoalPoint = goalPointMaster.Calculate_SG_Distance(StartPoint);

        //最短経路探索実行
        result = dijkstra.Execute(StartPoint, GoalPoint);
        RouteList = dijkstra.FindRoute();

        //経路を取得
        for (int j = 0; j < RouteList.Count; j++)
        {
            linkChanger.Add(RouteList[j].linkID);
        }
        linkIDs = linkChanger.ToArray();

        timer = GameObject.Find("Timer").GetComponent<Timer>();
        dTime = timer.dTime;

        foreach (string i in linkIDs)
        {
            linklist.Add(LINKS.linkList[i]);
        }

        nowLink = linklist[nowLinkIndex];

        //carListはリンク上にいる車の台数や名前等を持つリスト
        nowLink.carList.Add(this);

        nodes = nowLink.getStartNodes(transform.position.x, transform.position.y, carName);
        rect = nowLink.getRect(nodes[0], nodes[1]);
        endNodeDis = nowLink.length - runDistance;

        startTime = timer.Time;

    }

    void Update()
    {
        time += dTime;
        frontDis = getFrontCarDis();
        if (frontDis > maxCarsDistance)
        {
            move();
        }
    }

    /*
     * 車を前進させるメソッド
     */
    void move()
    {
        runDistance += velo * dTime;

        if (runDistance > nowLink.length)
        {
            do // runDistance>nowLink.lengthのとき(2つ以上のリンクを飛び越えたとき)
            {
                frontDis = getFrontCarDis();
                if (frontDis < maxCarsDistance)
                {
                    break;
                }

                //次のリンクに移動したためindexを追加し，carListから自身を消去
                nowLinkIndex++;
                nowLink.carList.Remove(this);

                //まだ行先があるとき
                if (nowLinkIndex < linklist.Count)
                {
                    //
                    float nextDis = runDistance - nowLink.length;

                    //ノードの終着点を次のノードの始点にする処理
                    transform.position = new Vector3(nodes[1].x, nodes[1].y, 0);

                    //リンクを更新(1つ先)
                    nowLink = linklist[nowLinkIndex];
                    nowLink.carList.Add(this);
                    nodes = nowLink.getStartNodes(transform.position.x, transform.position.y, carName);
                    rect = nowLink.getRect(nodes[0], nodes[1]);

                    //z軸回りにrect度分回転させる
                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, rect);
                    //nextDis分，今いるところから移動
                    transform.Translate(new Vector3(0, nextDis, 0));

                    //runDisは各ノードごとに走った距離
                    runDistance = nextDis;
                    endNodeDis = nowLink.length - runDistance;
                }
                else
                {
                    GameObject.Find("Button").GetComponent<MyButton>().resultList.Add(new CarResult(carName, startPoint.GetComponent<StartPoint>().spName, time, timer.Time, startTime));
                    Destroy(this.gameObject);
                    break;
                }
            } while (runDistance > nowLink.length);
        }
        else  //次のリンクに移動していない場合
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rect);
            transform.Translate(new Vector3(0, velo * dTime, 0));

            endNodeDis = nowLink.length - runDistance;
        }
    }

    /*
     * 前方の車との距離を計算するメソッド
     * return 前方車との距離
     */ 
    float getFrontCarDis()
    {
        myIndex = nowLink.carList.FindIndex(c => c.GetComponent<Car>().carName == carName);
        float dis = float.MaxValue;
        nowLinkName = nowLink.id;

        if (myIndex > 0) //前方に車があるとき
        {
            Car frontCar = nowLink.carList[myIndex - 1].GetComponent<Car>();
            if (frontCar.runDistance > this.runDistance)
            {
                dis = frontCar.runDistance - this.runDistance;
            }
            else 
            {
                dis = 0;
            }
        }
        else            //前方に車がないとき
        {
            if (nowLinkIndex + 1 < linklist.Count) //まだ先にリンクがある場合
            {
                Link nextLink = linklist[nowLinkIndex + 1];
                
                if (nextLink.carList.Count > 0)    //次のリンクにいる車の数が0より大きいとき
                {
                    Car frontCar = nextLink.carList[nextLink.carList.Count - 1].GetComponent<Car>();
                    dis = frontCar.runDistance + this.endNodeDis;
                }
            }
        }
        return dis;
    }
}
