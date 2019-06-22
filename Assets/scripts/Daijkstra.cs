using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;

/// <summary>ブランチクラス</summary>
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

/// <summary>ノードクラス</summary>
public class DijkstraNode
{
    /// <summary>最短経路確定状態の列挙体</summary>
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

/// <summary>ダイクストラ法アルゴリズム実装</summary>
public class Dijkstra
{
    LinkList ListMaster;
    public DijkstraNode StartNode;
    public DijkstraNode EndNode;
    public List<DijkstraNode> nodeCompList = new List<DijkstraNode>();
    /// <summary>コンストラクタ</summary>
    /// <param name="nNodeCount">全ノード数</param>
    public Dijkstra()
    {
        ListMaster = GameObject.Find("LinkList").GetComponent<LinkList>();
    }


    /// <summary>最短経路計算実行</summary>
    /// <param name="nStart">スタートノードのインデックス</param>
    /// <returns>検索回数</returns>
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

        foreach(DijkstraNode node in ListMaster.dijkstraNodeList)
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

    /// <summary>指定ノードに隣接するノードの最短距離を計算する。</summary>
    /// <param name="sourceNode">指定ノード</param>
    public void UpdateNodeProp(DijkstraNode sourceNode)
    {
        if (sourceNode == null) return;

        DijkstraNode destinationNode;
        float dTotalDistance;

        // ブランチリストの中から指定ノードに関連しているものを検索
        foreach (DijkstraLink link in ListMaster.dijkstraLinkList)
        {
            destinationNode = null;
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

    /// <summary>未確定ノードの中で最短経路をもつノードを検索</summary>
    /// <returns>最短経路をもつノード</returns>
    public DijkstraNode FindMinNode()
    {
        double dMinDistance = int.MaxValue; // 最小値を最初無限大とする
        List<DijkstraNode> delNodeList = new List<DijkstraNode>();

        DijkstraNode Finder = null;
        DijkstraNode DestinationNode;
        // 全てのノードをチェック
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

                // 確定したノードは無視
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

        // 最短距離を見つけた。このノードは確定
        Finder.Status = DijkstraNode.NodeStatus.Completed;
        nodeCompList.Add(Finder);
        //Finderの座標がEndNoteと同じだったとしてもFinder側はそれがわからないのでEndNoteの処理をしてあげる必要がある
        if(Finder.x == EndNode.x && Finder.y == EndNode.y)
        {
            EndNode = Finder;
            return EndNode;
        }
        return Finder;
    }


    //リンクを取得するメソッド
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

