using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class TestBT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Task]
    void SetColor(float r, float g, float b)
    {
        this.GetComponent<Renderer>().material.color = new Color(r, g, b);
        //this.transform.Translate(Vector3.up);
        Task.current.Succeed();
    }
}
