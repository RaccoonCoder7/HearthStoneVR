using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterState : Photon.PunBehaviour
{

    public Text txt;
    public Text txt1;
    public GameObject enemy;
    public Image img;
    public string HPTxt;
    public string attackTxt;

    // Start is called before the first frame update
    void Start()
    {
        txt.text = HPTxt;
        txt1.text = attackTxt;
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
        else
        {
            txt.text = point.ToString();
        }
        
    }

    IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 30; i++)
        { 
            enemy.transform.position += new Vector3(0, -0.1f, 0);
        yield return new WaitForSeconds(0.05f);
        }
        Destroy(enemy);
    }
}
