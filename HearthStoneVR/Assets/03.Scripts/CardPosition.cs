using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPosition : Photon.MonoBehaviour, IPunObservable
{
    private Transform handCards;
    [PunRPC]
    void CardMove(Vector3 pos, Quaternion rot, bool whoDeck)
    {
        if (whoDeck)
        {
            handCards = GameObject.Find("HandCanvas/HandCards").GetComponent<Transform>();
        }
        else
        {

            handCards = GameObject.Find("HandCanvas2/HandCards").GetComponent<Transform>();
        }
        transform.SetParent(handCards);
        transform.position = pos;
        transform.rotation = rot;
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
