using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Panda;
public class Wolf : MonoBehaviour
{
    private List<Node> pathpositions;
    [Range(1, 5)]
    public int Health = 0;
    [Range(1, 10)]
    public float Basespeed = 5;

    private float hiddenSpeed = 0;

    [SerializeField]
    private WolfSurround wolfSurroundings;

    [Task]
    private bool isRoaming = false;
    [Task]
    public bool checkSurroundings = false;
    [Task]
    private bool isFollowingPath = false;

    [Task]
    public bool isHit = false;

    [Task]
    private bool foundFood = false;

    private Vector3 startPosition;
    private Vector3 endPosition;


    [Task]
    private void CheckStats()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        Task.current.Succeed();
    }

    [Task]
    private void CheckHit()
    {
        if(isHit)
        {
            Health--;
            isHit = false;
            Task.current.Succeed();
        }
        Task.current.Fail();
    }
    [Task]
    private void Idle()
    {
        Task.current.Succeed();
    }

    [Task]
    private void MakeSound()
    {
        Task.current.Succeed();
    }
    [Task]
    private void DefineEndPoint()
    {
        hiddenSpeed = (Basespeed * (50 * 0.01f));
        startPosition = transform.position;
        while (true)
        {
            endPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 1, UnityEngine.Random.Range(-10, 10));
            if (PathfindingManager.instance.CheckPositions(endPosition) || startPosition == endPosition)
            {
                break;
            }
        }
        Task.current.Succeed();
    }

    [Task]
    private void ResetValues()
    {
        startPosition = Vector3.zero;
        endPosition = Vector3.zero;
        isRoaming = false;
        Task.current.Succeed();
    }

    [Task]
    private void ResetHunger()
    {
        foundFood = false;
    }

    [Task]
    private void CalculatePath()
    {
        PathfindingManager.FindPathWithPositions(startPosition, endPosition, StartMovingTarget);
        isRoaming = true;
        Task.current.Succeed();
    }

    public void StartMovingTarget(List<Node> positions)
    {
        pathpositions = new List<Node>();
        pathpositions = positions;
        StopCoroutine("MoveWolf");
        StartCoroutine("MoveWolf");
        isFollowingPath = true;
        Task.current.Succeed();

    }
    [Task]
    private void ToggleFoundFood()
    {
        foundFood = !foundFood;
        Task.current.Succeed();
    }

    IEnumerator MoveWolf()
    {
        Vector3 _target;
        int listcount = 0;
        _target = pathpositions[listcount].Position;
        while (true)
        {
            if (listcount >= pathpositions.Count)
            {
                pathpositions.Clear();
                isFollowingPath = false;
                yield break;
            }
            if (transform.position == _target)
            {
                _target = pathpositions[listcount].Position;
                listcount++;
            }
            transform.LookAt(_target);
            transform.position = Vector3.MoveTowards(transform.position, _target, 1 * Health * Time.deltaTime);
            yield return null;
        }
    }


    [Task]
    private void ToggleSurroundings(bool toggle)
    {
        checkSurroundings = toggle;
        wolfSurroundings.gameObject.SetActive(toggle);
        Task.current.Succeed();
    }

    [Task]
    private void TargetSheep()
    {
        if(wolfSurroundings.Sheep != null)
        {
            endPosition = wolfSurroundings.Sheep.transform.position;
            Task.current.Succeed();
            return;
        }
        Task.current.Fail();
    }

    [Task]
    private void EatSheep()
    {
        if(wolfSurroundings == null)
        {
            Task.current.Fail();
            return;
        }
        Destroy(wolfSurroundings.Sheep);
        Task.current.Succeed();
    }
}
