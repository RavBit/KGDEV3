using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoolooTest : MonoBehaviour
{
    public int Speed = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(Speed * Time.deltaTime, 0, 0));
    }
}
