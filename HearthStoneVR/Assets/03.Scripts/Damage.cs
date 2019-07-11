using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : Photon.PunBehaviour
{
    public GameObject canvas;
    public Image img;
    public Text txt;
    public Text minusTxt;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.isMine)
        {
            gameObject.transform.Rotate(new Vector3(0, -90, 0));

        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, 90, 0));

        }
        StartCoroutine("FadeIn");


    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1);
        Color color = img.color;
        Color txtColor = txt.color;
        while (color.a > 0.0f)
        {
            color.a -= 0.07f;
            txtColor.a -= 0.07f;
            img.color = color;
            txt.color = txtColor;
            minusTxt.color = txtColor;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
    void SetDamage(float point)
    {
        txt.text = point.ToString();
    }
}
