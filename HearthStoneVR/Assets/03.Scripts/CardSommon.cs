using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSommon : Photon.MonoBehaviour, IPunObservable
{
    private GameTouchMgr gtm1;
    private GameTouchMgr gtm2;
    public GameObject sommonedCard;
    private bool isCreate = false;
    private PhotonView monsterStatePhoton;
    private DeckController decCtrl;

    private void Start()
    {
        decCtrl = GameObject.Find("DeckController").GetComponent<DeckController>();
    }

    private void Update()
    {
        try
        {
            if (!isCreate && PhotonNetwork.room.PlayerCount == 2)
            {
                isCreate = true;
                gtm1 = GameObject.Find("GameTouchMgr1").GetComponent<GameTouchMgr>();
                gtm2 = GameObject.Find("GameTouchMgr2").GetComponent<GameTouchMgr>();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("not exist GameTouchMgr2 : " + e);
        }
    }

    [PunRPC]
    public void CardInstantiate(Vector3 pos, Quaternion rot, string tag, string myName, PhotonMessageInfo info)
    {
        if (myName == "GameTouchMgr1")
        {
            sommonedCard = Instantiate(Resources.Load("FIELD_" + tag) as GameObject, pos, rot, info.photonView.transform);
            sommonedCard.transform.SetParent(info.photonView.transform);
            monsterStatePhoton = sommonedCard.transform.GetChild(2).GetComponent<PhotonView>();
            monsterStatePhoton.viewID = decCtrl.count;
            decCtrl.count--;
        }
        else
        {
            sommonedCard = Instantiate(Resources.Load("FIELD_" + tag) as GameObject, pos, rot, info.photonView.transform);
            sommonedCard.transform.SetParent(info.photonView.transform);
            monsterStatePhoton = sommonedCard.transform.GetChild(2).GetComponent<PhotonView>();
            monsterStatePhoton.viewID = decCtrl.count;
            decCtrl.count--;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
