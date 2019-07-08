using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTouchMgr : Photon.PunBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    private int nowLayer;
    private int layerFieldCard;
    private int layerHandCard;
    private int layerTurn;
    private int layerHandCanvas;
    private BoxCollider boxCol;
    private Vector3 originalPos;
    private Renderer infoRend;
    private Material cardBack;
    public GameObject myHandCanvas;
    public GameObject InfoCard;

    private Transform[] nowDeck;
    private Transform[] nowDeck2;
    private DeckController decCtrl;
    private Transform[] child;
    private Transform[] child2;
    private GameObject deck;
    private GameObject deck2;

    enum TouchState { Idle, CardStay, CardDrag, ModelStay, ModelDrag, Disable };
    TouchState state = TouchState.Idle;

    void Start()
    {
        layerHandCard = LayerMask.NameToLayer("HANDCARD");
        layerFieldCard = LayerMask.NameToLayer("FIELDCARD");
        layerTurn = LayerMask.NameToLayer("TURN");
        layerHandCanvas = LayerMask.NameToLayer("HANDCANVAS");
        boxCol = myHandCanvas.GetComponentInChildren<BoxCollider>();
        boxCol.enabled = false;
        originalPos = myHandCanvas.transform.position;
        infoRend = InfoCard.GetComponent<Renderer>();
        infoRend.enabled = false;
        cardBack = Resources.Load("CARDBACK") as Material;
    }

    void Update()
    {
        cam = Camera.main;
        if (state == TouchState.Disable) return;

        if (state == TouchState.Idle)
        {
            OnIdle();
        }

        if (state == TouchState.ModelStay || state == TouchState.CardStay)
        {
            OnStay();
        }

        if (state == TouchState.CardDrag)
        {
            // TODO: buttonUp 에서 타겟팅한 오브젝트를 확인 후 할 동작을 정하고 실행.
            return;
        }
    }

    private void OnStay()
    {
        if (Input.GetMouseButtonUp(0))
        {
            state = TouchState.Idle;
            infoRend.enabled = false;
            return;
        }
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 100.0f, 1 << nowLayer))
        {
            state = state == TouchState.CardStay ? TouchState.CardDrag : TouchState.ModelDrag;
            // infoRend.enabled = false;
        }
    }

    private void OnIdle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                int layer = hit.transform.gameObject.layer;
                if (layer == layerFieldCard)
                {
                    state = TouchState.ModelStay;
                    nowLayer = layerFieldCard;
                    SetInfoPanelMaterial();
                    // infoRend.enabled = true;
                    // infoRend.materials[0] = Resources.Load(hit.transform.gameObject.tag) as Material;
                }
                else if (layer == layerHandCard)
                {
                    state = TouchState.CardStay;
                    nowLayer = layerHandCard;
                    SetInfoPanelMaterial();
                    // infoRend.enabled = true;
                    // infoRend.materials[0] = Resources.Load(hit.transform.gameObject.tag) as Material;
                }
            }
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                int layer = hit.transform.gameObject.layer;
                bool toMaximize = false;
                if (layer == layerHandCanvas)
                {
                    toMaximize = true;
                }
                if (layer == layerTurn)
                {
                    deck = GameObject.Find("Deck");
                    deck2 = GameObject.Find("Deck2");

                    child = deck.GetComponentsInChildren<Transform>();
                    child2 = deck.GetComponentsInChildren<Transform>();

                    nowDeck = new Transform[child.Length];
                    nowDeck2 = new Transform[child2.Length];

                    int count = 0;
                    for (int i = 0; i < child.Length; i++)
                    {
                        if (child[i].gameObject.layer == LayerMask.NameToLayer("HANDCARD"))
                        {
                            nowDeck[count] = child[i];
                            count++;
                        }
                    }

                    count = 0;
                    for (int i = 0; i < child2.Length; i++)
                    {
                        if (child2[i].gameObject.layer == LayerMask.NameToLayer("HANDCARD"))
                        {
                            nowDeck2[count] = child2[i];
                            count++;
                        }
                    }

                    if (gameObject.name == "GameTouchMgr1")
                    {
                        decCtrl.CardSet1(nowDeck[0].gameObject.transform);
                    }
                    else if (gameObject.name == "GameTouchMgr2")
                    {
                        decCtrl.CardSet2(nowDeck2[0].gameObject.transform);
                    }

                    //if (turnChange)
                    //{
                    //    decCtrl.CardSet1(nowDeck[0].gameObject.transform);
                    //    turnChange = false;
                    //    clickEventTurn.turnChange = true;
                    //}
                }
                float nowScale = myHandCanvas.transform.localScale.x;
                float scale = toMaximize ? 1 : 0.5f;

                if (nowScale != scale)
                {
                    boxCol.enabled = scale < 1;
                    myHandCanvas.transform.localScale = Vector3.one * scale;
                    if (boxCol.enabled)
                    {
                        myHandCanvas.transform.position = myHandCanvas.transform.position + new Vector3(15, 0, -3.2f);
                    }
                    else
                    {
                        myHandCanvas.transform.position = myHandCanvas.transform.position + new Vector3(-15, 0, 3.2f);
                    }
                }
            }
            else
            {
                if (!boxCol.enabled)
                {
                    boxCol.enabled = true;
                    myHandCanvas.transform.localScale = Vector3.one * 0.5f;
                    myHandCanvas.transform.position = myHandCanvas.transform.position + new Vector3(15, 0, -3.2f);
                }
            }
        }
    }

    private void SetInfoPanelMaterial()
    {
        infoRend.enabled = true;
        Material[] materials = new Material[] { Resources.Load(hit.transform.gameObject.tag) as Material, cardBack };
        infoRend.materials = materials;
    }
}
