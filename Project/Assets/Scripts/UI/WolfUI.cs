using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WolfUI : MonoBehaviour
{
    public Image SheepSearchIcon;
    bool blink = false;
    private bool SheepBlink;
    private void Awake()
    {
        StopCoroutine("Blinker");
        StartCoroutine("Blinker");
    }

    public IEnumerator Blinker()
    {
        while (true)
        {
            blink = !blink;
            SheepSearchIcon.gameObject.SetActive(blink);
            yield return new WaitForSeconds(1);
        }
    }
}
