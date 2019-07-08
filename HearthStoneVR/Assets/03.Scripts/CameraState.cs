using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraState : Photon.PunBehaviour
{

    public 
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.isMine)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
