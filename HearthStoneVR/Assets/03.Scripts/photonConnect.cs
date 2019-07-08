using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonConnect : Photon.PunBehaviour
{

    string version = "1";

    public byte maxPlayersPerRoom = 2;
    public GameObject canvas;
    public Image img;
    public static GameObject Player;
    public static GameObject PlayerModel;

    public Text Information;
    public GameObject JoinGameBT;

    public static photonConnect instance;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

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
        canvas.SetActive(true);
        
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
        if (PhotonNetwork.isMasterClient)
        {
            Player = PhotonNetwork.Instantiate(
            "OVRCameraRig", new Vector3(0, 26.55235f, -13.89566f), Quaternion.identity, 0);
            PlayerModel = PhotonNetwork.Instantiate(
            "Player1", new Vector3(0, 0, 22.1f), Quaternion.Euler(0, 180, 0), 0);
        }
        else
        {
            Player = PhotonNetwork.Instantiate(
            "OVRCameraRig2", new Vector3(0, 26.946f, 18.412f), Quaternion.Euler(0, 180, 0), 0);
            PlayerModel = PhotonNetwork.Instantiate(
            "Player2", new Vector3(7.8809e-06f, 0, -17), Quaternion.Euler(0, 0, 0), 0);
        }



    }

    IEnumerator FadeIn()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayersPerRoom;
        JoinGameBT.SetActive(false);
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
