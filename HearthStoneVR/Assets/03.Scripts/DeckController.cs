using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    private Transform handCards1;
    public Transform[] ShuffleDeck1;
    private Transform handCards2;
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
            ShuffleDeck1[i].gameObject.transform.SetParent(handCards1);
            ShuffleDeck2[i].gameObject.transform.SetParent(handCards2);
            ShuffleDeck1[i].gameObject.transform.position = new Vector3(0, 17.27f, -10.83f);
            ShuffleDeck2[i].gameObject.transform.position = new Vector3(0, 17.27f, 15.58f);
            ShuffleDeck1[i].gameObject.transform.rotation = Quaternion.Euler(40, 0, 0);
            ShuffleDeck2[i].gameObject.transform.rotation = Quaternion.Euler(40, 180, 0);
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
