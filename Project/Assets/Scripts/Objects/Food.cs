using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Food : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    private int Satisfaction;

    [SerializeField]
    [Range(0, 10)]
    private int Duration;

    private void Awake()
    {
        StartCoroutine("DestroyFood");
    }

    IEnumerator DestroyFood()
    {
        int count = Duration;
        while (true)
        {
            Duration--;
            if(Duration <= 0)
            {
                Destroy(this.gameObject);
            }
            yield return new WaitForSeconds(1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sheep")
        {
            if (other.GetComponent<Sheep>().isHungry)
            {
                other.GetComponent<Sheep>().CostEnergy(Satisfaction * 5);
                Destroy(this.gameObject);
            }
        }
    }
}
