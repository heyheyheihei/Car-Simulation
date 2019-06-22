using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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