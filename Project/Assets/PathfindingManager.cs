using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PathfindingManager : MonoBehaviour
{
    public Queue<PathContainer> PathContainerQueue;
    private PathContainer curPathContainer;

    private bool isRunningPathCalc = false;
    //Singleton for PathfindingManager to call from different entities
    public static PathfindingManager instance;

    public Pathfinding Pathfinding;
    // Start is called before the first frame update
    void Start()
    {
        //Check of PathfindingManager exist
        if (instance != null)
            Debug.LogError("More than one PathfindingManager in scene");
        else
        {
            PathContainerQueue = new Queue<PathContainer>();
            instance = this;
        }

    }

    public static void FindPathWithPositions(Vector3 _beginPos, Vector3 _endPos, Action<List<Node>> callPath)
    {
        PathContainer PC = new PathContainer(_beginPos, _endPos, callPath);
        instance.PathContainerQueue.Enqueue(PC);
        Debug.Log("Path queued" + PC.beginPos + " / endpos: " + PC.endPos);
        instance.RunPathQueue();
    }

    public bool CheckPositions(Vector3 _endPosition)
    {
        if (Physics.CheckSphere(_endPosition, GetComponent<Grid>().NodeRadius, GetComponent<Grid>().WallMask))
        {
            return false;
        }
        return true;
    }

    public void RunPathQueue()
    {
        if(PathContainerQueue.Count > 0 && !isRunningPathCalc)
        {
            isRunningPathCalc = true;
            curPathContainer = PathContainerQueue.Dequeue();
            Debug.Log("First object" + curPathContainer.endPos);
            Pathfinding.FindPath(curPathContainer.beginPos, curPathContainer.endPos);

        }
    }

    public void ReturnPathToEntity(List<Node> _foundpath)
    {
        curPathContainer.FinalPath(_foundpath);
        isRunningPathCalc = false;
        RunPathQueue();
    }

    public Tuple<Vector3, bool> FindClosestFood(Vector3 curPosition)
    {
        Node foundFood = instance.Pathfinding.ReturnFoundFood(curPosition);
        if(!foundFood.isFood)
        {
            return new Tuple<Vector3, bool>(Vector3.zero, false);
        }
        return new Tuple<Vector3, bool>(foundFood.Position, foundFood.isFood);
        
    }
}


public struct PathContainer
{
    public Vector3 beginPos;
    public Vector3 endPos;
    public Action<List<Node>> FinalPath;

    public PathContainer(Vector3 _beginpos, Vector3 _endpos, Action<List<Node>> _finalpath)
    {
        this.beginPos = _beginpos;
        this.endPos = _endpos;
        this.FinalPath = _finalpath;
    }
}