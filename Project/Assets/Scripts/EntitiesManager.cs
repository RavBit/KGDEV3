using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public Text AmountOfSheeps;

    public Text CombinedHappiness;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Test", 1);
        StartCoroutine("EntityManaging");
    }
    IEnumerator EntityManaging()
    {
        yield return new WaitForSeconds(5);
        while(true)
        {
            GameObject[] Sheeps = GameObject.FindGameObjectsWithTag("Sheep");
            int amount = Sheeps.Length;
            AmountOfSheeps.text = "Sheeps: " + Sheeps.Length;
            float energyLevel = 0;
            if(Sheeps.Length <= 0)
            {
                Debug.LogError("All Sheeps died!");
                yield break;
            }
            foreach (GameObject g in Sheeps)
            {
                energyLevel += g.GetComponent<Sheep>().Energy;
            }
            float happiness = energyLevel / amount;
            CombinedHappiness.text = "Sheep Energy: " + Mathf.Round(happiness);
            if (happiness > 60)
            {
                int sheepSpawn = Random.Range(2, 4);
                SpawnSheep(sheepSpawn);
                SpawnWolf(sheepSpawn / 2);

            }
            yield return new WaitForSeconds(20);
        }
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
