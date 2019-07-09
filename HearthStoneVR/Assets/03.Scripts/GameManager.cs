using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour
{
    public Image img;
    public GameObject canvas;
    public AudioClip[] clips;

    private AudioSource audioSource;
    private bool startGame;
    public GameObject waitingText;
    public GameObject txt;
    public GameObject photonManager;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    
        photonManager = GameObject.Find("photonManager");
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
        startGame = true;
        Color color = img.color;
        audioSource.clip = clips[0];
        audioSource.Play();
        while (color.a > 0.0f)
        {
            color.a -= 0.03f;
            img.color = color;
            yield return new WaitForSeconds(0.1f);
        }
        waitingText.SetActive(false);
        canvas.SetActive(false);
    }

    public void ChngeMusic(int musicNum, bool returnToOrigin){
        float originalTime = audioSource.time;
        float length = clips[musicNum].length;
        audioSource.PlayOneShot(clips[musicNum]);
        // audioSource.Stop();
        // audioSource.clip = clips[musicNum];
        // audioSource.Play();
        // if(returnToOrigin){
        //     StartCoroutine(returnToBGM(length, originalTime));
        // }
    }

    IEnumerator returnToBGM(float length, float originalTime){
        yield return new WaitForSeconds(length);
        audioSource.Stop();
        audioSource.clip = clips[0];
        audioSource.time = originalTime;
        audioSource.Play();
    }
    IEnumerator EndGame()
    {
        canvas.SetActive(true);
        waitingText.SetActive(false);
        txt.SetActive(true);
        txt.GetComponent<Text>().text = "WIN";
        yield return new WaitForSeconds(4);
        LeaveRoom();
    }
    public void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        Destroy(photonManager);
        PhotonNetwork.LeaveRoom();
    }
}
