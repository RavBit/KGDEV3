using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform StartPosition;
    public LayerMask WallMask;
    public LayerMask FoodMask;
    public Vector2 GridWorldSize;
    public float NodeRadius;
    public float Distance;

    public Node[,] grid;

    public List<Node> FinalPath;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = NodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void Update()
    {
        // Update the grid on changes
        if(grid != null)
        {
            UpdateGrid();
        }
    }

    // Create a grid with the following obstacles
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;
        for (int y = 0; y < gridSizeX; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + NodeRadius) + Vector3.forward * (y * nodeDiameter + NodeRadius);

                bool Wall = true;
                bool isFood = false;
                //Check if there's a wall
                if(Physics.CheckSphere(worldPoint, NodeRadius, WallMask))
                {
                    Wall = false;
                }
                //Check if there's food
                if (Physics.CheckSphere(worldPoint, 0.5f, FoodMask))
                {
                    Wall = true;
                    isFood = true;
                }
                grid[x, y] = new Node(Wall, isFood, worldPoint, x, y);
            }
        }
    }

    //Check the grid data. If the node is changed it will be changed in this function
    private void UpdateGrid()
    {
        foreach (Node node in grid)
        {
            if(Physics.CheckSphere(node.Position, NodeRadius, WallMask))
            {
                node.IsWalkable = false;
            }
            if (Physics.CheckSphere(node.Position, NodeRadius, FoodMask))
            {
                node.IsWalkable = true;
                node.isFood = true;
            }
            else
            {
                node.IsWalkable = true;
                node.isFood = false;
            }
        }
    }

    // Get neighbor nodes of the current node
    public List<Node> GetNeighboringNodes(Node a_Node)
    {
        List<Node> NeighboringNodes = new List<Node>();
        int xCheck;
        int yCheck;

        //Right Side
        xCheck = a_Node.GridX + 1;
        yCheck = a_Node.GridY;
        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Left Side
        xCheck = a_Node.GridX - 1;
        yCheck = a_Node.GridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Top Side
        xCheck = a_Node.GridX;
        yCheck = a_Node.GridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Bottom Side
        xCheck = a_Node.GridX;
        yCheck = a_Node.GridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        return NeighboringNodes;

    }

    // Returns the worldposition of the nodes
    public Node NodeFromWorldPostion(Vector3 a_WorldPosition)
    {
        float xpoint = ((a_WorldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x);
        float ypoint = ((a_WorldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y);

        xpoint = Mathf.Clamp01(xpoint);
        ypoint = Mathf.Clamp01(ypoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xpoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * ypoint);

        return grid[x, y];

    }

    //Gizmo for debugging (red very buggy due multiple paths created)
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                if (n.IsWalkable)
                {
                    Gizmos.color = Color.white;
                }
                if (n.isFood)
                {
                    Gizmos.color = Color.green;
                }
                if(!n.IsWalkable && !n.isFood)
                {
                    Gizmos.color = Color.yellow;
                }


                if (FinalPath != null)
                {
                    if (FinalPath.Contains(n))
                    {
                        Gizmos.color = Color.red;
                    }

                }


                Gizmos.DrawCube(n.Position, Vector3.one * (nodeDiameter - Distance));
            }
        }
    }

}
