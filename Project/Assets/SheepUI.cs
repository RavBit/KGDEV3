using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SheepUI : MonoBehaviour
{
    public Image HungerIcon;

    private bool HungerBlink;
    public void BlinkHungerIcon(bool state)
    {
        HungerBlink = state;
        StopCoroutine("Blinker");
        HungerIcon.gameObject.SetActive(state);
        if (HungerBlink)
        {
            StartCoroutine("Blinker");
        }
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
            HungerIcon.gameObject.SetActive(blink);
            Debug.Log("Blinking");
            yield return new WaitForSeconds(1);
        }
    }
}
