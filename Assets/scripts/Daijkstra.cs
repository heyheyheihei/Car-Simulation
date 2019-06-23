using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;

/*
 * ダイクストラ法に利用するためのリンク情報を持ったクラス
 */
public class DijkstraLink
{
    public DijkstraNode Node1; 
    public DijkstraNode Node2;
    public float LinkDistance;
    public string linkID;
    public int flag;

    public DijkstraLink(string linkid, DijkstraNode node1, DijkstraNode node2)
    {
        linkID = linkid;
        Node1 = node1;
        Node2 = node2;
        LinkDistance = Mathf.Sqrt(Mathf.Pow(node1.x - node2.x, 2f) + Mathf.Pow(node1.y - node2.y, 2f));
    }


}

/*
 * ダイクストラ法に利用するためのノード情報を持ったクラス
 */
 public class DijkstraNode
{
    public enum NodeStatus
    {
        NotYet,     // 未確定状態
        Temporary,  // 仮確定状態
        Completed   // 確定状態
    }

    public float Distance;             
    public DijkstraNode SourceNode;     // ソースノード
    public NodeStatus Status;           // 最短経路確定状態
    public float x;
    public float y;
    public string ID;
    public DijkstraLink LinkID;
    Dijkstra dijkstra;

    public List<DijkstraLink> linkList = new List<DijkstraLink>();

    public DijkstraNode(float _x, float _y)
    {
        x = _x;
        y = _y;
    }
}

/*
 * ダイクストラ法によりスタート地点からゴール地点までの最短経路探索を行うクラス
 */
public class Dijkstra
{
    LinkList ListMaster;
    public DijkstraNode StartNode;
    public DijkstraNode EndNode;
    public List<DijkstraNode> nodeCompList = new List<DijkstraNode>();


    public Dijkstra()
    {
        ListMaster = GameObject.Find("LinkList").GetComponent<LinkList>();
    }

    /*
    * 最短経路探索を実行するメソッド
    * return 探索回数
    */
    public int Execute(DijkstraNode startNode, DijkstraNode endNode)
    {
        StartNode = startNode;
        EndNode = endNode;
        if (startNode == null) return 0;

        // 全節点で距離を無限大，未確定とする
        foreach (DijkstraNode node in ListMaster.dijkstraNodeList)
        {
            node.Distance = int.MaxValue;
            node.Status = DijkstraNode.NodeStatus.NotYet;
            node.SourceNode = null;
            node.linkList = ListMaster.dijkstraLinkList;

            //To Do   startNodeとEndNodeがListMasterとは別物になってしまっているため、ListMaster内に組み込む
            if (startNode.x == node.x && startNode.y == node.y)
            {
                startNode = node;
            }
            if (EndNode.x == node.x && EndNode.y == node.y)
            {
                EndNode = node;
            }
        }

        // 始点では距離はゼロ，確定とする
        startNode.Distance = 0;
        startNode.Status = DijkstraNode.NodeStatus.Completed;
        nodeCompList.Add(startNode);
        startNode.SourceNode = null;

        //To Do 先ほどのToDo同様、startNodeとEndNodeをListMaster内に組み込む
        foreach (DijkstraNode node in ListMaster.dijkstraNodeList)
        {
            if (startNode.x == node.x && startNode.y == node.y)
            {
                node.Distance = 0;
                node.Status = DijkstraNode.NodeStatus.Completed;
                node.SourceNode = null;
                break;
            }
        }

        DijkstraNode scanNode = startNode;
        int nCount = 0;

        while (scanNode != null && EndNode.Status != DijkstraNode.NodeStatus.Completed)
        {
            UpdateNodeProp(scanNode);       // 隣接点のノードを更新
            scanNode = FindMinNode();       // 最短経路をもつノードを検索
            nCount++;
        }
        return nCount;
    }

