using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : Photon.PunBehaviour
{
    [HideInInspector]
    public Transform handCards1;
    [HideInInspector]
    public Transform handCards2;
    public Transform[] ShuffleDeck1;
    public Transform[] ShuffleDeck2;
    private bool isCreate = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isCreate && GameObject.Find("HandCanvas/HandCards") && GameObject.Find("HandCanvas/HandCards"))
        {
            handCards1 = GameObject.Find("HandCanvas/HandCards").GetComponent<Transform>();
            handCards2 = GameObject.Find("HandCanvas2/HandCards").GetComponent<Transform>();
            ShuffleArray(ShuffleDeck1);
            ShuffleArray(ShuffleDeck2);
            for (int i = 1; i < 6; ++i)
            {
                CardSet1(ShuffleDeck1[i].gameObject.transform);
                CardSet2(ShuffleDeck2[i].gameObject.transform);
            }
            isCreate = true;
        }
    }

    public void CardSet1(Transform deck1)
    {
        deck1.SetParent(handCards1);
        deck1.position = new Vector3(0, 17.27f, -10.83f);
        deck1.rotation = Quaternion.Euler(40, 0, 0);
    }

    public void CardSet2(Transform deck2)
    {
        deck2.SetParent(handCards2);
        deck2.position = new Vector3(0, 17.27f, 15.58f);
        deck2.rotation = Quaternion.Euler(40, 180, 0);
    }

    private void ShuffleArray<T>(T[] cardDeck)
    {
        int random1;
        int random2;

        T tmp;

        for (int index = 1; index < cardDeck.Length; ++index)
        {
            random1 = UnityEngine.Random.Range(1, cardDeck.Length);
            random2 = UnityEngine.Random.Range(1, cardDeck.Length);

            tmp = cardDeck[random1];
            cardDeck[random1] = cardDeck[random2];
            cardDeck[random2] = tmp;
        }
    }

    public void TurnOver()
    {
        Debug.Log("over");
    }
}
