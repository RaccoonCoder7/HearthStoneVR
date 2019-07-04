using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonConnect : Photon.PunBehaviour
{

    string version = "1";

    public byte maxPlayersPerRoom = 2;
    public GameObject panel;
    public Image img;
    Animator anim;
    public static GameObject localPlayer;

    public Text Information;
    public GameObject JoinGameBT;


    private void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
    }

    void Start()
    {
        //버전에 맞게 로비 접속
        PhotonNetwork.ConnectUsingSettings(version);
        JoinGameBT.SetActive(false);

    }

    //룸에 들어가거나 생성
    public void JoinGameRoom()
    {
        StartCoroutine("FadeIn");
    }


    //룸에 들어오면 실행되는 함수
    public override void OnJoinedRoom()
    {
        Debug.Log("You are Joined Room!");
        

        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.LoadLevel("MainGame");
        }
    }

    public override void OnConnectedToMaster()
    {
        JoinGameBT.SetActive(true);
        Information.text = "Connected.";
    }

    //씬이 로드되면 자동으로 호출
    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("now, You are in GAME!");
        localPlayer = PhotonNetwork.Instantiate(
           "Player",new Vector3(0, 0.5f, 0),Quaternion.identity, 0);

    }

    IEnumerator FadeIn()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayersPerRoom;
        JoinGameBT.SetActive(false);
        //anim = panel.GetComponent<Animator>();
        //anim.SetBool("FADE", true);
        //yield return new WaitForSeconds(1.5f);
        //PhotonNetwork.JoinOrCreateRoom("Fighting Room", options, TypedLobby.Default);
        Color color = img.color;
        while (color.a < 0.8f)
        {
            color.a += 0.05f;
            img.color = color;
            yield return new WaitForSeconds(0.1f);
        }
        PhotonNetwork.JoinOrCreateRoom("Fighting Room", options, TypedLobby.Default);
    }



}
