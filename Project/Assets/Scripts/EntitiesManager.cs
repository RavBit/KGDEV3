using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    [Header("Sheep that will spawn")]
    public GameObject Sheep;
    [Header("Wolf that will spawn")]
    public GameObject Wolf;

    [Header("Amount of Sheeps that will spawn")]
    [Range(1, 100)]
    public int SheepAmount = 10;

    [Header("Amount of Wolves that will spawn")]
    [Range(1, 100)]
    public int WolfAmount = 10;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Test", 1);
    }

    void Test()
    {
        SpawnSheep(SheepAmount);
        SpawnWolf(WolfAmount);
    }

    private void SpawnSheep(int _amount)
    {
        int countAmount = 0;
        while (countAmount != _amount)
        {
            int xPos = Random.Range(-7, 7);
            int zPos = Random.Range(-7, 7);
            if (Physics.CheckSphere(new Vector3(xPos, 0, zPos), 0.5f, layerMask))
            {
                return;
            }
            Instantiate(Sheep, new Vector3(xPos, 0, zPos), Quaternion.identity);
            countAmount++;
        }
    }

    private void SpawnWolf(int _amount)
    {
        int countAmount = 0;
        while (countAmount != _amount)
        {
            int xPos = Random.Range(-7, 7);
            int zPos = Random.Range(-7, 7);
            if (Physics.CheckSphere(new Vector3(xPos, 0, zPos), 0.5f, layerMask))
            {
                return;
            }
            Instantiate(Wolf, new Vector3(xPos, 0, zPos), Quaternion.identity);
            countAmount++;
        }
    }
}
