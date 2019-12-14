using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SheepUI : MonoBehaviour
{
    public Image HungerIcon;

    private bool HungerBlink;
    private void Start()
    {
        StartCoroutine("Blinker");
    }

    public void BlinkHungerIcon(bool state)
    {
        HungerIcon.gameObject.SetActive(state);
    }

    public IEnumerator Blinker()
    {
        bool blink = false;
        while(true)
        {
            blink = !blink;
            if(!HungerBlink)
            {
                break;
            }
            HungerIcon.GetComponent<Image>().enabled = blink;
            yield return new WaitForSeconds(1);
        }
    }
}
