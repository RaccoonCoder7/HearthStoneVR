using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Photon.PunBehaviour
{
    public Image img;
    public GameObject canvas;
    public AudioClip[] clips;

    private AudioSource audioSource;
    private bool startGame;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        audioSource.clip = clips[0];
        audioSource.Play();
    }

    public void ChngeMusic(int musicNum, bool returnToOrigin){
        float originalTime = audioSource.time;
        float length = clips[musicNum].length;

        audioSource.Stop();
        audioSource.clip = clips[musicNum];
        audioSource.Play();
        if(returnToOrigin){
            StartCoroutine(returnToBGM(length, originalTime));
        }
    }

    IEnumerator returnToBGM(float length, float originalTime){
        yield return new WaitForSeconds(length);
        audioSource.Stop();
        audioSource.clip = clips[0];
        audioSource.time = originalTime;
        audioSource.Play();
    }
}
