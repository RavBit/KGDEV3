using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    [Header("Main Entity that will spawn")]
    public GameObject Wooloo;

    [Header("Amount of Entities that will spawn")]
    [Range(1, 100)]
    public int Amount = 10;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Test", 1);
    }

    void Test()
    {
        SpawnEntities(Amount);
    }

    private void SpawnEntities(int _amount)
    {
        int countAmount = 0;
        while (countAmount != _amount)
        {
            int xPos = Random.Range(-10, 10);
            int zPos = Random.Range(-10, 10);
            if (Physics.CheckSphere(new Vector3(xPos, 0, zPos), 0.5f, layerMask))
            {
                return;
            }
            Instantiate(Wooloo, new Vector3(xPos, 0, zPos), Quaternion.identity);
            countAmount++;
        }
    }
}
