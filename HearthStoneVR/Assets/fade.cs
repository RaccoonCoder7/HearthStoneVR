using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fade : MonoBehaviour
{
    public Image img;

    void Start()
    {
        img = GetComponent<Image>();
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn() {
      Color color = img.color;
      while (color.a < 1f) {
         color.a += 0.05f;
         img.color = color;
         yield return new WaitForSeconds(0.1f);
      }
        yield return new WaitForSeconds(0.5f);
   }
}
