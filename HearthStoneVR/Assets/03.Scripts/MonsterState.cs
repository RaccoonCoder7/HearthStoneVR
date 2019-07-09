using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : Photon.PunBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.isMine)
        {
            
            gameObject.transform.Rotate(new Vector3(0, 0, -90));
        }
        else
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 90));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
