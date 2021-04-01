using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public List<Vector3> PathToTarget;
    public List<Node> CheckedNodes = new List<Node>();
    public List<Node> WaitingNodes = new List<Node>();
    public List<Node> FreeNodes = new List<Node>();
    public GameObject Target;
    public LayerMask SolidLayer;

    private List<Node> GetNeighboursNodes(Node node)
    {
        var neighbours = new List<Node>();

        var leftNode = new Vector3(node.Position.x - 1, node.Position.y, node.Position.z);
        var rightNode = new Vector3(node.Position.x + 1, node.Position.y, node.Position.z);
        var upNode = new Vector3(node.Position.x, node.Position.y, node.Position.z + 1);
        var downNode = new Vector3(node.Position.x, node.Position.y, node.Position.z - 1);

        neighbours.Add(new Node(leftNode, node.TargetPosition, node, node.G + 1));
        neighbours.Add(new Node(rightNode, node.TargetPosition, node, node.G + 1));
        neighbours.Add(new Node(upNode, node.TargetPosition, node, node.G + 1));
        neighbours.Add(new Node(downNode, node.TargetPosition, node, node.G + 1));
        return neighbours;
    }

    public List<Vector3> CalculatePathFromNode(Node node)
    {
        var path = new List<Vector3>();
        Node currentNode = node;

        while (currentNode.PrevNode != null)
        {
            path.Add(new Vector3(currentNode.Position.x, currentNode.Position.y, currentNode.Position.z));
            currentNode = currentNode.PrevNode;
        }

        return path;
    }

    private bool IsEqualVector(Vector3 obj1, Vector3 obj2)
    {
        return Mathf.Abs(obj1.x - obj2.x) < Mathf.Epsilon && Mathf.Abs(obj1.y - obj2.y) < Mathf.Epsilon && Mathf.Abs(obj1.z - obj2.z) < Mathf.Epsilon;
    }

    public List<Vector3> GetPath(Vector3 target)
    {
        PathToTarget = new List<Vector3>();
        CheckedNodes = new List<Node>();
        WaitingNodes = new List<Node>();

        Vector3 StartPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        Vector3 TargetPosition = new Vector3(Mathf.Round(Target.transform.position.x), Mathf.Round(Target.transform.position.y), Mathf.Round(Target.transform.position.z));

        if (IsEqualVector(StartPosition, TargetPosition)) return PathToTarget;

        Node startNode = new Node(StartPosition, TargetPosition, null, 0);
        CheckedNodes.Add(startNode);
        WaitingNodes.AddRange(GetNeighboursNodes(startNode));

        while (WaitingNodes.Count > 0)
        {
            Node nodeToCheck = WaitingNodes.Where(x => x.F == WaitingNodes.Min(y => y.F)).FirstOrDefault();

            if (IsEqualVector(nodeToCheck.Position, TargetPosition))
            {
                return CalculatePathFromNode(nodeToCheck);
            }

            var walkable = (Physics.OverlapSphere(nodeToCheck.Position, 0.1f, SolidLayer).Length == 0); //Physics.Raycast(new Ray(nodeToCheck.PrevNode.Position, nodeToCheck.Position), 1f, SolidLayer);//
            nodeToCheck.IsWalkable = walkable;
            if (!walkable)
            {
                WaitingNodes.Remove(nodeToCheck);
                CheckedNodes.Add(nodeToCheck);
            }
            else if (walkable)
            {
                WaitingNodes.Remove(nodeToCheck);
                if (!CheckedNodes.Where(x => IsEqualVector(x.Position, nodeToCheck.Position)).Any())
                {
                    CheckedNodes.Add(nodeToCheck);
                    WaitingNodes.AddRange(GetNeighboursNodes(nodeToCheck));
                }
            }
        }
        FreeNodes = CheckedNodes;

        return PathToTarget;
    }
    void OnDrawGizmos()
    {
        foreach (var item in CheckedNodes)
        {
            if (item.IsWalkable)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(item.Position.x, item.Position.y, item.Position.z), 0.2f);
        }
        if (PathToTarget != null)
            foreach (var item in PathToTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector3(item.x, item.y, item.z), 0.3f);
            }
    }
    void Start()
    {
       
    }

    void Update()
    {
        /*
        Node n = new Node(transform.position, Target.transform.position, null, 0);
        var neighbours = GetNeighboursNodes(n);
        Debug.Log(neighbours);
        Debug.Log("left = " + (Physics.OverlapSphere(neighbours[0].Position, 0.1f, SolidLayer).Length == 0));
        Debug.Log("right = " + (Physics.OverlapSphere(neighbours[1].Position, 0.1f, SolidLayer).Length == 0));
        Debug.Log("up = " + (Physics.OverlapSphere(neighbours[2].Position, 0.1f, SolidLayer).Length == 0));
        Debug.Log("down = " + (Physics.OverlapSphere(neighbours[3].Position, 0.1f, SolidLayer).Length == 0)); */       
        //PathToTarget = GetPath(Target.transform.position);
    }
}
