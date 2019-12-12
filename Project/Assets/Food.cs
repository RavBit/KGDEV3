using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    private int Satisfaction;

    [SerializeField]
    [Range(0, 10)]
    private int Duration;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sheep")
        {
            if (other.GetComponent<Sheep>().isHungry)
            {
                other.GetComponent<Sheep>().CostEnergy(Satisfaction * 10);
                Destroy(this.gameObject);
            }
        }
    }
}
