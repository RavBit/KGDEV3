using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wolf")
        {
            if(other.GetComponent<Wolf>().checkSurroundings)
            {
                other.GetComponent<Wolf>().isHit = true;
                Destroy(this.gameObject);
            }
        }
    }
}
