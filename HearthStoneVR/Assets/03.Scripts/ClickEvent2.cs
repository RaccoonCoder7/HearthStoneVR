using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvent2 : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Transform[] nowDeck2;
    private DeckController decCtrl;
    private Transform[] child2;

    // Start is called before the first frame update
    void Start()
    {
        decCtrl = GameObject.Find("DeckController").GetComponent<DeckController>();
    }

    // Update is called once per frame
    void Update()
    {
        child2 = GameObject.Find("Deck2").GetComponentsInChildren<Transform>();
        if (child2.Length == 0) return;
        nowDeck2 = new Transform[child2.Length];
        int count = 0;
        for (int i = 0; i < child2.Length; i++)
        {
            if (child2[i].tag == "CARD")
            {
                nowDeck2[count] = child2[i];
                count++;
            }
        }
        if (nowDeck2[0] == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

            if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("TURN")))
            {
                decCtrl.CardSet2(nowDeck2[0].gameObject.transform);
            }
        }
    }
}
