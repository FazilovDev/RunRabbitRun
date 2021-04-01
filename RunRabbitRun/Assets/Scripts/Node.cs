using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Node
{
    public Vector3 Position { get; set; }
    public Vector3 TargetPosition { get; set; }
    public Node PrevNode { get; set; }
    public int F { get; set; } // F=G*H
    public int G { get; set; } // от старта до ноды
    public int H { get; set; } // от ноды до цели
    public bool IsWalkable { get; set; }
    public Node(Vector3 nodePosition, Vector3 targetPosition, Node prevNode, int g)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PrevNode = prevNode;
        G = g;
        H = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.y - Position.y);
        F = G + H;
    }
}

