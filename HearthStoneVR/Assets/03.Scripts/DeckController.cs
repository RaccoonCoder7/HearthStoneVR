using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    [HideInInspector]
    public Transform handCards1;
    public Transform[] ShuffleDeck1;
    [HideInInspector]
    public Transform handCards2;
    public Transform[] ShuffleDeck2;

    // Start is called before the first frame update
    void Start()
    {
        handCards1 = GameObject.Find("HandCanvas/HandCards").GetComponent<Transform>();
        handCards2 = GameObject.Find("HandCanvas2/HandCards").GetComponent<Transform>();
        ShuffleArray(ShuffleDeck1);
        ShuffleArray(ShuffleDeck2);
        for (int i = 0; i < 5; ++i)
        {
            CardSet(ShuffleDeck1[i].gameObject.transform, ShuffleDeck2[i].gameObject.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardSet(Transform deck1, Transform deck2)
    {
        deck1.SetParent(handCards1);
        deck2.SetParent(handCards2);
        deck1.position = new Vector3(0, 17.27f, -10.83f);
        deck2.position = new Vector3(0, 17.27f, 15.58f);
        deck1.rotation = Quaternion.Euler(40, 0, 0);
        deck2.rotation = Quaternion.Euler(40, 180, 0);
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

        for (int index = 0; index < cardDeck.Length; ++index)
        {
            random1 = UnityEngine.Random.Range(0, cardDeck.Length);
            random2 = UnityEngine.Random.Range(0, cardDeck.Length);

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
