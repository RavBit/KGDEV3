using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    //Basic grid values
    [SerializeField]
    public int GridX;
    [SerializeField]
    public int GridY;

    public bool IsWalkable;     // Check if Node is walkable

    public bool isFood;         // Check if Node is food

    public Vector3 Position;

    public Node Parent;        // Next node to create path with

    public int GCost;
    public int HCost;

    public int FConst { get { return (GCost + HCost); }}


    //Constructor for Node
    public Node(bool a_IsWalkable, bool _isFood, Vector3 a_Position, int a_GridX, int a_GridY)
    {
        IsWalkable = a_IsWalkable;
        isFood = _isFood;
        Position = a_Position;
        GridX = a_GridX;
        GridY = a_GridY;
    }
}
