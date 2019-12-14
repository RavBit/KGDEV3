using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSurround : MonoBehaviour
{
    public GameObject Sheep;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sheep")
        {
            if (other.GetComponent<Sheep>().isHungry)
            {
                other.GetComponent<Sheep>().isStun = true;
                Sheep = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sheep")
        {
            if (other.GetComponent<Sheep>().isHungry)
            {
                other.GetComponent<Sheep>().isStun = false;
                Sheep = null;
            }
        }
    }
}
