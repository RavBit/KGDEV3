using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleUI : MonoBehaviour
{
    public Canvas canvas;

    public GameObject[] Bullets;

    public void SetActiveBullets(int amount)
    {
        foreach (GameObject Bullet in Bullets)
        {
            Bullet.SetActive(false);
        }
        for (int i = 0; i < amount; i++)
        {
            Bullets[i].SetActive(true);
        }
    }
}
