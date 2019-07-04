using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMgr : MonoBehaviour
{
    public GameObject panelCenter;

    void Start()
    {
        Hashtable ht2 = GetHashTB();
        ht2.Add("y", 0);
        ht2.Add("oncompletetarget", this.gameObject);
        ht2.Add("oncomplete", "RotatePanel3");
        iTween.RotateTo(panelCenter, ht2);
    }

    private Hashtable GetHashTB(){
        Hashtable ht = new Hashtable();
        ht.Add("time", 1f);
        ht.Add("easetype", iTween.EaseType.easeOutElastic);
        return ht;
    }
}
