using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySerializeController1 : Photon.PunBehaviour, IPunObservable
{
    public bool turnChange = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(turnChange);
        }
        else
        {
            // Network player, receive data
            this.turnChange = (bool)stream.ReceiveNext();
        }
    }
}
