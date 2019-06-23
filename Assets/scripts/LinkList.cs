using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/*
 * リンクとノード情報を持ったcsvファイルを読み込み，リストを作成するクラス
 */
public class LinkList : MonoBehaviour
{
    public Dictionary<string, Link> linkList = new Dictionary<string, Link>();
    public List<DijkstraNode> dijkstraNodeList = new List<DijkstraNode>();
    public List<DijkstraLink> dijkstraLinkList = new List<DijkstraLink>();
    public string LINK_DATA_NAME = "Link_simple";
    DijkstraNode Node1;
    DijkstraNode Node2;

    void Start()
    {
        TextAsset csvFile = Resources.Load("CSV/" + LINK_DATA_NAME) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        string[] rows;
        reader.ReadLine();
        while (reader.Peek() > -1)
        {
            rows = reader.ReadLine().Split(',');
            linkList.Add(rows[0], new Link(rows[0], float.Parse(rows[1]), float.Parse(rows[2]), float.Parse(rows[3]), float.Parse(rows[4])));

            Node1 = new DijkstraNode(float.Parse(rows[1]), float.Parse(rows[2]));
            Node2 = new DijkstraNode(float.Parse(rows[3]), float.Parse(rows[4]));
            dijkstraNodeList.Add(Node1);
            dijkstraNodeList.Add(Node2);

            dijkstraLinkList.Add(new DijkstraLink(rows[0], Node1, Node2));

        }

    }

    void Update()
    {

    }
}


