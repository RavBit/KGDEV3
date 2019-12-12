using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
public class Wooloo_old : MonoBehaviour
{
    [Header("Canvas Components")]
    [SerializeField]
    private GameObject SleepIcon;

    public LayerMask WallMask;

    public List<Node> Paths;

    [Range(10, 100)]
    [SerializeField]
    private float speed = 20f;
    //Check variables
    [Task]
    [SerializeField]
    private bool hasTarget = false;

    [Task]
    [SerializeField]
    private bool isHit = false;


    [Task]
    public bool isCaught = false;

    [Task]
    public bool atHerder = false;
    private bool Sleeping = false;


    private Vector3 target;
    private void Awake()
    {
        GetComponent<PandaBehaviour>().Tick();
    }

    private void Update()
    {
        Debug.Log("State: " + GetComponent<PandaBehaviour>().status.ToString());
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision: " + collision.gameObject.name);
        if(collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Bullet");
            isCaught = true;
        }
    }

    [Task]
    private void CalculatePosition()
    {
        Debug.Log("Calculate Position");
        target = new Vector3(Random.Range(-7, 7), 0, Random.Range(-7, 7));
        CheckPath();
        hasTarget = true;
        Task.current.Succeed();
    }
    [Task]
    private void NextStep()
    {
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
        //transform.position = _target;
        Debug.Log("Has target: " + hasTarget);
        Task.current.Succeed();
    }

    [Task]
    private void CheckPath()
    {
        if (Physics.CheckSphere(target, 1, WallMask))
        {
            Debug.Log("Test");
            hasTarget = false;
            Task.current.Fail();
            return;
        }
        if (target == transform.position)
        {
            Debug.Log("Setting target");
            hasTarget = false;
            Task.current.Fail();
            return;
        }
        Paths.Clear();
        //Pathfinding.instance.FindPath(transform.position, target);
        //Paths = Pathfinding.instance.PublicPath;
        Task.current.Succeed();
    }

    [Task]
    private void Sleep()
    {
        SleepIcon.SetActive(true);
        Sleeping = true;
        Debug.Log("He sleep");
        Task.current.Succeed();
    }

    [Task]
    private void WakeUp()
    {
        Debug.Log("He no longer sleeps");
        Sleeping = false;
        SleepIcon.SetActive(false);
        Task.current.Succeed();
    }

    [Task]
    private void Defend()
    {
        Debug.Log("Defending");
        Task.current.Succeed();
    }


    [Task]
    private void Continue()
    {
        isHit = !isHit;
        Task.current.Succeed();
    }

    [Task]
    private void Idle()
    {
        Debug.Log("Idle pass");
        Task.current.Succeed();
    }

    [Task]
    private void Follow()
    {
        //target = Pathfinding.instance.PlayerPosition.position;
        if(transform.position == target)
        {
            atHerder = true;
        }
        CheckPath();
        Task.current.Succeed();
    }

    [Task]
    private void Released()
    {
        isCaught = false;
        hasTarget = false;
        Task.current.Succeed();
    }

    [Task]
    private void AttachToHerder()
    {
        //transform.parent = Pathfinding.instance.PlayerPosition;
    }
}
