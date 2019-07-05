using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvent : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Transform[] nowDeck1;
    private DeckController decCtrl;
    private Transform[] child1;

    // Start is called before the first frame update
    void Start()
    {
        decCtrl = GameObject.Find("DeckController").GetComponent<DeckController>();
    }

    // Update is called once per frame
    void Update()
    {
        child1 = GameObject.Find("Deck").GetComponentsInChildren<Transform>();
        if (child1.Length == 0) return;
        nowDeck1 = new Transform[child1.Length];
        int count = 0;
        for (int i = 0; i < child1.Length; i++)
        {
            if (child1[i].tag == "CARD")
            {
                nowDeck1[count] = child1[i];
                count++;
            }
        }
        if (nowDeck1[0] == null) return;
        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

            if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("TURN")))
            {
                decCtrl.CardSet1(nowDeck1[0].gameObject.transform);
            }
        }
    }
}
