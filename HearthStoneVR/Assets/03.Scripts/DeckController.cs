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
    public bool isMyTurn = false;
    private PhotonView cardMovePhoton;

    private void Update()
    {
        Initite();
    }

    public void Initite()
    {
        try
        {
            if (!isCreate && PhotonNetwork.room.PlayerCount == 2)
            {
                isCreate = true;
                handCards1 = GameObject.Find("HandCanvas/HandCards").GetComponent<Transform>();
                handCards2 = GameObject.Find("HandCanvas2/HandCards").GetComponent<Transform>();
                //ShuffleArray(ShuffleDeck1);
                //ShuffleArray(ShuffleDeck2);
                for (int i = 1; i < 6; ++i)
                {
                    CardSet1(ShuffleDeck1[i].gameObject);
                    CardSet2(ShuffleDeck2[i].gameObject);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("not exist HandCanvas2/HandCards : " + e);
        }
    }

    public void CardSet1(GameObject deck1)
    {
        cardMovePhoton = deck1.GetComponent<PhotonView>();
        deck1.transform.SetParent(handCards1);
        deck1.transform.position = new Vector3(0, 17.27f, -10.83f);
        deck1.transform.rotation = Quaternion.Euler(40, 0, 0);
        try
        {
            cardMovePhoton.photonView.RPC("CardMove", PhotonTargets.All,
                               deck1.transform.position, deck1.transform.rotation, true);
        }
        catch (System.Exception e)
        {
            Debug.Log("DeckController: " + e);
            isCreate = false;
            return;
        }

    }

    public void CardSet2(GameObject deck2)
    {
        cardMovePhoton = deck2.GetComponent<PhotonView>();
        deck2.transform.SetParent(handCards2);
        deck2.transform.position = new Vector3(0, 17.27f, 15.58f);
        deck2.transform.rotation = Quaternion.Euler(40, 180, 0);
        try
        {
            cardMovePhoton.photonView.RPC("CardMove", PhotonTargets.All,
                                   deck2.transform.position, deck2.transform.rotation, false);
        }
        catch (System.Exception e)
        {
            Debug.Log("DeckController: " + e);
            isCreate = false;
            return;
        }
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