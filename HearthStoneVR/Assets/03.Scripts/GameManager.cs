using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Photon.PunBehaviour
{
    public Image img;
    public GameObject canvas;
    bool startGame;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.room.PlayerCount == 2 && startGame == false)
        {
            StartCoroutine("FadeOut");
        }
        
    }

    IEnumerator FadeOut()
    {
        Color color = img.color;
        while (color.a > 0.0f)
        {
            color.a -= 0.05f;
            img.color = color;
            yield return new WaitForSeconds(0.5f);
        }
        canvas.SetActive(false);
        startGame = true;


    }
}
