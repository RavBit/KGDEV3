using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Grid grid;
    private List<Node> finalPath;

    public Transform PlayerPosition;


    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void FindPath(Vector3 a_StartPosition, Vector3 a_TargetPosition)
    {
        Node StartNode = grid.NodeFromWorldPostion(a_StartPosition);
        Node TargetNode = grid.NodeFromWorldPostion(a_TargetPosition);

        Debug.Log("Startnode: " + StartNode.Position + " end note: " + TargetNode.Position);
        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();
        OpenList.Add(StartNode);


        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)//Loop through the open list starting from the second object
            {
                if (OpenList[i].FConst < CurrentNode.FConst || OpenList[i].FConst == CurrentNode.FConst && OpenList[i].HCost < CurrentNode.HCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];//Set the current node to that object
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if(CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }
            Debug.Log("Neighbours: " + grid.GetNeighboringNodes(CurrentNode).Count);
            foreach (Node NeighborNode in grid.GetNeighboringNodes(CurrentNode))
            {
                if(!NeighborNode.IsWalkable || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = CurrentNode.GCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if (!OpenList.Contains(NeighborNode) || MoveCost < NeighborNode.FConst)
                {
                    NeighborNode.GCost = MoveCost;
                    NeighborNode.HCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.Parent = CurrentNode;

                    if(!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }


    private void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = a_EndNode;

        while(CurrentNode != a_StartingNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        FinalPath.Reverse();
        FinalPath.Add(a_EndNode);
        finalPath = new List<Node>();
        finalPath = FinalPath;
        grid.FinalPath = FinalPath;
        PathfindingManager.instance.ReturnPathToEntity(FinalPath);
        
    }


    public List<Node> PublicPath
    {
        get
        {
            return finalPath;
        }
    }


    public Node ReturnFoundFood(Vector3 curPosition)
    {
        Dictionary<int, Node> foodNodes = new Dictionary<int, Node>();
        for (int x = 0; x < grid.GridWorldSize.x / 2; x++)
        {
            for (int y = 0; y < grid.GridWorldSize.y / 2; y++)
            {
                if (grid.grid[x, y].isFood)
                {
                    Debug.Log("adding: " + grid.grid[x, y].Position);
                    int dist = GetManhattenDistance(grid.NodeFromWorldPostion(curPosition), grid.grid[x, y]);
                    if (!foodNodes.ContainsKey(dist) && dist < 5)
                        foodNodes.Add(dist, grid.grid[x, y]);
                }
            }
        }
        if (foodNodes.Count == 0)
        {
            return new Node(false, false, Vector3.zero, 0, 0);
        }
        Node ClosestFood = foodNodes.OrderBy(kvp => kvp.Key).First().Value;
        return ClosestFood;
    }
    int GetManhattenDistance(Node a_NodeA, Node a_NodeB)
    {
        int ix = Mathf.Abs(a_NodeA.GridX - a_NodeB.GridX);
        int iy = Mathf.Abs(a_NodeA.GridY - a_NodeB.GridY);

        return ix + iy;
    }
}

