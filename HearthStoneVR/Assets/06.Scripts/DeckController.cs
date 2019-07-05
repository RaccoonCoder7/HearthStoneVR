using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public GameObject handCards;
    private Transform[] cardDeck;
    // Start is called before the first frame update
    void Start()
    {
        cardDeck = handCards.GetComponentsInChildren<Transform>();
        ShuffleArray(cardDeck);
        for (int i = 1; i < 6; ++i)
        {
            Debug.Log(cardDeck[i].gameObject.name);
            Vector3 cardPos = cardDeck[i].gameObject.transform.position;
            cardPos = new Vector3(cardPos.x, cardPos.y + 1, cardPos.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShuffleArray<T>(T[] cardDeck)
    {
        int random1;
        int random2;

        T tmp;

        for (int index = 1; index < cardDeck.Length; ++index)
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
