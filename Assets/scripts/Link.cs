using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public float x, y;
    public string ID;

    public Node(float _x, float _y)
    {
        x = _x;
        y = _y;
    }
}


public class Link {

    public Node node1, node2;

    public string id;
    public float length;
    public float[] rect = new float[2];
    public List<Car> carList = new List<Car>();

    //コンストラクタ
    public Link(string _id, float _x1, float _y1, float _x2, float _y2)
    {
        id = _id;
        node1 = new Node(_x1, _y1);
        node2 = new Node(_x2, _y2);

        float x1, y1, x2, y2;
        x1 = node1.x;
        y1 = node1.y;
        x2 = node2.x;
        y2 = node2.y;

        length = Mathf.Sqrt(Mathf.Pow(x1 - x2, 2f) + Mathf.Pow(y1 - y2, 2f));
    }

    //carで進行方向を変更する際に必要なメソッド
    public float getRect(Node startNode, Node endNode)
    {
        float rect = 0;
        float x1, y1, x2, y2;

        if(length > 0)
        {
            x1 = startNode.x;
            y1 = startNode.y;
            x2 = endNode.x;
            y2 = endNode.y;

            float r1, r2;

            r1 = Mathf.Acos((y2 - y1) / length) / Mathf.PI * 180;
            r2 = x2 - x1;

            //座標位置によって角度の向きを変更
            if (r2 < 0)
            {
                rect = r1;
            }
            else
            {
                rect = -1 * r1;
            }
        }
        return rect;
    }

    
    public Node[] getStartNodes(float startX, float startY, string id)
    {
        Node[] nodes = new Node[2];

        if(startX == node1.x && startY == node1.y)
        {
            nodes[0] = node1;
            nodes[1] = node2;

        }
        else if(startX == node2.x && startY == node2.y)
        {
            nodes[1] = node1;
            nodes[0] = node2;

        }
        else
        {
            Debug.Log("not_found 404:" + id + " x:" + (startX - node1.x) + " y:" + (startY - node1.y) + " n1x:" + node1.x + " n1y:" + node1.y + " n2x:" + node2.x + " n2y:" + node2.y);
        }
        return nodes;
    }



}






