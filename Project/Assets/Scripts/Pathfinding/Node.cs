using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    [SerializeField]
    public int GridX; // X position in the Array of Nodes
    [SerializeField]
    public int GridY; // Y position in the Array of Nodes

    public bool IsWalkable;

    public Vector3 Position;

    public Node Parent;

    public int GCost;
    public int HCost;

    public int FConst { get { return (GCost + HCost); }}

    public Node(bool a_IsWalkable, Vector3 a_Position, int a_GridX, int a_GridY)
    {
        IsWalkable = a_IsWalkable;
        Position = a_Position;
        GridX = a_GridX;
        GridY = a_GridY;
    }
}
