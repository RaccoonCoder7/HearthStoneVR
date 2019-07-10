using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoTurn : Photon.MonoBehaviour, IPunObservable
{
    [PunRPC]
    void TurnMove(Vector3 pos)
    {
        transform.position = pos;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }
}