    /*
    *指定ノードに隣接するノードの最短距離を計算するメソッド 
    * 引数　指定ノード
    */
    public void UpdateNodeProp(DijkstraNode sourceNode)
    {
        if (sourceNode == null) return;

        DijkstraNode destinationNode;
        float dTotalDistance;

        //リンクリストの中から指定ノードに関連しているものを検索
        foreach (DijkstraLink link in ListMaster.dijkstraLinkList)
        {
            destinationNode = null;
            //link内にsourceNodeがあるか判別する処理
            if (link.Node1.x == sourceNode.x && link.Node1.y == sourceNode.y)
            {
                link.flag = 1;
                destinationNode = link.Node2;
            }
            else if (link.Node2.x == sourceNode.x && link.Node2.y == sourceNode.y)
            {
                link.flag = 2;
                destinationNode = link.Node1;
            }
            else
            {
                continue;
            }

            // 隣接ノードを見つけた際の処理
            if (destinationNode.Status == DijkstraNode.NodeStatus.Completed) continue;

            // ノードの現在の距離に現在取り出しているlinkの距離を加える
            dTotalDistance = sourceNode.Distance + link.LinkDistance;
            if (destinationNode.Distance <= dTotalDistance) continue;

            // 現在の仮の最短距離よりもっと短いルートを見つけた際の処理
            destinationNode.Distance = dTotalDistance;  // 仮の最短距離
            destinationNode.SourceNode = sourceNode;
            destinationNode.Status = DijkstraNode.NodeStatus.Temporary;
        }       //foreach終わり
    }

    /*
    * 未確定ノードの中から最短経路を持つノードを検索するメソッド
    * return 最短経路を持つノード
    */
    public DijkstraNode FindMinNode()
    {
        double dMinDistance = int.MaxValue; // 最小値を最初無限大とする
        List<DijkstraNode> delNodeList = new List<DijkstraNode>();

        DijkstraNode Finder = null;
        DijkstraNode DestinationNode;

        // 確定しているノードをチェック
        foreach (DijkstraNode node in nodeCompList)
        {
            int i = 0;
            foreach (DijkstraLink link in node.linkList)
            {
                if (link.flag == 1)
                {
                   DestinationNode = link.Node2;
                }
                else if(link.flag == 2)
                {
                    DestinationNode = link.Node1;
                }
                else
                {
                    continue;
                }

                // 検索先が確定したノードの場合は無視
                if (DestinationNode.Status == DijkstraNode.NodeStatus.Completed)
                {
                    i++;
                    continue;
                }
                // 未確定のノードの中で最短距離のノードを探す
                if (DestinationNode.Distance >= dMinDistance) continue;

                dMinDistance = DestinationNode.Distance;
                Finder = DestinationNode;
            }
            if (i == node.linkList.Count) delNodeList.Add(node);
        }
        foreach (DijkstraNode no in delNodeList)
        {
            nodeCompList.Remove(no);
        }
        if (Finder == null) return null;

        // 最短距離を見つけた,したがってこのノードを確定とする
        Finder.Status = DijkstraNode.NodeStatus.Completed;
        nodeCompList.Add(Finder);

        //FinderがEndNodeであった場合の処理
        if(Finder.x == EndNode.x && Finder.y == EndNode.y)
        {
            EndNode = Finder;
            return EndNode;
        }
        return Finder;
    }

    /*
    * 探索したノードを基に最短経路(リンクの連なり)を持ったリストを作成するメソッド
    * return  最短経路を持ったリスト
    */
    public List<DijkstraLink> FindRoute()
    {
        List<DijkstraNode> doneNodeList = new List<DijkstraNode>();
        List<DijkstraLink> routeList = new List<DijkstraLink>();

        DijkstraNode node = EndNode;
        while (node != null && node.SourceNode != null)
        {
            doneNodeList.Add(node);
            node = node.SourceNode;
        }

        doneNodeList.Add(node);
        //EndNode→StartNodeの準でリストに保存されているためReverse()
        doneNodeList.Reverse();

        DijkstraNode node1, node2;
        for (int i = 0; i < doneNodeList.Count - 1; i++)
        {
            node1 = doneNodeList[i];
            node2 = doneNodeList[i + 1];
            foreach (DijkstraLink link in ListMaster.dijkstraLinkList)
            {
                //To Do このif文の書き方をスマートにする（可読性を上げる）
                if (node1.x == link.Node1.x && node1.y == link.Node1.y &&
                    node2.x == link.Node2.x && node2.y == link.Node2.y ||
                    node2.x == link.Node1.x && node2.y == link.Node1.y &&
                    node1.x == link.Node2.x && node1.y == link.Node2.y)
                {
                    node2.ID = link.linkID;
                }
            }
            foreach (DijkstraLink route in node1.linkList)
            {
                if (route.Node2.x == node2.x && route.Node2.y == node2.y && route.linkID == node2.ID)
                {
                    routeList.Add(route);
                    break;
                }
            }
        }
        return routeList;
    }
}

