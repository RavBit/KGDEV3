using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Grid grid;
    private List<Node> finalPath;


    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void FindPath(Vector3 a_StartPosition, Vector3 a_TargetPosition)
    {
        //Finds the nodes that are on the worldposition of the requested positions
        Node StartNode = grid.NodeFromWorldPostion(a_StartPosition);
        Node TargetNode = grid.NodeFromWorldPostion(a_TargetPosition);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);


        while (OpenList.Count > 0)
        {
            //Loop through list with the first node
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                //If the F-Cost is less or equal than the current node it's f-cost
                if (OpenList[i].FConst < CurrentNode.FConst || OpenList[i].FConst == CurrentNode.FConst && OpenList[i].HCost < CurrentNode.HCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            //Remove from open list + Add to closed list
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            //If reached the target run the next function
            if(CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            //Check the neighbors of the node
            foreach (Node NeighborNode in grid.GetNeighboringNodes(CurrentNode))
            {
                //If this object is not a wall and if it has already been checked
                if(!NeighborNode.IsWalkable || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                
                //Get the Fcost of the neighbor
                int MoveCost = CurrentNode.GCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if (!OpenList.Contains(NeighborNode) || MoveCost < NeighborNode.FConst)
                {
                    NeighborNode.GCost = MoveCost;
                    NeighborNode.HCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.Parent = CurrentNode; //Parent to retrace in next function

                    if(!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }

    //Retrace the steps and reverse path
    private void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = a_EndNode;

        //Retrace with parents to the start position
        while(CurrentNode != a_StartingNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        FinalPath.Reverse();
        //Add target to the list to make sure the player reaches it's target (not only the path)
        FinalPath.Add(a_EndNode);
        finalPath = new List<Node>();
        finalPath = FinalPath;
        grid.FinalPath = FinalPath;
        //Return to the entity with the final path
        PathfindingManager.instance.ReturnPathToEntity(FinalPath);
        
    }
    
    //Create a dictionary that will return the closest food to the Sheep
    public Node ReturnFoundFood(Vector3 curPosition)
    {
        Dictionary<int, Node> foodNodes = new Dictionary<int, Node>();
        for (int x = 0; x < grid.GridWorldSize.x / 2; x++)
        {
            for (int y = 0; y < grid.GridWorldSize.y / 2; y++)
            {
                if (grid.grid[x, y].isFood)
                {
                    //Calculate GetManhattenDistance between the nodes
                    int dist = GetManhattenDistance(grid.NodeFromWorldPostion(curPosition), grid.grid[x, y]);
                    if (!foodNodes.ContainsKey(dist) && dist < 5)
                        foodNodes.Add(dist, grid.grid[x, y]);
                }
            }
        }
        //If there is no food return an empty node
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

