using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerActive : Photon.PunBehaviour
{

    public GameObject rig;
    // Start is called before the first frame update
    void Awake()
    {
        if (photonView.isMine)
        {
            rig.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
