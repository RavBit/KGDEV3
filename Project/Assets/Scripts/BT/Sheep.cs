using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using DG.Tweening;
public class Sheep : MonoBehaviour
{
    public SheepUI SheepUI;
    private List<Node> pathpositions;

    [Range(1, 10)]
    public float Basespeed = 5;

    private float hiddenSpeed = 0;

    [Range(0, 100)]
    public float Energy = 100;

    [Task]
    private bool isRoaming = false;
    [Task]
    private bool isFollowingPath = false;
    [Task]
    public bool isHungry = false;

    [Task]
    public bool isStun = false;

    [Task]
    private bool foundFood = false;

    private Vector3 startPosition;
    private Vector3 endPosition;


    [Task]
    private void CheckStats()
    {
        isHungry = Energy < 40 ? true : false;
        if(isHungry)
        {
            SheepUI.BlinkHungerIcon(true);
        }
        Task.current.Succeed();
    }

    [Task]
    private void Idle()
    {
        Debug.Log("Idling");
        Task.current.Succeed();
    }

    [Task]
    private void MakeSound()
    {
        Debug.Log("BAAHHH");
        Task.current.Succeed();
    }


    [Task]
    private void DefineEndPoint()
    {
        hiddenSpeed = (Basespeed * (Energy * 0.01f));
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
    }

    [Task]
    private void ResetHunger()
    {
        isHungry = false;
        foundFood = false;
        SheepUI.BlinkHungerIcon(false);
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
        StopCoroutine("MoveSheep");
        StartCoroutine("MoveSheep");
        isFollowingPath = true;
        Task.current.Succeed();

    }

    IEnumerator MoveSheep()
    {
        Vector3 _target;
        int listcount = 0;
        _target = pathpositions[listcount].Position;
        while (true)
        {
            if(listcount >= pathpositions.Count || isStun)
            {
                CostEnergy(-(UnityEngine.Random.Range(1.0f, 2.5f) * listcount));
                pathpositions.Clear();
                isFollowingPath = false;
                yield break;
            }
            if(transform.position == _target)
            {
                _target = pathpositions[listcount].Position;
                listcount++;
            }
            transform.LookAt(_target);
            transform.position = Vector3.MoveTowards(transform.position, _target, hiddenSpeed * Time.deltaTime);
            yield return null;
        }
    }

    [Task]
    private void SearchFood()
    {
        Tuple<Vector3, bool> _foundfood = PathfindingManager.instance.FindClosestFood(transform.position);
        if (!_foundfood.Item2)
            Task.current.Fail();
        if (_foundfood.Item2)
        {
            foundFood = _foundfood.Item2;
            endPosition = _foundfood.Item1;
            startPosition = transform.position;
            hiddenSpeed = (Basespeed * (Energy * 0.01f));
            Task.current.Succeed();
        }
    }

    //[Task]
    public void CostEnergy(float amount)
    {
        Energy += amount;
        //Task.current.Succeed();
    }

    [Task]
    public void Starving()
    {
        transform.DOShakeRotation(3, 1, 5, 50, false);
        Task.current.Succeed();
    }
}
