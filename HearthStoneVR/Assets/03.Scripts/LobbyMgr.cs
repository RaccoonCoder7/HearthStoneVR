using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMgr : MonoBehaviour
{
    public Text Information;
    public GameObject JoinGameBT;

    void Start()
    {
        // JoinGameBT.SetActive(false);
    }

    // public override void OnConnectedToMaster()
    // {
    //     JoinGameBT.SetActive(true);
    //     Information.text = "Connected.";
    // }

    public void OnClickJoin(){
        Debug.Log("Join!");
    }
}
