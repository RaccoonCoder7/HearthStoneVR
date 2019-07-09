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
    private int layerField;
    private BoxCollider boxCol;
    private Vector3 originalPos;
    private Renderer infoRend;
    private Material cardBack;
    private GameObject nowDragging;
    private AudioSource audioSource;
    private List<GameObject> sommonedCards = new List<GameObject>();

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
        layerField = LayerMask.NameToLayer("FIELD");
        cam = Camera.main;
        boxCol = myHandCanvas.GetComponentInChildren<BoxCollider>();
        boxCol.enabled = false;
        originalPos = myHandCanvas.transform.position;
        infoRend = InfoCard.GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        infoRend.enabled = false;
        cardBack = Resources.Load("CARDBACK") as Material;
    }

    void Update()
    {
        // cam = Camera.main;
        if (state == TouchState.Disable) return;

        if (state == TouchState.Idle)
        {
            OnIdle();
            return;
        }

        if (state == TouchState.ModelStay || state == TouchState.CardStay)
        {
            OnStay();
            return;
        }
        if (state == TouchState.CardDrag)
        {
            OnDragCard();
            return;
        }

        if (state == TouchState.ModelDrag)
        {
            OnDragModel();
        }
    }

    private void OnDragCard()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            // TODO: 화살표 그리기
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
        }

        if (Input.GetMouseButtonUp(0))
        {
            audioSource.Stop();
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                int layer = hit.transform.gameObject.layer;
                if (layer == layerField)
                {
                    string tag = nowDragging.tag;
                    // GameObject sommonCard = Resources.Load("FIELD_" + tag) as GameObject;
                    GameObject sommonedCard = Instantiate(Resources.Load("FIELD_" + tag) as GameObject,
                                             hit.transform.position, hit.transform.rotation, hit.transform);
                    Destroy(nowDragging);
                    MaximizeHand();
                    state = TouchState.Idle;
                    infoRend.enabled = false;
                    //TODO: 턴 종료시 포문으로 canAttack바꾸기
                    sommonedCards.Add(sommonedCard);
                }
                state = TouchState.Idle;
            }
            return;
        }
    }

    private void OnDragModel()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            // TODO: 화살표 그리기
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                int layer = hit.transform.gameObject.layer;
                if (layer == layerFieldCard)
                {
                    // 전투
                    CardState myCardState = nowDragging.GetComponent<CardState>();
                    CardState enemyCardState = hit.collider.GetComponent<CardState>();
                    Debug.Log("MineAttack: " + myCardState.Attack);
                    Debug.Log("MineHP: " + myCardState.HP);
                    Debug.Log("EnemyAttack: " + enemyCardState.Attack);
                    Debug.Log("EnemyHP: " + enemyCardState.HP);
                    myCardState.playAttackSound();
                    enemyCardState.HP -= myCardState.Attack;
                    myCardState.HP -= enemyCardState.Attack;
                    //TODO: UI변경, 하수인죽기, 애니메이션

                    state = TouchState.Idle;
                    infoRend.enabled = false;
                }
                state = TouchState.Idle;
            }
            state = TouchState.Idle;
            infoRend.enabled = false;
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
            if (state == TouchState.CardStay)
            {
                // TODO: 소환 가능한 카드인지? 마나체크.
                // if(nowDragging.GetComponent<CardState>().cost > mana){
                //     return;
                // }
                // 가능할 시,
                MinimizeHand();
                state = TouchState.CardDrag;
                return;
            }
            if (state == TouchState.ModelStay)
            {
                // TODO: 공격 가능한 카드인지? 공격가능여부체크.
                // 가능할 시,
                // state = TouchState.ModelDrag;
                // return;
            }
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
                    nowDragging = hit.collider.gameObject;
                    state = TouchState.ModelStay;
                    nowLayer = layerFieldCard;
                    MinimizeHand();
                    SetInfoPanelMaterial();
                }
                else if (layer == layerHandCard)
                {
                    nowDragging = hit.collider.gameObject;
                    state = TouchState.CardStay;
                    nowLayer = layerHandCard;
                    SetInfoPanelMaterial();
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
                        if (child[i].gameObject.layer == layerHandCard)
                        {
                            nowDeck[count] = child[i];
                            count++;
                        }
                    }

                    count = 0;
                    for (int i = 0; i < child2.Length; i++)
                    {
                        if (child2[i].gameObject.layer == layerHandCard)
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
                MinimizeHand();
            }
        }
    }

    private void SetInfoPanelMaterial()
    {
        infoRend.enabled = true;
        Material[] materials = new Material[] { Resources.Load(hit.transform.gameObject.tag) as Material, cardBack };
        infoRend.materials = materials;
    }

    private void MinimizeHand()
    {
        if (!boxCol.enabled)
        {
            boxCol.enabled = true;
            myHandCanvas.transform.localScale = Vector3.one * 0.5f;
            myHandCanvas.transform.position = myHandCanvas.transform.position + new Vector3(15, 0, -3.2f);
        }
    }

    private void MaximizeHand()
    {
        if (boxCol.enabled)
        {
            boxCol.enabled = false;
            myHandCanvas.transform.localScale = Vector3.one;
            myHandCanvas.transform.position = myHandCanvas.transform.position + new Vector3(-15, 0, 3.2f);
        }
    }
}
