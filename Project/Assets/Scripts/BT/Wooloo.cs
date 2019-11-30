using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
public class Wooloo : MonoBehaviour
{
    public List<Node> Paths;

    private void Awake()
    {
        GetComponent<PandaBehaviour>().Tick();
    }
    [Task]
    public void GetEndPosition()
    {
        Vector3 target = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
        Pathfinding.instance.FindPath(transform.position, target);
        Paths = Pathfinding.instance.PublicPath;
        Debug.Log("Finished");
    }
    [Task]
    public void Walking()
    {

    }

}
