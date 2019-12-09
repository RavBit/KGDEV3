using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
public class Wooloo : MonoBehaviour
{
    public List<Node> Paths;

    //Check variables
    [Task]
    [SerializeField]
    private bool hasTarget = false;

    [Task]
    private bool isCaught = false;


    private Vector3 target;
    private void Awake()
    {
        GetComponent<PandaBehaviour>().Tick();
    }

    private void Update()
    {
        Debug.Log("State: " + GetComponent<PandaBehaviour>().status.ToString());
    }

    [Task]
    private void CalculatePosition()
    {
        Debug.Log("Calculate Position");
        target = new Vector3(Random.Range(-15, 15), 0, Random.Range(-15, 15));
        CheckPath();
        hasTarget = true;
        Task.current.Succeed();
    }
    [Task]
    private void NextStep()
    {
        float speed = 10f;
        Vector3 _target = new Vector3();
        if (Paths.Count == 0)
        {
            _target = target;
        }
        else
        {
            _target = Paths[0].Position;
        }
        transform.LookAt(_target);
        while (transform.position != _target)
        {
            transform.position = Vector3.Lerp(transform.position, _target, speed * Time.deltaTime);
            return;
        }
        transform.position = _target;
        Debug.Log("Has target: " + hasTarget);
        Task.current.Succeed();
    }
    [Task]
    private void Idle()
    {
        Debug.Log("Idle");
        //GetComponent<PandaBehaviour>().Reset();
    }

    [Task]
    private void CheckPath()
    {
        if (target == transform.position)
        {
            Debug.Log("Setting target");
            hasTarget = false;
            Task.current.Fail();
            return;
        }
        Paths.Clear();
        Pathfinding.instance.FindPath(transform.position, target);
        Paths = Pathfinding.instance.PublicPath;
        Task.current.Succeed();
    }

}
