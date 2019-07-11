using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTouchMgr2 : Photon.PunBehaviour
{
    private Transform tr;
    private LineRenderer line;

    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    private int nowLayer;
    private int layerFieldCard;
    private int layerHandCard;
    private int layerTurn;
    private int layerHandCanvas;
    private int layerField;
    private int layerPlayer;
    private BoxCollider boxCol;
    private Vector3 originalPos;
    private Renderer infoRend;
    private Material cardBack;
    private GameObject nowDragging;
    private AudioSource audioSource;
    private List<GameObject> sommonedCards = new List<GameObject>();

    public GameObject myHandCanvas;
    public GameObject InfoCard;
    public ArrowRenderer arrowRenderer;

    private Transform[] nowDeck;
    private DeckController decCtrl;
    private Transform[] child;
    private GameObject deck;
    private PhotonView turnButtonPhoton;


    enum TouchState { Idle, CardStay, CardDrag, ModelStay, ModelDrag, Disable };
    TouchState state = TouchState.Idle;

    private void Awake()
    {
        infoRend = InfoCard.GetComponent<Renderer>();
        infoRend.enabled = false;
    }
    void Start()
    {
        tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();

        layerHandCard = LayerMask.NameToLayer("HANDCARD");
        layerFieldCard = LayerMask.NameToLayer("FIELDCARD");
        layerTurn = LayerMask.NameToLayer("TURN");
        layerHandCanvas = LayerMask.NameToLayer("HANDCANVAS");
        layerField = LayerMask.NameToLayer("FIELD");
        layerPlayer = LayerMask.NameToLayer("PLAYER");
        cam = Camera.main;
        boxCol = myHandCanvas.GetComponentInChildren<BoxCollider>();
        boxCol.enabled = false;
        originalPos = myHandCanvas.transform.position;
        infoRend = InfoCard.GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        infoRend.enabled = false;
        cardBack = Resources.Load("CARDBACK") as Material;
        decCtrl = GameObject.Find("DeckController").GetComponent<DeckController>();
        turnButtonPhoton = GameObject.Find("TurnBtton").GetComponent<PhotonView>();
        if (!gameObject.GetComponentInParent<PhotonView>().isMine) state = TouchState.Disable;
    }

    void Update()
    {
        if (PhotonNetwork.room.PlayerCount == 2)
        {
            cam = Camera.main;
        }

        ray = new Ray(tr.position, tr.forward);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            float dist = hit.distance;
            line.SetPosition(1, new Vector3(0, 0, dist));
        }

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
        // ray = new Ray(tr.position, tr.forward);
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.green);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                 arrowRenderer.SetPositions(nowDragging.transform.position, hit.point);
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            audioSource.Stop();
             arrowRenderer.SetPositions(new Vector3(0, -10, 0), new Vector3(0, -10, 0));
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
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
            }
            infoRend.enabled = false;
            state = TouchState.Idle;
        }
    }

    private void OnDragModel()
    {
        // ray = new Ray(tr.position, tr.forward);
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.green);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                 arrowRenderer.SetPositions(nowDragging.transform.position, hit.point);
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
             arrowRenderer.SetPositions(new Vector3(0, -10, 0), new Vector3(0, -10, 0));
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                int layer = hit.transform.gameObject.layer;
                if (layer == layerFieldCard)
                {
                    // 전투
                    CardState myCardState = nowDragging.GetComponent<CardState>();
                    CardState enemyCardState;
                    if (hit.collider.tag == "MODEL")
                    {
                        enemyCardState = hit.transform.parent.GetComponent<CardState>();
                    }
                    else
                    {
                        enemyCardState = hit.collider.GetComponent<CardState>();
                    }
                    Debug.Log("MineAttack: " + myCardState.Attack);
                    Debug.Log("MineHP: " + myCardState.HP);
                    Debug.Log("EnemyAttack: " + enemyCardState.Attack);
                    Debug.Log("EnemyHP: " + enemyCardState.HP);
                    myCardState.doAttack(enemyCardState.transform.position);
                    enemyCardState.HP -= myCardState.Attack;
                    myCardState.HP -= enemyCardState.Attack;
                    StartCoroutine(EnemyAttack(myCardState, enemyCardState));
                    //TODO: UI변경, 하수인죽기, 애니메이션

                    state = TouchState.Idle;
                    infoRend.enabled = false;
                    myCardState.canAttack = false;
                    //TODO: 턴 종료시 포문으로 canAttack바꾸기
                    sommonedCards.Add(nowDragging);
                }
                else if (layer == layerPlayer)
                {
                    CardState myCardState = nowDragging.GetComponent<CardState>();
                    Vector3 playerPos = hit.collider.transform.position;
                    playerPos.y += 20;
                    myCardState.doAttack(playerPos);
                    GameObject.Find("GameMgr").GetComponent<GameManager>().EndGame();
                    //TODO: 영웅공격 구현: 게임종료
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
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            state = TouchState.Idle;
            infoRend.enabled = false;
            return;
        }
        // ray = new Ray(tr.position, tr.forward);
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << nowLayer))
        {
            if (state == TouchState.CardStay)
            {
                // TODO: 소환 가능한 카드인지? 마나체크.
                // if(nowDragging.GetComponent<CardState>().cost > mana){
                //     return;
                // }
                // 가능할 시,
                Debug.Log("minimize1");
                MinimizeHand();
                state = TouchState.CardDrag;
                return;
            }
            if (state == TouchState.ModelStay)
            {
                // TODO: 공격 가능한 카드인지? 공격가능여부체크.
                // if(nowDragging.GetComponent<CardState>().canAttack){

                // }

                // 가능할 시,
                state = TouchState.ModelDrag;
                // return;
            }
        }
    }

    private void OnIdle()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            // ray = new Ray(tr.position, tr.forward);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                int layer = hit.transform.gameObject.layer;
                if (layer == layerFieldCard)
                {
                    if (hit.collider.tag == "MODEL")
                    {
                        nowDragging = hit.transform.parent.gameObject;
                    }
                    else
                    {
                        nowDragging = hit.collider.gameObject;
                    }
                    state = TouchState.ModelStay;
                    nowLayer = layerFieldCard;
                    Debug.Log("minimize2");
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
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            // ray = new Ray(tr.position, tr.forward);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                int layer = hit.transform.gameObject.layer;
                bool toMaximize = false;

                if (layer == layerHandCanvas)
                {
                    toMaximize = true;
                }
                else if (layer == layerTurn)
                {
                    Debug.Log(gameObject.name);
                    if (gameObject.name == "GameTouchMgr1" && hit.transform.position.y > 13.0f && hit.transform.position.y < 14.0f)
                    {
                        turnButtonPhoton.photonView.RPC("TurnMove", PhotonTargets.All,
                            new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z));
                        DrawCard("Deck");
                    }
                    else if (gameObject.name == "GameTouchMgr2" && hit.transform.position.y > 14.0f)
                    {
                        turnButtonPhoton.photonView.RPC("TurnMove", PhotonTargets.All,
                               new Vector3(hit.transform.position.x, hit.transform.position.y - 1, hit.transform.position.z));
                        DrawCard("Deck2");
                    }
                    return;
                }
                float nowScale = myHandCanvas.transform.localScale.x;
                float scale = toMaximize ? 1 : 0.5f;

                if (nowScale != scale)
                {
                    boxCol.enabled = scale < 1;
                    myHandCanvas.transform.localScale = Vector3.one * scale;
                    if (boxCol.enabled)
                    {
                        Debug.Log("minimize3");
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
                Debug.Log("minimize4");
                MinimizeHand();
            }
        }
    }

    private void DrawCard(string whatDeck)
    {
        Debug.Log("DrawCard call");
        deck = GameObject.Find(whatDeck);
        child = deck.GetComponentsInChildren<Transform>();
        nowDeck = new Transform[child.Length];
        int count = 0;
        for (int i = 0; i < child.Length; i++)
        {
            if (child[i].gameObject.layer == layerHandCard)
            {
                nowDeck[count] = child[i];
                count++;
            }
        }

        if (whatDeck == "Deck")
        {
            decCtrl.CardSet2(nowDeck[0].gameObject);
        }
        else
        {
            decCtrl.CardSet1(nowDeck[0].gameObject);
        }
    }

    private void SetInfoPanelMaterial()
    {
        infoRend.enabled = true;
        Material[] materials = new Material[] { Resources.Load(nowDragging.tag) as Material, cardBack };
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

    IEnumerator EnemyAttack(CardState myCardState, CardState enemyCardState)
    {
        yield return new WaitForSeconds(0.5f);

        GameObject myDamage = Instantiate(Resources.Load("DamageCanvas") as GameObject, nowDragging.transform.position + new Vector3(0, 10, 0), Quaternion.Euler(0, 90, 0), nowDragging.transform);
        myDamage.SendMessage("SetDamage", myCardState.Attack);
        Transform myHPState = nowDragging.transform.GetChild(2);
        GameObject enemyDamage = Instantiate(Resources.Load("DamageCanvas") as GameObject, enemyCardState.transform.position + new Vector3(0, 10, 0), Quaternion.Euler(0, 90, 0));
        enemyDamage.SendMessage("SetDamage", myCardState.Attack);
        Transform enemyHPState = enemyCardState.transform.GetChild(2);
        Debug.Log(myCardState.HP);
        Debug.Log(enemyCardState.HP);
        myHPState.SendMessage("SetHealth", myCardState.HP);
        enemyHPState.SendMessage("SetHealth", enemyCardState.HP);
    }
}
