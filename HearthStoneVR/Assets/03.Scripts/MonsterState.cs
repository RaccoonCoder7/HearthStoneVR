using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterState : Photon.PunBehaviour
{

    public Text txt;
    public GameObject enemy;
    public Image img;
    public string HPTxt;

    // Start is called before the first frame update
    void Start()
    {
        txt.text = HPTxt;
        if (photonView.isMine)
        {
            
            gameObject.transform.Rotate(new Vector3(0, 0, -90));
        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 90));
        }
    }

    // Update is called once per frame
    void SetHealth(float point)
    {
        if (point <= 0)
        {
            txt.text = "0";
            StartCoroutine("EnemyDeath");
        }
        txt.text = point.ToString();
    }

    IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(1);
        Renderer render = enemy.GetComponentInChildren<Renderer>();
        //while (color > 0.0f)
        //{
        //    render.material.color
        //    color.a -= 0.07f;
        //    img.color = color;
        //    yield return new waitforseconds(0.1f);
        //}
        Destroy(enemy);
    }
}
